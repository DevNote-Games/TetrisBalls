using System.Collections.Generic;
using UnityEngine;
using VG2;


public class ChainBoostersController
{

    public ChainBoostersController(BallsController ballsController)
    {
        ballsController.onBallChainExploded += OnBallChainExploded;
    }


    private void OnBallChainExploded(List<Ball> balls, ExplosionType explosionType)
    {
        if (explosionType != ExplosionType.Chain) return;


        if (balls.Count >= Configs.Boosters.SpawnBombChainBallsRequire)
        {
            int randomBallIndex = Random.Range(0, balls.Count);
            var bomb = SceneContainer.InstantiatePrefabFromComponent(Configs.Boosters.BombPrefab);
            bomb.transform.position = balls[randomBallIndex].transform.position;
            bomb.Activate();
        }
    }

}
