using System.Collections.Generic;
using UnityEngine;

public class BallSpawnerController
{
    private List<Transform> _spawnPositions;





    public BallSpawnerController(List<Transform> spawnPositions)
    {
        _spawnPositions = spawnPositions;
    }



    public void SpawnBalls()
    {
        foreach (var spawnPosition in _spawnPositions)
        {
            var ballGroups = Configs.BallSpawn.BallGroupPrefabs;
            var randomBallGroupPrefab = ballGroups[Random.Range(0, ballGroups.Count)];

            var groupInstance = Object.Instantiate(randomBallGroupPrefab, spawnPosition.position, Quaternion.identity);

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







}
