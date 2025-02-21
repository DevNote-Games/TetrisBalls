using System.Collections.Generic;
using UnityEngine;

public class BallGroup : MonoBehaviour
{
    [SerializeField] private InteractionHandler _interactionHandler;
    [SerializeField] private float _pushForce;
    [SerializeField] private List<Ball> _balls; public List<Ball> Balls => _balls;



    private void OnEnable()
    {
        foreach (var ball in _balls)
            ball.TogglePhysics(false);


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
        //throw new NotImplementedException();
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

        Destroy(gameObject);
            
    }



}
