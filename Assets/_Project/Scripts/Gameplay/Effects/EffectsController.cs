using System;
using System.Collections.Generic;
using UnityEngine;


public class EffectsController : IDisposable
{
    private BallsController _ballsController;
    private LevelController _levelController;
    private int _lastDecalRenderPriority = 0;
    private GameObject _decalsContainer;


    private const float BLAST_PARTICLE_Z_POSITION = -0.5f;
    private const float SMALL_BALLS_PARTICLE_ALPHA = 0.7f;



    public EffectsController(BallsController ballsController, LevelController levelController)
    {
        _ballsController = ballsController;
        _ballsController.onBallChainExploded += OnBallChainExploded;
        _levelController = levelController;
        _levelController.onLevelStarted += OnLevelStarted;
    }

    public void Dispose()
    {
        _ballsController.onBallChainExploded -= OnBallChainExploded;
    }

    private void OnLevelStarted()
    {
        if (_decalsContainer != null)
            UnityEngine.Object.Destroy(_decalsContainer);
    }


    private void OnBallChainExploded(List<Ball> balls, ExplosionType explosionType)
    {
        Debug.Log(balls.Count);

        int score = Configs.GameRules.GetScoreForChainExplosion(balls.Count);
        ScoreCalculator.DistributeScore(score, balls);

        Vector2 averagePosition = Vector2.zero;
        for (int i = 0; i < balls.Count; i++)
        {
            averagePosition += (Vector2)balls[i].transform.position;
            balls[i].onExploded += OnBallExploded;
        }
            
        averagePosition /= balls.Count;

        var scorePart = UnityEngine.Object.Instantiate(Configs.BallVisual.ScorePartPrefab, UIContainer.VfxCanvas);
        (scorePart.transform as RectTransform).position = CameraContainer.OrthographicCamera.WorldToScreenPoint(averagePosition);
        scorePart.text = score.ToString();
    }

    private void OnBallExploded(Ball ball)
    {
        ball.onExploded -= OnBallExploded;
        
        SpawnSplashDecal(ball.transform.position, ball.Material.color);
        SpawnBlastParticle(ball.transform.position, ball.Material.color);
        SpawnSmallBallsParticle(ball);
    }


    private void SpawnSplashDecal(Vector2 position, Color color)
    {
        if (_decalsContainer == null)
            _decalsContainer = new GameObject("Decals");

        var splashDecal = UnityEngine.Object.Instantiate(Configs.BallVisual.SplashDecalPrefab, _decalsContainer.transform);
        splashDecal.transform.position = position;

        splashDecal.material = new Material(splashDecal.material);
        splashDecal.material.SetFloat("_DrawOrder", _lastDecalRenderPriority);
        _lastDecalRenderPriority++;
        splashDecal.material.SetColor("_Color", color);

        float randomScale = UnityEngine.Random.Range(0.9f, 1.5f);
        splashDecal.transform.localScale = new Vector3(randomScale, randomScale, 1f);
        splashDecal.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));

        splashDecal.material.SetTexture("Base_Map", Configs.BallVisual.GetRandomSplashTexture());
    }

    private void SpawnBlastParticle(Vector2 position, Color color)
    {
        var position3D = new Vector3(position.x, position.y, BLAST_PARTICLE_Z_POSITION);
        var blastParticle = UnityEngine.Object.Instantiate(Configs.BallVisual.BlastParticlePrefab, position3D, Quaternion.identity);
        blastParticle.SetColor(color);
    }

    private void SpawnSmallBallsParticle(Ball ball)
    {
        var particle = UnityEngine.Object.Instantiate(Configs.BallVisual.SmallBallsParticlePrefab, UIContainer.VfxCanvas);
        Vector2 screenPosition = CameraContainer.PerspectiveCamera.WorldToScreenPoint(ball.transform.position);

        particle.transform.position = screenPosition;
        particle.attractorTarget = UIContainer.LevelScore.transform;

        Color color = ball.Material.color;
        color.a = SMALL_BALLS_PARTICLE_ALPHA;
        particle.startColor = color;

        int score = ball.ScoreForBall;

        particle.onAnyParticleFinished.AddListener(() => _levelController.AddScore(score));

        particle.onLastParticleFinished.AddListener(() =>
        {
            particle.onAnyParticleFinished.RemoveAllListeners();
            particle.onLastParticleFinished.RemoveAllListeners();
            UnityEngine.Object.Destroy(particle.gameObject);
        });



    }

}
