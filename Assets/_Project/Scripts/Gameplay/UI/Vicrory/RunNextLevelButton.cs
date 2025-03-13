using VG2;
using Zenject;

public class RunNextLevelButton : ButtonHandler
{
    [Inject] private LevelController _levelController;


    protected override void OnClick()
    {
        _levelController.StartLevel(GameState.Level + 1);
    }
}
