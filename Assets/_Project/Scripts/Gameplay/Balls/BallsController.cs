using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VG2;

public class BallsController
{
    private struct BallChain
    {
        public float beginTime;
        public List<Ball> balls;
    }




    public delegate void OnBallChainExploded(List<Ball> balls, ExplosionType explosionType);
    public event OnBallChainExploded onBallChainExploded;


    public List<Ball> AllBalls { get; private set; } = new();

    private List<SpawnableItem> _spawnedItems = new();
    private List<Vector2> _spawnPositions;
    private List<Ball> _handledChainBalls = new List<Ball>();


    private const float BALL_EXPLOSION_DURATION = 0.5f;



    public void SetBallsSpawnPositions(List<Vector2> spawnPositions)
    {
        _spawnPositions = spawnPositions;
    }



    public void OnItemReleased(SpawnableItem item)
    {
        _spawnedItems.Remove(item);
        if (_spawnedItems.Count == 0) RespawnBalls();
    }

    public void DestroyAllBalls()
    {
        foreach (var ball in AllBalls.ToList())
            Object.Destroy(ball.gameObject);
            
        AllBalls.Clear();
    }

    public void ReplaceRandomItem(SpawnableItem newItemPrefab)
    {
        var newItemInstance = SceneContainer.InstantiatePrefabFromComponent(newItemPrefab);

        int randomIndex = Random.Range(0, _spawnedItems.Count);
        var replacedItem = _spawnedItems[randomIndex];
        _spawnedItems[randomIndex] = newItemInstance;
        newItemInstance.transform.position = replacedItem.transform.position;

        Object.Destroy(replacedItem.gameObject);
    }


    public void RespawnBalls()
    {
        foreach (var item in _spawnedItems)
            if (item != null) Object.Destroy(item.gameObject);

        _spawnedItems.Clear();

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

            _spawnedItems.Add(groupInstance);
        }
    }


    private List<BallType> GetRandomBallTypesInsideGroup()
    {
        //return new List<BallType> { BallType.Green, BallType.Red };

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



    public void HandleChain(Ball ball)
    {
        _handledChainBalls.Clear();
        HandleChainRecursive(ball);

        int ballsAmount = _handledChainBalls.Count;

        if (ballsAmount >= Configs.GameRules.MinBallsChainRequire)
        {
            var explodingChain = new ExplodingBallChain(new List<Ball>(_handledChainBalls));
            explodingChain.onExploded += (balls) =>
            {
                onBallChainExploded?.Invoke(balls, ExplosionType.Chain);

                foreach (var ball in balls)
                    ball.FastExplode();
            };
        }

    }

    public void FastExplodeBallChain(List<Ball> balls, ExplosionType explosionType)
    {
        onBallChainExploded?.Invoke(balls, explosionType);

        foreach (var ball in balls)
            ball.FastExplode();
    }


    private void HandleChainRecursive(Ball ball)
    {
        for (int i = 0; i < ball.ConnectedBalls.Count; i++)
        {
            var connectedBall = ball.ConnectedBalls[i];

            if (connectedBall.IsMatched(ball) && _handledChainBalls.Contains(connectedBall) == false)
            {
                _handledChainBalls.Add(ball.ConnectedBalls[i]);
                HandleChainRecursive(connectedBall);
            }
        }
    }







}
