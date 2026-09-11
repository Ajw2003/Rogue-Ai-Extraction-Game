using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float distance;
    [SerializeField] private float minAngle = -45;
    [SerializeField] private float maxAngle = 90;
    [SerializeField] private float sensitivity = 3f;

    [SerializeField] private float smoothing = 0.12f;

    private float _rotationX = 0f;
    private float _rotationY = 0f;

    public Vector3 currentRotation;
    private Vector3 _rotationVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        _rotationX = angles.y;
        _rotationY = angles.x;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        _rotationX += Input.GetAxis("Mouse Y") * sensitivity;
        _rotationY += Input.GetAxis("Mouse X") * sensitivity;

        _rotationX = Mathf.Clamp(_rotationX, minAngle, maxAngle);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(_rotationX, _rotationY, 0), ref _rotationVelocity, smoothing);
        transform.eulerAngles = currentRotation;
        transform.position = target.position - (transform.forward * distance);
    }
}
