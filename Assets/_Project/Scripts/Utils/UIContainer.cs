using UnityEngine;
using Zenject;

public class UIContainer : MonoBehaviour, IInitializable
{
    private static UIContainer _instance;


    [SerializeField] private RectTransform _vfxCanvas; public static RectTransform VfxCanvas => _instance._vfxCanvas;
    [SerializeField] private RectTransform _mainCanvas; public static RectTransform MainCanvas => _instance._mainCanvas;
    [SerializeField] private RectTransform _levelScore; public static RectTransform LevelScore => _instance._levelScore;
    [SerializeField] private VictoryScreenView _victoryScreen; public static VictoryScreenView VictoryScreen => _instance._victoryScreen;
    [SerializeField] private LoseScreenView _loseScreen; public static LoseScreenView LoseScreen => _instance._loseScreen;
    [SerializeField] private CoinsView _coins; public static CoinsView Coins => _instance._coins;



    public static RectTransform Canvas { get; private set; } 


    public void Initialize() => _instance = this;


}
