using R3;
using UnityEngine;
using VG2;

public class BombBoosterAvailable : ReactiveView
{
    [SerializeField] private GameObject _bombButton;

    protected override void Subscribe()
    {
        GameState.Level.Subscribe(_ => Display());
    }

    protected override void Display()
    {
        _bombButton.SetActive(GameState.Level.Value >= Configs.Boosters.BombAvailableFromLevel);
    }

    
}
