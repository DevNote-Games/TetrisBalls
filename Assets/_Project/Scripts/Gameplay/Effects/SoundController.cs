using DG.Tweening;
using UnityEngine;
using VG2;
using Zenject;

public class SoundController : IInitializable
{
    private LevelController _levelController;
    private bool _canPlayBallExplosion = true;
    private bool _canPlayBallCollision = true;

    private float PLAY_BALL_COLLISION_BARRIER_DURATION = 0.2f;
    private float PLAY_BALL_EXPLOSION_BARRIER_DURATION = 0.2f;
    private float PLAY_BALL_EXPLOSION_REPEAT_TIME = 0.02f;


    public SoundController(LevelController levelController)
    {
        _levelController = levelController;
    }

    public void Initialize()
    {
        Sound.Play(SoundKey.Music);
    }


    public void PlayBallCollision()
    {
        if (!_canPlayBallCollision) return;

        Sound.Play(SoundKey.BallPop);

        DOVirtual.DelayedCall(PLAY_BALL_COLLISION_BARRIER_DURATION, () => _canPlayBallCollision = true);
        _canPlayBallCollision = false;
    }

    public void PlayBallExplosion()
    {
        if (!_canPlayBallExplosion) return;

        Sound.Play(SoundKey.BallExplosion);
        DOVirtual.DelayedCall(PLAY_BALL_EXPLOSION_REPEAT_TIME, () => Sound.Play(SoundKey.BallExplosion));
        DOVirtual.DelayedCall(PLAY_BALL_EXPLOSION_REPEAT_TIME * 2f, () => Sound.Play(SoundKey.BallExplosion));


        Sound.Play(SoundKey.BallSplash);
        _canPlayBallExplosion = false;
        DOVirtual.DelayedCall(PLAY_BALL_EXPLOSION_BARRIER_DURATION, () => _canPlayBallExplosion = true);
    }


    public void PlayScoreFill()
    {
        const float minPitch = 0.9f;
        const float maxPitch = 1.1f;

        float pitch = Mathf.Lerp(minPitch, maxPitch, (float)_levelController.CurrentScore / _levelController.RequiredScore);
        Sound.Play(SoundKey.ScoreFill).pitch = pitch;
    }




}
