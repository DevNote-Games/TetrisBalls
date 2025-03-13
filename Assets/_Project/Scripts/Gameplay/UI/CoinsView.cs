using R3;
using TMPro;
using UnityEngine;
using VG2;

public class CoinsView : ReactiveView
{
    [SerializeField] private TextMeshProUGUI _coinsText;


    protected override void Subscribe()
    {
        // ������������� �� ������� ��������� �����
        disposables.Add(GameState.Coins.Subscribe(coins => Display()));
    }

    // ������� ���������� � OnEnable, � ����� ��� ��������� �������� GameState.Coins
    protected override void Display()
    {
        _coinsText.text = GameState.Coins.Value.ToString();
    }

    
}
