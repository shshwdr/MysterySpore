using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    public Camera mainCamera;

    private Vector2 minBounds;
    private Vector2 maxBounds;

    void Awake()
    {
        mainCamera = mainCamera ?? Camera.main;
        Vector3 cameraBottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 cameraTopRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        minBounds = new Vector2(cameraBottomLeft.x, cameraBottomLeft.y);
        maxBounds = new Vector2(cameraTopRight.x, cameraTopRight.y);
        
        
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0; // Keep z at 0 for 2D

        worldMousePos.x = Mathf.Clamp(worldMousePos.x, minBounds.x, maxBounds.x);
        worldMousePos.y = Mathf.Clamp(worldMousePos.y, minBounds.y, maxBounds.y);

        transform.position = worldMousePos;
    }
}