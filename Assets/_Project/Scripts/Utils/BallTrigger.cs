using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    public delegate void OnBallHandle(Ball ball);
    public event OnBallHandle onBallEnter;
    public event OnBallHandle onBallExit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null && 
            collision.attachedRigidbody.gameObject.TryGetComponent<Ball>(out var ball))
            onBallEnter?.Invoke(ball);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null && 
            collision.attachedRigidbody.gameObject.TryGetComponent<Ball>(out var ball))
            onBallExit?.Invoke(ball);
    }


}
