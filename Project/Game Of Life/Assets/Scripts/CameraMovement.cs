using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera m_camera;
    public float m_zoomFactor = 0.3f;
    public float m_panSpeed = 0.5f;

    private Transform m_target;
    private Vector2 m_grabScreenVec;
    private Vector2 m_grabMouseVec;
    private Vector3 m_grabCameraVec;


    void Start() {
        m_target = m_camera.transform;
    }
        
    /*
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */
    void LateUpdate()
    {
        if(Input.GetMouseButtonDown(2)) // set grab vector 
        {
            m_grabMouseVec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector3 cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_grabScreenVec = new Vector2(cameraPos.x, cameraPos.y);
            m_grabCameraVec = m_target.position;
        }
        else
        if (Input.GetMouseButton(2)) // move Camera
        {
            Vector3 cameraPosV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 cameraPos = new Vector2(cameraPosV3.x, cameraPosV3.y);
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 mouseMove = (m_grabMouseVec - mousePos) * m_panSpeed;
            m_target.position = m_grabCameraVec + new Vector3(mouseMove.x, mouseMove.y, 0.0f);
        }

        if(Input.mouseScrollDelta.y == -1)
        {
            m_camera.orthographicSize *= (1 + m_zoomFactor);
        } else if (Input.mouseScrollDelta.y == 1)
        {
            m_camera.orthographicSize /= (1 + m_zoomFactor);

        }
    }

}
