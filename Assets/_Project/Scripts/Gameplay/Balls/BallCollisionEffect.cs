using DG.Tweening;
using UnityEngine;
using Zenject;

public class BallCollisionEffect : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private BallTrigger _ballTrigger;

    [Inject] private SoundController _soundController;


    private bool _isPlaying;
    private Sequence _currentSequence;
    private float DURATION = 0.15f;
    private float VELOCITY_REQUIRE = 3.5f;



    private void OnEnable()
    {
        _ballTrigger.onBallEnter += OnBallEnter;
    }

    private void OnDisable()
    {
        _ballTrigger.onBallEnter -= OnBallEnter;
        _currentSequence?.Kill();
    }



    private void OnBallEnter(Ball ball)
    {
        if (_isPlaying) return;
        if (_ball.Velocity.magnitude > VELOCITY_REQUIRE || ball.Velocity.magnitude > VELOCITY_REQUIRE)
        {
            _soundController.PlayBallCollision();
            _currentSequence?.Kill();
            _currentSequence = DOTween.Sequence()
                .AppendCallback(() => _isPlaying = true)
                .Append(_ball.Material.DOColor(Color.white, DURATION / 2f).SetEase(Ease.OutQuad))
                .Append(_ball.Material.DOColor(_ball.Color, DURATION / 2f).SetEase(Ease.InQuad))
                .AppendCallback(() => _isPlaying = false);
        }
    }
}
