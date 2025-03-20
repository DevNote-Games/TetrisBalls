using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using VG2;

public class CoinsView : ReactiveView
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    private Tween _currentTween;

    private const float ANIMATION_TEXT_SCALING = 1.3f;
    private const float ANIMATION_DURATION = 0.4f;


    protected override void Subscribe()
    {
        disposables.Add(GameState.Coins.Subscribe(coins => Display()));
    }

    protected override void Display()
    {
        SetValue(GameState.Coins.Value);
    }


    public void SetValue(int value)
    {
        _coinsText.text = value.ToString();

        _currentTween?.Kill();
        _currentTween = DOTween.Sequence(_coinsText.transform)
            .Append(_coinsText.transform.DOScale(ANIMATION_TEXT_SCALING, ANIMATION_DURATION / 2f))
            .Append(_coinsText.transform.DOScale(1f, ANIMATION_DURATION / 2f))
            .SetEase(Ease.Flash);
    }

}
