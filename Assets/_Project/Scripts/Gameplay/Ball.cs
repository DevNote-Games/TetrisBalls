using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Ball : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private BallTrigger _ballTrigger;

    [Inject] private BallsController _ballsController;

    private bool _exploded = false;



    public List<Ball> ConnectedBalls { get; private set; } = new();

    public BallType BallType { get; private set; }


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
        ConnectedBalls.Add(ball);
        _ballsController.HandleShape(ball);
    }

    private void OnBallExit(Ball ball)
    {
        ConnectedBalls.Remove(ball);
    }

    



    public void SetType(BallType ballType)
    {
        BallType = ballType;
        _meshRenderer.material = Configs.BallVisual.GetBallMaterial(ballType);
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

        ConnectedBalls.Clear();
        transform.DOScale(1.3f, 0.5f).onComplete += () => Destroy(gameObject);

        _exploded = true;
    }



}
