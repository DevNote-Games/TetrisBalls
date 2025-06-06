using System;
using System.Collections.Generic;
using UnityEngine;


public class EffectsController : IDisposable
{
    private BallSpawnerController _ballSpawnerController;
    private LevelController _levelController;
    private int _lastDecalRenderPriority = 0;


    private const float BLAST_PARTICLE_Z_POSITION = -0.5f;
    private const float SMALL_BALLS_PARTICLE_ALPHA = 0.7f;



    public EffectsController(BallSpawnerController ballsController, LevelController levelController)
    {
        _ballSpawnerController = ballsController;
        _ballSpawnerController.onBallChainExploded += OnBallChainHandled;
        _levelController = levelController;
    }

    public void Dispose()
    {
        _ballSpawnerController.onBallChainExploded -= OnBallChainHandled;
    }


    private void OnBallChainHandled(List<Ball> balls)
    {
        int score = Configs.GameRules.GetScoreForChainExplosion(balls.Count);
        int scoreForBall = score / balls.Count;
        int scoreForLastBall = scoreForBall + score % balls.Count;

        Vector2 averagePosition = Vector2.zero;
        for (int i = 0; i < balls.Count; i++)
        {
            averagePosition += (Vector2)balls[i].transform.position;
            balls[i].onExploded += OnBallExploded;

            if (i != balls.Count - 1) balls[i].SetScoreForBall(scoreForBall);
            else balls[i].SetScoreForBall(scoreForLastBall);

        }
            

        averagePosition /= balls.Count;

        var scorePart = UnityEngine.Object.Instantiate(Configs.BallVisual.ScorePartPrefab, UIContainer.Canvas);
        (scorePart.transform as RectTransform).position = CameraContainer.PerspectiveCamera.WorldToScreenPoint(averagePosition);
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
        var splashDecal = UnityEngine.Object.Instantiate(Configs.BallVisual.SplashDecalPrefab, position, Quaternion.identity);

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
