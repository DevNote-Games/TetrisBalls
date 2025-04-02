using UnityEngine;
using Zenject;

public class BoosterTutorial : MonoBehaviour
{
    [SerializeField] private CursorMoveTween _moveCursorPrefab;
    [SerializeField] private CicleScaleTween _horizontalCursorPrefab;
    [SerializeField] private Transform _moveCursorToPoint;

    private CicleScaleTween _horizontalCursorInstance;
    private CursorMoveTween _moveCursorInstance;
    private ItemGroup _boosterItem;


    [Inject] private BallsController _ballsController;
    [Inject] private LevelController _levelController;

    private void Start()
    {
        _horizontalCursorInstance = Instantiate(_horizontalCursorPrefab, UIContainer.MainCanvas);
        _horizontalCursorInstance.transform.position = UIContainer.BombBoosterButton.transform.position;
        _ballsController.onItemGroupReplaced += OnBallsReplacedByBooster;
    }

    private void OnBallsReplacedByBooster(ItemGroup boosterItemGroup)
    {
        Destroy(_horizontalCursorInstance.gameObject);
        _ballsController.onItemGroupReplaced -= OnBallsReplacedByBooster;

        _boosterItem = boosterItemGroup;
        _boosterItem.onTaked += OnBoosterTaked;
        _boosterItem.onCanceled += OnBoosterCanceled;

        RunCursorAnimation();
    }


    private void RunCursorAnimation()
    {
        Vector2 from = CameraContainer.OrthographicCamera.WorldToScreenPoint(_boosterItem.transform.position);
        Vector2 to = CameraContainer.OrthographicCamera.WorldToScreenPoint(_moveCursorToPoint.position);

        if (_moveCursorInstance != null) Destroy(_moveCursorInstance.gameObject);
        _moveCursorInstance = Instantiate(_moveCursorPrefab, UIContainer.MainCanvas);
        _moveCursorInstance.Run(from, to);
    }

    private void OnBoosterCanceled()
    {
        RunCursorAnimation();
    }

    private void OnBoosterTaked()
    {
        if (_moveCursorInstance != null) Destroy(_moveCursorInstance.gameObject);
    }



}
