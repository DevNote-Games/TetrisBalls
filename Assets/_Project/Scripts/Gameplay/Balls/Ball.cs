using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour
{
    public event Action<Ball> onExploded;

    [SerializeField] private MeshRenderer _meshRenderer; 
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private BallTrigger _ballTrigger;

    private BallChainsController _ballChainsController;


    public Color Color { get; private set; }


    public int ScoreForBall { get; private set; } public void SetScoreForBall(int score) => ScoreForBall = score;
    public Material Material => _meshRenderer.material;
    public Vector2 Velocity => _rigidbody.velocity;

    public BallType BallType { get; private set; }



    [Inject] private void Construct(BallChainsController ballChainsController)
    {
        _ballChainsController = ballChainsController;
    }


    private void OnEnable()
    {
        _ballTrigger.onBallEnter += OnBallEnter;
        _ballTrigger.onBallExit += OnBallExit;
    }

    private void OnDisable()
    {
        _ballTrigger.onBallEnter -= OnBallEnter;
        _ballTrigger.onBallExit -= OnBallExit;
    }

    private void OnBallEnter(Ball ball)
    {
        _ballChainsController.HandleBallEnter(ball, this);
    }

    private void OnBallExit(Ball ball)
    {
        _ballChainsController.HandleBallsExit(this, ball);
    }


    public void SetType(BallType ballType)
    {
        BallType = ballType;
        _meshRenderer.material = Configs.BallVisual.GetBallMaterial(ballType);
        Color = _meshRenderer.material.color;
    }


    public void TogglePhysics(bool enabled)
    {
        _ballTrigger.gameObject.SetActive(enabled);
        _collider.enabled = enabled;
        _rigidbody.isKinematic = !enabled;
    }

    public void Push(Vector2 direction, float force) => _rigidbody.AddForce(direction.normalized * force, ForceMode2D.Impulse);



    public void RunExplode(float explosionDuration)
    {
        transform.DOKill();
        transform.DOScale(1.3f, explosionDuration).onComplete += () =>
        {
            onExploded?.Invoke(this);
            Destroy(gameObject);
        };
    }



}
