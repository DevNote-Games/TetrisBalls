using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using VG2;
using Zenject;

public class BombBooster : Item
{

    [SerializeField] private float _lifetime = 1.5f;
    [SerializeField] private float _explosionRadius = 1f;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameObject _body;
    [SerializeField] private ParticleSystem _explosionParticles;
    [SerializeField] private List<MeshRenderer> _meshes;

    private ParcticleSystemEvents _explosionParticlesEvents;


    [Inject] private BallsController _ballsController;
    [Inject] private LevelController _levelController;

    private Tween _lifetimeTween;



    public override void Activate()
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
        _body.gameObject.SetActive(true);

        _explosionParticles.GetComponent<ParcticleSystemEvents>().onStopped += OnExplosionFinished;

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

        _ballsController.FastExplodeBallChain(explodedBalls, ExplosionType.Booster);

        _body.gameObject.SetActive(false);
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector2.zero;
        _explosionParticles.Play();
        Sound.Play(SoundKey.Bomb);
    }


    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }

    public override void SetAvailableForPlacing(bool available)
    {
        _meshes.ForEach(mesh => Graphics.SetTransparentMode(enabled: !available, mesh.material));
    }
}
