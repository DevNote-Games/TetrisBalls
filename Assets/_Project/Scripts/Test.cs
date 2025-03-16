using UnityEngine;
using VG2;


public class Test : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;



    private void Start()
    {
        //CombineAllMeshes();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneContainer.InstantiatePrefab(_bombPrefab).transform.position = Vector3.zero;
        }


        
    }



}
