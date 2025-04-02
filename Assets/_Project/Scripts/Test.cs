using UnityEngine;
using VG2;
using Zenject;


public class Test : MonoBehaviour
{
    [Inject] private LevelController _levelController;



    private void Start()
    {
        if (!Environment.editor) gameObject.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _levelController.StartLevel(GameState.Level.Value - 1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            _levelController.StartLevel(GameState.Level.Value + 1);
        }

        
    }



}
