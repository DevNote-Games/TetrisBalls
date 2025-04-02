using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VG2;
using Zenject;
using R3;

public class UseBombButtonView : ReactiveView
{
    [SerializeField] private Button _useButton;
    [Space(10)]
    [SerializeField] private GameObject _availableSection;
    [SerializeField] private TextMeshProUGUI _availableAmountText;
    [SerializeField] private GameObject _purchaseSection;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private GameObject _adSection;

    [Inject] private BallsController _ballsController;


    private void Start()
    {
        _useButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _useButton.onClick.RemoveListener(OnButtonClick);
    }

    protected override void Subscribe()
    {
        disposables.Add(GameState.Coins.Subscribe(_ => Display()));
        disposables.Add(GameState.BombBoosters.Subscribe(_ => Display()));
    }

    protected override void Display()
    {
        _availableSection.SetActive(GameState.BombBoosters.Value > 0);
        _purchaseSection.SetActive(GameState.BombBoosters.Value == 0 && GameState.Coins.Value >= Configs.Boosters.BombPrice);
        _adSection.SetActive(GameState.BombBoosters.Value == 0 && GameState.Coins.Value < Configs.Boosters.BombPrice);

        _priceText.text = $"<sprite=0>{Configs.Boosters.BombPrice}";
        _availableAmountText.text = $"{GameState.BombBoosters.Value}";
    }

    

    private void OnButtonClick()
    {
        if (GameState.BombBoosters.Value > 0)
        {
            GameState.BombBoosters.Value--;
            ApplyBooster();
        }

        else if (GameState.Coins.Value >= Configs.Boosters.BombPrice)
        {
            GameState.Coins.Value -= Configs.Boosters.BombPrice;
            ApplyBooster();
        }

        else
        {
            Ads.Rewarded.Show(AdKey.BombBooster, onShown: (result) =>
            {
                if (result == Ads.Rewarded.Result.Success)
                    GameState.BombBoosters.Value++;
            });
        }
    }

    
    private void ApplyBooster()
    {
        Analytics.SendEvent(EventKey.BombUsed);
        _ballsController.ReplaceRandomItem(Configs.Boosters.BombGroupPrefab);
    }



}
