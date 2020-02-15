using UnityEngine;

public class UserInput : MonoBehaviour
{
    public float zoomSpeed = 0.1f;
    public Camera mainCamera = null;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = mainCamera.GetComponent<Transform>();
    }

    private void ZoomIn()
    {
        //cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z-1);
       // mainCamera.orthographicSize *= 1.0f - zoomSpeed;
        //if (mainCamera.orthographicSize < 0.2f) mainCamera.orthographicSize = 0.2f;
    }

    private void ZoomOut()
    {
       // cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z + 1);
       // mainCamera.orthographicSize *= 1.0f + zoomSpeed;
    }

    private void checkMouseScroll()
    {
        float mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll == 1) ZoomIn();
        else if (mouseScroll == -1) ZoomOut();
    }

    // Update is called once per frame
    void Update()
    {
        checkMouseScroll();
    }
}
