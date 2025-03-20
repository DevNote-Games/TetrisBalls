using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BallGroup : SpawnableItem
{
    [SerializeField] private InteractionHandler _interactionHandler;
    [SerializeField] private float _pushForce;
    [SerializeField] private List<Ball> _balls; public List<Ball> Balls => _balls;


    private const float TAKED_POSITION_Z = -0.5f; 


    [Inject] private BallsController _ballsController;

    private void OnEnable()
    {
        foreach (var ball in _balls)
        {
            ball.State = BallState.Inventory;
            ball.TogglePhysics(false);
        }

        _interactionHandler.onBeginDrag += OnBeginDrag;
        _interactionHandler.onDrag += OnDrag;
        _interactionHandler.onEndDrag += OnEndDrag;
    }

    private void OnDisable()
    {
        _interactionHandler.onBeginDrag -= OnBeginDrag;
        _interactionHandler.onDrag -= OnDrag;
        _interactionHandler.onEndDrag -= OnEndDrag;
    }

    private void OnBeginDrag(Vector2 position)
    {
        foreach (var ball in _balls)
            ball.SetTransparentMode(true);
            
    }

    private void OnDrag(Vector2 position)
    {
        Vector2 worldPosition = CameraContainer.OrthographicCamera.ScreenToWorldPoint(position);
        transform.position = new Vector3(worldPosition.x, worldPosition.y, TAKED_POSITION_Z);
    }

    private void OnEndDrag(Vector2 position)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        foreach (var ball in _balls)
        {
            ball.TogglePhysics(true);
            ball.State = BallState.Active;
            ball.transform.SetParent(null);
            ball.SetTransparentMode(false);

            Vector2 pushDirection = ball.transform.position - transform.position;
            ball.Push(pushDirection, _pushForce);
        }

        _ballsController.OnItemReleased(this);
        Destroy(gameObject);
    }

}
