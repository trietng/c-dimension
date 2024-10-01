using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2.0f;  // Speed of zooming
    public float moveSpeed = 5.0f;  // Speed of moving the camera
    public float rotationSpeed = 2.0f; // Speed of camera rotation

    private Vector3 initialPosition; // Store the initial position of the camera
    private float initialFieldOfView; // Store the initial field of view
    private Quaternion initialRotation; // Store the initial rotation of the camera
    private Camera mainCamera; // Reference to the main camera

    private bool isRotating; // To track if the right mouse button is held down
    private Vector3 lastMousePosition; // To track the last mouse position

    void Start()
    {
        // Try to get the main camera
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found! Please ensure there is a camera tagged as 'MainCamera'.");
            return; // Exit the method if the camera is not found
        }

        // Store the initial position, field of view, and rotation
        initialPosition = transform.position;
        initialFieldOfView = mainCamera.fieldOfView;
        initialRotation = transform.rotation; // Store the initial rotation
    }

    void Update()
    {
        if (mainCamera == null) return; // Exit if the main camera is not found

        // Zooming the camera in and out
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            mainCamera.fieldOfView -= scrollInput * zoomSpeed * 100 * Time.deltaTime;
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 15, 90); // Limit the zoom level
        }

        // Moving the camera with arrow keys only
        if (Input.GetKey(KeyCode.LeftArrow))  // Move left
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow)) // Move right
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))    // Move up
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))  // Move down
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }

        // Camera rotation with right mouse button drag
        if (Input.GetMouseButtonDown(0)) // Right mouse button down
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition; // Store the initial mouse position
        }
        if (Input.GetMouseButtonUp(0)) // Right mouse button up
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition; // Calculate the mouse movement
            float rotationX = deltaMouse.x * rotationSpeed * Time.deltaTime; // Horizontal rotation
            float rotationY = deltaMouse.y * rotationSpeed * Time.deltaTime; // Vertical rotation

            transform.Rotate(Vector3.up, -rotationX, Space.World); // Rotate around the Y-axis
            transform.Rotate(Vector3.right, rotationY, Space.World); // Rotate around the X-axis

            lastMousePosition = Input.mousePosition; // Update the last mouse position
        }

        // Reset camera position, zoom, and rotation
        if (Input.GetKeyDown(KeyCode.V)) // Press R to reset the camera
        {
            ResetCamera();
        }
    }

    // Method to reset camera position, zoom, and rotation
    void ResetCamera()
    {
        transform.position = initialPosition; // Reset position
        transform.rotation = initialRotation; // Reset rotation
        if (mainCamera != null) // Check if the main camera is still valid
        {
            mainCamera.fieldOfView = initialFieldOfView; // Reset field of view
        }
    }
}
