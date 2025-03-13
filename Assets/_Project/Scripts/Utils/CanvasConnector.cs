using UnityEngine;

public class CanvasConnector : MonoBehaviour
{

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = CameraContainer.OrthographicCamera;
    }



}
