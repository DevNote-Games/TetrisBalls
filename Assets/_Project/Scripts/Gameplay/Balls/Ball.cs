using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Ball : Item
{
    public event Action<Ball> onExploded;

    [SerializeField] private bool _activateOnStart;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private BallTrigger _ballTrigger;

    private BallsController _ballsController;
    private SoundController _soundController;

    private const float TRANSPARENT_MODE_ALPHA = 0.5f;

    public Color Color { get; private set; }


    public BallState State { get; set; } = BallState.Active;


    public int ScoreForBall { get; private set; } public void SetScoreForBall(int score) => ScoreForBall = score;
    public Material Material => _meshRenderer.material;
    public Vector2 Velocity => _rigidbody.velocity;



    public List<Ball> ConnectedBalls { get; private set; } = new();

    [field: SerializeField] public BallType BallType { get; private set; }



    [Inject] private void Construct(BallsController ballsController, SoundController soundController)
    {
        _ballsController = ballsController;
        _soundController = soundController;
        _ballsController.AllBalls.Add(this);
    }


    private void OnEnable()
    {
        State = BallState.Inventory;
        TogglePhysics(false);

        _ballTrigger.onBallEnter += OnBallEnter;
        _ballTrigger.onBallExit += OnBallExit;

        _ballsController?.AllBalls.Add(this);

        if (_activateOnStart)
        {
            SetType(BallType);
            Activate();
            _rigidbody.isKinematic = true;
        }
    }

    private void OnDisable()
    {
        _ballTrigger.onBallEnter -= OnBallEnter;
        _ballTrigger.onBallExit -= OnBallExit;
        _ballsController.AllBalls.Remove(this);
    }

    private void OnBallEnter(Ball ball)
    {
        ConnectedBalls.Add(ball);

        if (ball.State != BallState.Exploding && State != BallState.Exploding)
            _ballsController.HandleChain(ball);
    }

    private void OnBallExit(Ball ball)
    {
        ConnectedBalls.Remove(ball);
    }

    public bool IsMatched(Ball ball) => BallType == ball.BallType;



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



    public void AddConnectedUniqueChainBallsToExplodeList(List<Ball> balls)
    {
        foreach (Ball ball in ConnectedBalls)
            if (!balls.Contains(ball) && IsMatched(ball))
            {
                balls.Add(ball);
                ball.AddConnectedUniqueChainBallsToExplodeList(balls);
            }
    }

    public void Explode()
    {
        if (State == BallState.Exploding) return;

        transform.DOScale(1.3f, 0.5f).onComplete += () =>
        {
            onExploded?.Invoke(this);

            foreach (var connectedBall in ConnectedBalls)
                connectedBall.ConnectedBalls.Remove(this);

            _soundController.PlayBallExplosion();
            Destroy(gameObject);
        };

        State = BallState.Exploding;
    }

    public void FastExplode()
    {
        if (State == BallState.Exploding) return;

        foreach (var connectedBall in ConnectedBalls)
            connectedBall.ConnectedBalls.Remove(this);

        onExploded?.Invoke(this);

        _soundController.PlayBallExplosion();
        Destroy(gameObject);

        State = BallState.Exploding;
    }


    public override void SetAvailableForPlacing(bool available) 
        => Graphics.SetTransparentMode(!available, Material);


    public override void Activate()
    {
        State = BallState.Active;
        TogglePhysics(true);
        Graphics.SetTransparentMode(false, Material);
    }

    private void OnValidate()
    {
        _meshRenderer.material = Configs.BallVisual.GetBallMaterial(BallType);
    }
}
