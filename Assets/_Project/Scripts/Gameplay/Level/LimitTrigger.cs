using UnityEngine;
using Zenject;

public class LimitTrigger : MonoBehaviour
{

    [Inject] private LevelController _levelController;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ball = collision.attachedRigidbody.GetComponent<Ball>();
        _levelController.AddBallToLimit(ball);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var ball = collision.attachedRigidbody.GetComponent<Ball>();
        _levelController.RemoveBallFromLimit(ball);
    }


}
