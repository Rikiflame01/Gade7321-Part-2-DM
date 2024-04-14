using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public GridManager gridManager; 
    public float sensitivity = 300f;

    private float currentRotationX = -45f;
    private float currentRotationY = 45f;
    private float distanceFromTarget = 15f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (gridManager != null)
        {
            transform.position = gridManager.GridCenter + new Vector3(0, 0, -distanceFromTarget);
            transform.LookAt(gridManager.GridCenter);
        }
    }

    void Update()
    {
        if (gridManager != null)
            CameraControl(gridManager.GridCenter);
    }

    void CameraControl(Vector3 targetCenter)
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        currentRotationY += mouseX;
        currentRotationX += mouseY;
        currentRotationX = Mathf.Clamp(currentRotationX, -90f, 90f);

        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
        Vector3 positionOffset = rotation * new Vector3(0, 0, -distanceFromTarget);

        transform.position = targetCenter + positionOffset;
        transform.LookAt(targetCenter);
    }
}
