using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class BombBooster : MonoBehaviour
{

    [SerializeField] private float _lifetime = 1.5f;
    [SerializeField] private float _explosionRadius = 1f;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _body;
    [SerializeField] private ParticleSystem _explosionParticles;

    private ParcticleSystemEvents _explosionParticlesEvents;


    [Inject] private BallsController _ballsController;
    [Inject] private LevelController _levelController;

    private Tween _lifetimeTween;


    private void Start()
    {
        _explosionParticles.GetComponent<ParcticleSystemEvents>().onStopped += OnExplosionFinished;
    }


    private void OnEnable()
    {
        _rigidbody.isKinematic = false;
        _body.gameObject.SetActive(true);

        _lifetimeTween?.Kill();
        _lifetimeTween = DOVirtual.DelayedCall(_lifetime, Explode);
    }

    private void OnDisable()
    {
        _lifetimeTween?.Kill();
    }


    private void OnExplosionFinished()
    {
        Destroy(gameObject);
    }


    private void Explode()
    {
        var explodedBalls = new List<Ball>();

        foreach (var ball in _ballsController.AllBalls)
            if (ball.State == BallState.Active && Vector2.Distance(transform.position, ball.transform.position) < _explosionRadius)
                explodedBalls.Add(ball);
        
        int score = Configs.GameRules.GetScoreForChainExplosion(explodedBalls.Count);
        ScoreCalculator.DistributeScore(score, explodedBalls);

        _ballsController.FastExplodeBallChain(explodedBalls);

        _body.gameObject.SetActive(false);
        _rigidbody.isKinematic = true;
        _explosionParticles.Play();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }




}
