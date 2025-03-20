using UnityEngine;
using VG2;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private UIContainer _uiContainer;
    [SerializeField] private CameraContainer _cameraContainer;

    public override void InstallBindings()
    {
        new SceneContainer(Container);
        _cameraContainer.Initialize();
        _uiContainer.Initialize();
        var gameTime = new GameTime();

        var ui = new UIController();
        var balls = new BallsController();
        var level = new LevelController(balls, ui);
        var effects = new EffectsController(balls, level);
        var chainBoosters = new ChainBoostersController(balls);


        Container.BindInterfacesAndSelfTo<LevelController>().FromInstance(level).AsSingle();
        Container.BindInterfacesAndSelfTo<BallsController>().FromInstance(balls).AsSingle();
        Container.BindInterfacesAndSelfTo<EffectsController>().FromInstance(effects).AsSingle();
        Container.BindInterfacesAndSelfTo<UIController>().FromInstance(ui).AsSingle();
        Container.BindInterfacesAndSelfTo<ChainBoostersController>().FromInstance(chainBoosters).AsSingle();


    }

}
