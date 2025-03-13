using UnityEngine;
using Zenject;

public class UIContainer : MonoBehaviour, IInitializable
{
    private static UIContainer _instance;


    [SerializeField] private RectTransform _vfxCanvas; public static RectTransform VfxCanvas => _instance._vfxCanvas;
    [SerializeField] private RectTransform _mainCanvas; public static RectTransform MainCanvas => _instance._mainCanvas;
    [SerializeField] private RectTransform _levelScore; public static RectTransform LevelScore => _instance._levelScore;




    public static RectTransform Canvas { get; private set; } 


    public void Initialize() => _instance = this;


}
