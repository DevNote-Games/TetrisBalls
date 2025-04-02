using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VG2;
using Zenject;

public class VictoryScreenView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private Slider _levelProgressSlider;
    [SerializeField] private TextMeshProUGUI _levelProgressText;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private RectTransform _rewards;
    [SerializeField] private RectTransform _buttons;
    [SerializeField] private GameObject _adButton;

    public bool UseAdBonus { get; private set; }

    private const float FILL_LEVEL_PROGRESS_DELAY = 0.5f;
    private const float FILL_LEVEL_PROGRESS_DURATION = 2f;
    private const float SHOW_OTHER_DURATION = 1f;

    private const float LEVEL_LABEL_UPSCALE = 1.3f;
    private const float LEVEL_LABEL_UPSCALE_DURATION = 0.5f;


    [Inject] private LevelController _levelController;


    private void OnEnable() => Display(_levelController.CompletedLevel);



    private void Display(int completedLevel)
    {
        UseAdBonus = false;

        int requiredScore = Configs.Levels.GetLevelRequiredScore(completedLevel);

        _levelText.text = completedLevel.ToString();
        _rewardText.text = Configs.GameRules.LevelCoinsReward.ToString();

        _adButton.SetActive(true);
        _rewards.localScale = Vector3.zero;
        _buttons.localScale = Vector3.zero;
        _levelProgressSlider.value = 0f;


        var progressSequence = DOTween.Sequence()
            .AppendInterval(FILL_LEVEL_PROGRESS_DELAY)
            .AppendCallback(() => Sound.Play(SoundKey.FillProgress))
            .Append(_levelProgressSlider.DOValue(1f, FILL_LEVEL_PROGRESS_DURATION).SetEase(Ease.Linear));

        progressSequence.onUpdate += () =>
        {
            int score = (int)(_levelProgressSlider.value * requiredScore);
            _levelProgressText.text = $"{score}/{requiredScore}";
        };

        progressSequence.onComplete += () =>
        {
            int nextLevel = completedLevel + 1;


            DOTween.Sequence()
                .Append(_levelText.transform.DOScale(LEVEL_LABEL_UPSCALE, LEVEL_LABEL_UPSCALE_DURATION / 2f))
                .Append(_levelText.transform.DOScale(1f, LEVEL_LABEL_UPSCALE_DURATION / 2f))
                .SetEase(Ease.OutFlash);

            Sound.Play(SoundKey.LevelUp);

            _levelText.text = nextLevel.ToString();
            _levelProgressSlider.value = 0f;

            int nextLevelRequire = Configs.Levels.GetLevelRequiredScore(nextLevel);
            _levelProgressText.text = $"{0}/{nextLevelRequire}";

            DOTween.Sequence()
                .Append(_rewards.DOScale(1f, SHOW_OTHER_DURATION).SetEase(Ease.OutBack))
                .Append(_buttons.DOScale(1f, SHOW_OTHER_DURATION).SetEase(Ease.OutBack));

        };
            
    }


    public void ShowBonusReward()
    {
        UseAdBonus = true;
        _rewardText.text = Configs.GameRules.LevelCoinsRewardWithAdBonus.ToString();
        _adButton.SetActive(false);
    }






}
