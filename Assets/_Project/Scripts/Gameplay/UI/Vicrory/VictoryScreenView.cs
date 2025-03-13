using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class VictoryScreenView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private Slider _levelProgressSlider;
    [SerializeField] private TextMeshProUGUI _levelProgressText;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private RectTransform _rewards;
    [SerializeField] private RectTransform _buttons;


    private float FILL_LEVEL_PROGRESS_DELAY = 0.5f;
    private float FILL_LEVEL_PROGRESS_DURATION = 2f;
    private float SHOW_OTHER_DURATION = 1f;


    [Inject] private LevelController _levelController;


    private void OnEnable() => Display(_levelController.CompletedLevel);



    private void Display(int completedLevel)
    {
        int requiredScore = Configs.Levels.GetLevelRequiredScore(completedLevel);

        _rewardText.text = Configs.GameRules.LevelCoinsReward.ToString();

        _rewards.localScale = Vector3.zero;
        _buttons.localScale = Vector3.zero;
        _levelProgressSlider.value = 0f;

        var progressSequence = DOTween.Sequence()
            .AppendInterval(FILL_LEVEL_PROGRESS_DELAY)
            .Append(_levelProgressSlider.DOValue(1f, FILL_LEVEL_PROGRESS_DURATION).SetEase(Ease.Linear));

        progressSequence.onUpdate += () =>
        {
            int score = (int)(_levelProgressSlider.value * requiredScore);
            _levelProgressText.text = $"{score}/{requiredScore}";
        };

        progressSequence.onComplete += () =>
        {
            int nextLevel = completedLevel + 1;

            _levelText.text = nextLevel.ToString();
            _levelProgressSlider.value = 0f;

            int nextLevelRequire = Configs.Levels.GetLevelRequiredScore(nextLevel);
            _levelProgressText.text = $"{0}/{nextLevelRequire}";

            DOTween.Sequence()
                .Append(_rewards.DOScale(1f, SHOW_OTHER_DURATION).SetEase(Ease.OutBack))
                .Append(_buttons.DOScale(1f, SHOW_OTHER_DURATION).SetEase(Ease.OutBack));

        };
            




    }









}
