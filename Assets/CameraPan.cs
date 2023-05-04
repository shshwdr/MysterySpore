using UnityEngine;
using Cinemachine;

public class CameraPan : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float panSpeed = 5f;

    void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("Please assign a Cinemachine Virtual Camera to the script.");
        }
    }

    void Update()
    {
        if (virtualCamera != null)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 newPosition = virtualCamera.transform.position;
            newPosition.x += horizontalInput * panSpeed * Time.deltaTime;
            newPosition.y += verticalInput * panSpeed * Time.deltaTime;

            virtualCamera.transform.position = newPosition;
        }
    }
}