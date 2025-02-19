using System.Collections.Generic;
using UnityEngine;
using VG2;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private List<Transform> _ballGroupSpawnPositions;



    public override void InstallBindings()
    {
        new SceneContainer(Container);
        var gameTime = new GameTime();

        Container.Bind<BallSpawnerController>().FromInstance(new BallSpawnerController(_ballGroupSpawnPositions)).AsSingle();


    }


}
