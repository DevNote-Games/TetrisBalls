using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StartTutorial : MonoBehaviour
{
    [SerializeField] private Transform _moveCursorToPoint;
    [SerializeField] private CursorMoveTween _cursorPrefab;

    private CursorMoveTween _cursorInstance;
    private bool _useCursor = true;

    [Inject] private LevelController _levelController;


    private void Start()
    {
        var itemGroups = FindObjectsByType<ItemGroup>(FindObjectsSortMode.None);
        foreach (var group in itemGroups)
        {
            group.onTaked += OnItemGroupTaked;
            group.onCanceled += OnItemGroupCanceled;
            group.onItemsReleased += OnItemGroupReleased;
        }
           
        RunCursorAnimation();
    }

    private void OnItemGroupReleased(List<Item> items)
    {
        _useCursor = false;
    }

    private void OnItemGroupCanceled()
    {
        if (_useCursor) RunCursorAnimation();
    }

    private void OnItemGroupTaked()
    {
        if (_cursorInstance != null)
            Destroy(_cursorInstance.gameObject);
    }


    private void RunCursorAnimation()
    {
        if (_cursorInstance != null)
            Destroy(_cursorInstance.gameObject);

        Vector2 ballSpawnPosition = _levelController.CurrentLevel.GetBallGroupSpawnPositions()[0];
        Vector2 from = CameraContainer.OrthographicCamera.WorldToScreenPoint(ballSpawnPosition);
        Vector2 to = CameraContainer.OrthographicCamera.WorldToScreenPoint(_moveCursorToPoint.transform.position);

        _cursorInstance = Instantiate(_cursorPrefab, UIContainer.MainCanvas);
        _cursorInstance.Run(from, to);
    }




}
