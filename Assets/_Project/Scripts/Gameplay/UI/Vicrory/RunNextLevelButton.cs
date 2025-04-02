using UnityEngine;
using VG2;
using Zenject;

public class RunNextLevelButton : ButtonHandler
{
    private enum ButtonType { Standart, AdBonus }

    [SerializeField] private ButtonType _buttonType;
    [SerializeField] private VictoryScreenView _victoryScreen;

    [Inject] private LevelController _levelController;
    [Inject] private UIController _uiController;


    protected override void OnClick()
    {
        if (_buttonType == ButtonType.Standart)
            RunNextLevel();

        else if (_buttonType == ButtonType.AdBonus)
        {
            Ads.Rewarded.Show(AdKey.BonusLevelReward, resetCooldown: true, onShown: (result) =>
            {
                if (result == Ads.Rewarded.Result.Success)
                    _victoryScreen.ShowBonusReward();
            });
        }
    }

    private void RunNextLevel()
    {
        int reward = _victoryScreen.UseAdBonus ? Configs.GameRules.LevelCoinsRewardWithAdBonus : Configs.GameRules.LevelCoinsReward;
        GameState.Coins.Value += reward;

        _levelController.StartLevel(GameState.Level.Value + 1);
        Analytics.SendEvent(EventKey.LevelCompleted(_levelController.CompletedLevel));

        _uiController.HideCurrentView();
        _uiController.RunCoinsParticles(reward);
    }



}
