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

    private BallsController _ballsController;


    public Color Color { get; private set; }

    private bool _exploded = false;


    public int ScoreForBall { get; private set; } public void SetScoreForBall(int score) => ScoreForBall = score;
    public Material Material => _meshRenderer.material;
    public Vector2 Velocity => _rigidbody.velocity;



    public List<Ball> ConnectedBalls { get; private set; } = new();

    public BallType BallType { get; private set; }



    [Inject] private void Construct(BallsController ballsController)
    {
        _ballsController = ballsController;
        _ballsController.AllBalls.Add(this);
    }


    private void OnEnable()
    {
        _ballTrigger.onBallEnter += OnBallEnter;
        _ballTrigger.onBallExit += OnBallExit;

        _ballsController?.AllBalls.Add(this);
    }

    private void OnDisable()
    {
        _ballTrigger.onBallEnter -= OnBallEnter;
        _ballTrigger.onBallExit -= OnBallExit;
        _ballsController.AllBalls.Remove(this);
    }

    private void OnBallEnter(Ball ball)
    {
        if (!ball._exploded)
        {
            ConnectedBalls.Add(ball);
            _ballsController.HandleChain(ball);
        }
        
    }

    private void OnBallExit(Ball ball)
    {
        ConnectedBalls.Remove(ball);
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



    public void Explode()
    {
        if (_exploded) return;

        foreach (var connectedBall in ConnectedBalls)
            connectedBall.ConnectedBalls.Remove(this);

        //ConnectedBalls.Clear();
        transform.DOScale(1.3f, 0.5f).onComplete += () =>
        {
            

            onExploded?.Invoke(this);
            Destroy(gameObject);
        };

        _exploded = true;
    }



}
