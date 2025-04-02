using TMPro;
using UnityEngine;
using VG2;
using Zenject;
using R3;
using UnityEngine.UI;
using DG.Tweening;


public class LevelProgressView : ReactiveView
{
    [SerializeField] private TextMeshProUGUI _levelNubmerText;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Slider _slider;

    [Inject] private LevelController _levelController;

    private Tween _textTween;

    private const float SCALED_TEXT_SIZE = 1.3f;
    private const float TEXT_ANIMATION_DURATION = 0.3f;

    



    protected override void Subscribe()
    {
        disposables.Add(_levelController.OnLevelStarted.Subscribe(_ => Display()));
        disposables.Add(_levelController.OnScoreChanged.Subscribe(_ => Display()));
    }


    protected override void Display()
    {
        _levelNubmerText.text = GameState.Level.ToString();
        _valueText.text = $"{_levelController.CurrentScore}/{_levelController.RequiredScore}";
        _slider.value = (float)_levelController.CurrentScore / _levelController.RequiredScore;

        _textTween?.Kill();
        _textTween = DOTween.Sequence(_valueText.transform)
                .Append(_valueText.transform.DOScale(SCALED_TEXT_SIZE, TEXT_ANIMATION_DURATION / 2f))
                .Append(_valueText.transform.DOScale(1f, TEXT_ANIMATION_DURATION / 2f))
                .SetEase(Ease.Flash);
    }

    
}
