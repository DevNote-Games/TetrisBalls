using UnityEngine;
using Zenject;


public class Test : MonoBehaviour
{
    [Inject] private BallsController _ballsController;


    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            _ballsController.SpawnBalls();

        }

    }


}
