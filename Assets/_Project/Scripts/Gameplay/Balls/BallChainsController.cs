using System;
using System.Collections.Generic;
using UnityEngine;

public class BallChainsController
{

    private class Chain
    {
        public event Action<Chain> onExploded;

        public bool RunExplosion { get; private set; } = false;
        private float _explosionBeginTime;
        public List<Ball> Balls { get; private set; }


        public Chain(List<Ball> balls) => Balls = balls;

        public void MergeExplosionBeginTime(Chain chain)
        {
            if (RunExplosion) 
                _explosionBeginTime = Mathf.Min(_explosionBeginTime, chain._explosionBeginTime);

            else
            {
                RunExplosion = true;
                _explosionBeginTime = chain._explosionBeginTime;
            }
            
        }


        public void UpdateExplosion()
        {
            if (Balls.Count >= Configs.GameRules.MinBallsChainRequire)
            {
                if (!RunExplosion)
                {
                    RunExplosion = true;
                    _explosionBeginTime = Time.time;
                }

                float passedTimeFromBegin = Time.time - _explosionBeginTime;
                float explosionDuration = Mathf.Max(EXPLOSION_DURATION - passedTimeFromBegin, 0.1f);

                foreach (var ball in Balls)
                    ball.RunExplode(explosionDuration);

                Balls[0].onExploded += OnBallExploded;
            }
        }

        private void OnBallExploded(Ball ball)
        {
            Balls[0].onExploded -= OnBallExploded;
            onExploded?.Invoke(this);
        }
    }

    private LevelController _levelController;
    private Dictionary<Ball, Chain> _ballsByChain = new();

    private const float EXPLOSION_DURATION = 0.5f;



    public BallChainsController(LevelController levelController)
    {
        _levelController = levelController;
    }


    public void HandleBallsExit(Ball owner, Ball leaver)
    {

    }




    public void HandleBallEnter(Ball visitor, Ball receiver)
    {
        // Visitor and receiver is inside same chain
        if (_ballsByChain[visitor] == _ballsByChain[receiver]) return;


        // Ball-visitor doesn't have a chain 
        if (_ballsByChain.ContainsKey(visitor) == false)
        {
            // Ball-receiver have a chain 
            if (_ballsByChain.ContainsKey(receiver))
                AddBallToChain(visitor, _ballsByChain[receiver]);

            // Ball-receiver doesn't have a chain 
            else CreateChain(visitor, receiver);
        }


        // If ball-visitor have a chain 
        else
        {
            // Ball-receiver have a chain 
            if (_ballsByChain.ContainsKey(receiver))
                MergeChains(_ballsByChain[visitor], _ballsByChain[receiver]);

            // Ball-receiver doesn't have a chain 
            else AddBallToChain(receiver, _ballsByChain[visitor]);
        }
    }


    private void CreateChain(Ball first, Ball second)
    {
        var newChain = new Chain(new List<Ball> { first, second });

        _ballsByChain.Add(first, newChain);
        _ballsByChain.Add(second, newChain);

        newChain.onExploded += OnChainExploded;
    }

    private void OnChainExploded(Chain chain)
    {
        chain.onExploded -= OnChainExploded;

        int score = Configs.GameRules.GetScoreForChainExplosion(chain.Balls.Count);
        _levelController.AddScore(score);

    }

    private void AddBallToChain(Ball ball, Chain chain)
    {
        chain.Balls.Add(ball);
        chain.UpdateExplosion();
        _ballsByChain.Add(ball, chain);
    }


    private void MergeChains(Chain first, Chain second)
    {
        foreach (var ball in second.Balls)
        {
            first.Balls.Add(ball);
            _ballsByChain[ball] = first;
        }

        if (second.RunExplosion)
            first.MergeExplosionBeginTime(second);

        first.UpdateExplosion();
    }



}
