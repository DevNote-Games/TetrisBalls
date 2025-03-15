using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VG2;

public class BallSpawnerController
{
    private struct BallChain
    {
        public float beginTime;
        public List<Ball> balls;
    }


    public delegate void OnBallChainHandled(List<Ball> balls);
    public event OnBallChainHandled onBallChainExploded;

    public List<Ball> AllBalls { get; private set; } = new();

    private List<BallGroup> _spawnedBallGroups = new();
    private List<Vector2> _spawnPositions;
    private int _ballGroupsLeft;

    private List<BallChain> _currentExplodingBallChains = new();
    private List<Ball> _handledChainBalls = new();


    private const float BALL_EXPLOSION_DURATION = 0.5f;



    public void SetBallsSpawnPositions(List<Vector2> spawnPositions)
    {
        _spawnPositions = spawnPositions;
    }



    public void OnBallGroupReleased()
    {
        _ballGroupsLeft--;
        if (_ballGroupsLeft == 0) RespawnBalls();
    }

    public void DestroyAllBalls()
    {
        foreach (var ball in AllBalls.ToList())
            Object.Destroy(ball.gameObject);
            
        AllBalls.Clear();
    }

    public void RespawnBalls()
    {
        foreach (var ballGroup in _spawnedBallGroups)
            if (ballGroup != null) Object.Destroy(ballGroup.gameObject);

        _spawnedBallGroups.Clear();

        foreach (var spawnPosition in _spawnPositions)
        {
            var ballGroups = Configs.BallSpawn.BallGroupPrefabs;
            var randomBallGroupPrefab = ballGroups[Random.Range(0, ballGroups.Count)];

            var groupInstance = SceneContainer.InstantiatePrefabFromComponent(randomBallGroupPrefab, spawnPosition, Quaternion.identity);

            var ballTypes = GetRandomBallTypesInsideGroup();
            foreach (var ball in groupInstance.Balls)
            {
                var ballType = ballTypes[Random.Range(0, ballTypes.Count)];
                ball.SetType(ballType);
            }

            _spawnedBallGroups.Add(groupInstance);
        }

        _ballGroupsLeft = _spawnPositions.Count;
    }


    private List<BallType> GetRandomBallTypesInsideGroup()
    {
        var result = new List<BallType>();
        int typesAmount = Random.Range(0, 2) == 0 ? 2 : 3;

        for (int i = 0; i < typesAmount; )
        {
            int index = Random.Range(0, Configs.BallSpawn.SpawnableBallTypes.Count);
            BallType ballType = Configs.BallSpawn.SpawnableBallTypes[index];

            if (result.Contains(ballType) == false)
            {
                result.Add(ballType);
                i++;
            }
            
        }

        return result;
    }




}
