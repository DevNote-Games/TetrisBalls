using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BallGroup : MonoBehaviour
{
    [SerializeField] private InteractionHandler _interactionHandler;
    [SerializeField] private float _pushForce;
    [SerializeField] private List<Ball> _balls; public List<Ball> Balls => _balls;

    [Inject] private BallsController _ballsController;

    private void OnEnable()
    {
        foreach (var ball in _balls)
            ball.TogglePhysics(false);

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
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
        transform.position = worldPosition;
    }

    private void OnEndDrag(Vector2 position)
    {
        foreach (var ball in _balls)
        {
            ball.TogglePhysics(true);
            ball.transform.SetParent(null);

            Vector2 pushDirection = ball.transform.position - transform.position;
            ball.Push(pushDirection, _pushForce);
        }

        _ballsController.OnBallGroupReleased();
        Destroy(gameObject);
    }



}
