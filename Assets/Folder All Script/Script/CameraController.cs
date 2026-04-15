using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _sensitivity;

    private Vector2 _mouseInput;
    private float _pitch;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        // Horizontal rotation (Y axis)
        transform.Rotate(
            Vector3.up,
            _mouseInput.x * _sensitivity * Time.deltaTime
        );

        // Vertical rotation (X axis)
        _pitch -= _mouseInput.y * _sensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);

        transform.localEulerAngles =
            new Vector3(_pitch, transform.localEulerAngles.y, 0f);
    }

    public void OnMouseMove(InputValue value)
    {
        _mouseInput = value.Get<Vector2>();
    }

}
