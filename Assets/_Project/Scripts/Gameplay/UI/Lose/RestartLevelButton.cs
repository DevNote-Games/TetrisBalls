using VG2;
using Zenject;

public class RestartLevelButton : ButtonHandler
{
    [Inject] private LevelController _levelController;
    [Inject] private UIController _uiController;


    protected override void OnClick()
    {
        _uiController.HideCurrentView();
        _levelController.StartLevel(GameState.Level.Value);
    }

}
