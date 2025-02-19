using UnityEngine;
using Zenject;


public class Test : MonoBehaviour
{
    [Inject] private BallSpawnerController _ballSpawnerController;


    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            _ballSpawnerController.SpawnBalls();

        }

    }


}
