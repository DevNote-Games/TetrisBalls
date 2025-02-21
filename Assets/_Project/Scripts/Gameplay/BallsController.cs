using System.Collections.Generic;
using UnityEngine;
using VG2;

public class BallsController
{
    private List<Transform> _spawnPositions;
    private List<Ball> _handledShapeBalls = new List<Ball>();


    public BallsController(List<Transform> spawnPositions)
    {
        _spawnPositions = spawnPositions;
    }



    public void SpawnBalls()
    {
        foreach (var spawnPosition in _spawnPositions)
        {
            var ballGroups = Configs.BallSpawn.BallGroupPrefabs;
            var randomBallGroupPrefab = ballGroups[Random.Range(0, ballGroups.Count)];

            var groupInstance = SceneContainer.InstantiatePrefabFromComponent(randomBallGroupPrefab, spawnPosition.position, Quaternion.identity);

            var ballTypes = GetRandomBallTypesInsideGroup();
            foreach (var ball in groupInstance.Balls)
            {
                var ballType = ballTypes[Random.Range(0, ballTypes.Count)];
                ball.SetType(ballType);
            }
        }

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



    public void HandleShape(Ball ball)
    {
        _handledShapeBalls.Clear();
        HandleShapeRecursive(ball);

        if (_handledShapeBalls.Count >= Configs.GameRules.MinBallsRequireForExplosion)
        {
            foreach (var handledBall in _handledShapeBalls)
                handledBall.Explode();

        }


    }

    private void HandleShapeRecursive(Ball ball)
    {
        for (int i = 0; i < ball.ConnectedBalls.Count; i++)
        {
            var connectedBall = ball.ConnectedBalls[i];

            if (connectedBall.BallType == ball.BallType && _handledShapeBalls.Contains(connectedBall) == false)
            {
                _handledShapeBalls.Add(ball.ConnectedBalls[i]);
                HandleShapeRecursive(connectedBall);
            }
        }
    }







}
