using R3;
using TMPro;
using UnityEngine;
using VG2;

public class CoinsView : ReactiveView
{
    [SerializeField] private TextMeshProUGUI _coinsText;


    protected override void Subscribe()
    {
        // Подписываемся на событие изменения монет
        disposables.Add(GameState.Coins.Subscribe(coins => Display()));
    }

    // Функция вызывается в OnEnable, а также при изменении значения GameState.Coins
    protected override void Display()
    {
        _coinsText.text = GameState.Coins.Value.ToString();
    }

    
}
