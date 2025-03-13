using UnityEngine;
using Zenject;

public class CameraContainer : MonoBehaviour, IInitializable
{
    private static CameraContainer _instance;


    [SerializeField] private Camera _perspectiveCamera; public static Camera PerspectiveCamera => _instance._perspectiveCamera;
    [SerializeField] private Camera _orthographicCamera; public static Camera OrthographicCamera => _instance._orthographicCamera;


    public void Initialize() => _instance = this;



    public static void AdjustCamerasToRect(Rect rect)
    {
        AdjustOrthographicCameraToRectangle(rect);
        SyncPerspectiveCamera();
    }

    private static void AdjustOrthographicCameraToRectangle(Rect rect)
    {
        float rectWidth = rect.size.x;
        float rectHeight = rect.size.y;
        float cameraAspect = OrthographicCamera.aspect;

        if (rectWidth / rectHeight > cameraAspect)
            OrthographicCamera.orthographicSize = rectWidth / (2f * cameraAspect);

        else OrthographicCamera.orthographicSize = rectHeight / 2f;

        OrthographicCamera.transform.position = new Vector3(rect.position.x, rect.position.y, -10f);
    }

    private static void SyncPerspectiveCamera()
    {
        float orthoHeight = OrthographicCamera.orthographicSize * 2f;

        float fov = PerspectiveCamera.fieldOfView * Mathf.Deg2Rad;
        float distance = (orthoHeight / 2f) / Mathf.Tan(fov / 2f);

        PerspectiveCamera.transform.position = 
            new Vector3(OrthographicCamera.transform.position.x, OrthographicCamera.transform.position.y, -distance);
    }




}
