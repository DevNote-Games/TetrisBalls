using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemGroup : MonoBehaviour
{
    public event Action<List<Item>> onItemsReleased;

    [SerializeField] private InteractionHandler _interactionHandler;
    [SerializeField] private List<Item> _items; public List<Item> Items => _items;

    private Vector3 _originPosition;
    private Collider2D[] _colliderChecker = new Collider2D[1];

    private const float TAKED_POSITION_Z = -0.5f; 


    [Inject] private BallsController _ballsController;
    [Inject] private LevelController _levelController;


    public void SetOriginPosition(Vector3 position) => _originPosition = position;

    private void OnEnable()
    {
        _interactionHandler.onDrag += OnDrag;
        _interactionHandler.onEndDrag += OnEndDrag;
    }

    private void OnDisable()
    {
        _interactionHandler.onDrag -= OnDrag;
        _interactionHandler.onEndDrag -= OnEndDrag;
    }

    private void OnDrag(Vector2 position)
    {
        Vector2 worldPosition = CameraContainer.OrthographicCamera.ScreenToWorldPoint(position);
        transform.position = new Vector3(worldPosition.x, worldPosition.y, TAKED_POSITION_Z);

        bool placingAvailable = GroupIsAvailableForPlacing();
        _items.ForEach(item => item.SetAvailableForPlacing(placingAvailable));
    }

    private bool GroupIsAvailableForPlacing()
    {
        foreach (var item in _items)
        {
            int count = Physics2D.OverlapCircleNonAlloc(item.transform.position, item.Radius, _colliderChecker, LayerMask.GetMask("Default", "Balls"));
            bool itemInsideField = _levelController.CurrentLevel.MainFieldRect.Contains(item.transform.position);
            if (count > 0 || !itemInsideField) return false;
        }

        return true;
    }


    private void OnEndDrag(Vector2 position)
    {
        if (GroupIsAvailableForPlacing())
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

            foreach (var item in _items)
            {
                item.transform.SetParent(null);
                item.Activate();
                _ballsController.OnItemGroupReleased(this);
                //Vector2 pushDirection = item.transform.position - transform.position;
                //item.Push(pushDirection, _pushForce);
            }

            onItemsReleased?.Invoke(_items);
            Destroy(gameObject);
        }
        else
        {
            transform.position = _originPosition;
            _items.ForEach(item => item.SetAvailableForPlacing(true));
        }      
    }

}
