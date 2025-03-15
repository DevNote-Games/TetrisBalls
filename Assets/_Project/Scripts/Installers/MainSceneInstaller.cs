using System.Collections.Generic;
using UnityEngine;
using VG2;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [Header("UI")]
    [SerializeField] private UIContainer _uiContainer;
    [SerializeField] private VictoryScreenView _victoryScreen;
    [SerializeField] private LoseScreenView _loseScreen;



    [Header("Gameplay")]
    [SerializeField] private CameraContainer _cameraContainer;
    [SerializeField] private List<Transform> _ballGroupSpawnPositions;

    public override void InstallBindings()
    {
        new SceneContainer(Container);
        _cameraContainer.Initialize();
        _uiContainer.Initialize();
        var gameTime = new GameTime();

        var ui = new UIController(_victoryScreen, _loseScreen);
        var ballSpawner = new BallSpawnerController();
        var level = new LevelController(ballSpawner, ui);
        var effects = new EffectsController(ballSpawner, level);
        var ballChains = new BallChainsController(level);


        Container.BindInterfacesAndSelfTo<LevelController>().FromInstance(level).AsSingle();
        Container.BindInterfacesAndSelfTo<BallSpawnerController>().FromInstance(ballSpawner).AsSingle();
        Container.BindInterfacesAndSelfTo<EffectsController>().FromInstance(effects).AsSingle();
        Container.BindInterfacesAndSelfTo<UIController>().FromInstance(ui).AsSingle();
    }

}
