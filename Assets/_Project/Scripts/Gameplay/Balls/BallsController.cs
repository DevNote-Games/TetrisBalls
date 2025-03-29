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

    private Level _currentLevel;
    private List<ItemGroup> _spawnedItemGroups = new();
    private List<Ball> _handledChainBalls = new List<Ball>();


    private const float BALL_EXPLOSION_DURATION = 0.5f;



    public void OnItemGroupReleased(ItemGroup item)
    {
        _spawnedItemGroups.Remove(item);
        if (_spawnedItemGroups.Count == 0) RespawnBalls(_currentLevel);
    }

    public void DestroyAllBalls()
    {
        foreach (var ball in AllBalls.ToList())
            Object.Destroy(ball.gameObject);
            
        AllBalls.Clear();
    }

    public void ReplaceRandomItem(ItemGroup newItemGroupPrefab)
    {
        var newItemInstance = SceneContainer.InstantiatePrefabFromComponent(newItemGroupPrefab);

        int randomIndex = Random.Range(0, _spawnedItemGroups.Count);
        var replacedItem = _spawnedItemGroups[randomIndex];
        _spawnedItemGroups[randomIndex] = newItemInstance;
        newItemInstance.transform.position = replacedItem.transform.position;

        Object.Destroy(replacedItem.gameObject);
    }


    public void RespawnBalls(Level currentLevel)
    {
        _currentLevel = currentLevel;

        foreach (var item in _spawnedItemGroups)
            if (item != null) Object.Destroy(item.gameObject);

        _spawnedItemGroups.Clear();

        foreach (var spawnPosition in currentLevel.GetBallGroupSpawnPositions())
        {
            var ballGroups = _currentLevel.BallGroupPrefabs;
            var randomBallGroupPrefab = ballGroups[Random.Range(0, ballGroups.Count)];

            var groupInstance = SceneContainer.InstantiatePrefabFromComponent(randomBallGroupPrefab, spawnPosition, Quaternion.identity);
            groupInstance.SetOriginPosition(spawnPosition);

            var ballTypes = GetRandomBallTypesInsideGroup();
            foreach (var item in groupInstance.Items)
            {
                var ball = item as Ball;
                var ballType = ballTypes[Random.Range(0, ballTypes.Count)];
                ball.SetType(ballType);
            }

            _spawnedItemGroups.Add(groupInstance);
        }
    }


    private List<BallType> GetRandomBallTypesInsideGroup()
    {
        //return new List<BallType> { BallType.Green, BallType.Red };

        if (_currentLevel.BallTypes.Count <= 2)
            return _currentLevel.BallTypes;

        var result = new List<BallType>();
        int typesAmount = Random.Range(0, 2) == 0 ? 2 : 3;

        for (int i = 0; i < typesAmount; )
        {
            int index = Random.Range(0, _currentLevel.BallTypes.Count);
            BallType ballType = _currentLevel.BallTypes[index];

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
