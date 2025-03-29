using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using VG2;
using Zenject;

public class LevelController : IInitializable, ITickable
{
    public event Action onLevelStarted;

    public Observable<Unit> OnScoreChanged => _onScoreChanged; private Subject<Unit> _onScoreChanged = new();



    public int CompletedLevel { get; private set; } = 1;
    public int CurrentScore { get; private set; } = 0;
    public int RequiredScore { get; private set; } = 1;

    private BallsController _ballsController;
    private UIController _uiController;

    private Level _currentLevelInstance; public Level CurrentLevel => _currentLevelInstance;
    private Dictionary<Ball, float> _ballsTimesInsideLimit = new();
    private bool _levelFinished;
    



    public LevelController(BallsController ballsController, UIController uiController)
    {
        _ballsController = ballsController;
        _uiController = uiController;
    }


    public void AddScore(int score)
    {
        if (_levelFinished) return;

        CurrentScore = Mathf.Clamp(CurrentScore + score, 0, RequiredScore);

        if (CurrentScore == RequiredScore)
            Win();

        _onScoreChanged?.OnNext(Unit.Default);
    }


    public void Initialize()
    {
        StartLevel(GameState.Level);
    }

    public void StartLevel(int levelNumber)
    {
        _ballsController.DestroyAllBalls();

        if (_currentLevelInstance != null)
            UnityEngine.Object.Destroy(_currentLevelInstance.gameObject);

        CurrentScore = 0;
        RequiredScore = Configs.Levels.GetLevelRequiredScore(levelNumber);
        _onScoreChanged?.OnNext(Unit.Default);

        GameState.Level = levelNumber;

        _currentLevelInstance = SceneContainer.InstantiatePrefabFromComponent(Configs.Levels.GetLevelPrefab(levelNumber));

        CameraContainer.AdjustCamerasToRect(_currentLevelInstance.CameraArea);
        _ballsController.RespawnBalls(_currentLevelInstance);

        _levelFinished = false;
        onLevelStarted?.Invoke();

        Ads.Interstitial.Show(AdKey.Interstitial);
    }

    public void AddBallToLimit(Ball ball)
    {
        _ballsTimesInsideLimit.Add(ball, Configs.GameRules.TimeToLimitLose);
    }

    public void RemoveBallFromLimit(Ball ball)
    {
        _ballsTimesInsideLimit.Remove(ball);
    }


    private void Lose()
    {
        _uiController.ShowLoseScreen();
        _levelFinished = true;
    }

    private void Win()
    {
        _uiController.ShowVictoryScreen();
        _levelFinished = true;
    }

    public void Tick()
    {
        HandleCheckLose();
    }

    private void HandleCheckLose()
    {
        if (_levelFinished) return;

        foreach (var key in _ballsTimesInsideLimit.Keys.ToList())
        {
            _ballsTimesInsideLimit[key] -= Time.deltaTime;
            if (_ballsTimesInsideLimit[key] <= 0f)
            {
                Lose();
                return;
            }
        }
    }


}
