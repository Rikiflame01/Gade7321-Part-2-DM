using UnityEngine;

public class NewPlayerCameraController : MonoBehaviour
{
    public GridManager gridManager;
    public float sensitivity = 300f;
    public float distanceFromTarget = 15f;

    public float rotationStep = 90f;
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private bool isCursorLocked = false;

    void Start()
    {
        ToggleCursorState();
        InitializeCameraPosition();
    }

    void Update()
    {
        if (gridManager != null)
            CameraControl(gridManager.GridCenter);

        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            ResetView();
            isCursorLocked = !isCursorLocked;
            ToggleCursorState();
        }
    }

    void CameraControl(Vector3 targetCenter)
    {
        if (isCursorLocked)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            currentRotationY += mouseX;
            currentRotationX += mouseY;
            currentRotationX = Mathf.Clamp(currentRotationX, -90f, 90f);
        }

        UpdateCameraPosition(targetCenter);
    }

    public void ResetView()
    {
        currentRotationX = Mathf.Round(currentRotationX / 90f) * 90f;
        currentRotationY = Mathf.Round(currentRotationY / 90f) * 90f;
        UpdateCameraPosition(gridManager.GridCenter);
    }

    private void UpdateCameraPosition(Vector3 targetCenter)
    {
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
        Vector3 positionOffset = rotation * new Vector3(0, 0, -distanceFromTarget);

        transform.position = targetCenter + positionOffset;
        transform.LookAt(targetCenter);
    }

    public void RotateViewVertical(int direction)
    {
        // Rotate up or down
        currentRotationX += rotationStep * direction;
        currentRotationX = Mathf.Clamp(currentRotationX, -90f, 90f);
        UpdateCameraPosition(gridManager.GridCenter);
    }

    public void RotateViewHorizontal(int direction)
    {
        // Rotate left or right
        currentRotationY += rotationStep * direction;
        UpdateCameraPosition(gridManager.GridCenter);
    }

    private void InitializeCameraPosition()
    {
        if (gridManager != null)
        {
            transform.position = gridManager.GridCenter + new Vector3(0, 0, -distanceFromTarget);
            transform.LookAt(gridManager.GridCenter);
        }
    }

    private void ToggleCursorState()
    {
        if (isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
