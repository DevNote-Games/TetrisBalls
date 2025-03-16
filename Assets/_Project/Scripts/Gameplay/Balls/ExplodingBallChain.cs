using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplodingBallChain
{
    public event Action<List<Ball>> onExploded;


    private List<Ball> _explodingBalls;

    public ExplodingBallChain(List<Ball> explodingBalls)
    {
        _explodingBalls = explodingBalls;

        foreach (var ball in explodingBalls)
            ball.Explode();

        explodingBalls[0].onExploded += OnExploded;
    }

    private void OnExploded(Ball explodedBall)
    {
        explodedBall.onExploded -= OnExploded;

        var explodedBalls = new List<Ball>(_explodingBalls);

        foreach (var ball in _explodingBalls)
            ball.AddConnectedUniqueChainBallsToExplodeList(explodedBalls);

        explodedBalls.RemoveAll(ball => ball == null);
        onExploded?.Invoke(explodedBalls.Distinct().ToList());
    }
}
