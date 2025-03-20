using UnityEngine;
using Zenject;

public class BoosterGroup : SpawnableItem
{
    [SerializeField] private BombBooster _booster;
    [SerializeField] private InteractionHandler _interactionHandler;

    [Inject] private BallsController _ballController;


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
        transform.position = worldPosition;
    }


    private void OnEndDrag(Vector2 position)
    {
        _booster.Activate();
        _booster.transform.SetParent(null);
        _ballController.OnItemReleased(this);

        Destroy(gameObject);
    }

}
