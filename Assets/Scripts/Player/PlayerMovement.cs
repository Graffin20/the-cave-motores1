using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("References")]
    public Transform cameraTransform;

    private CharacterController _cc;
    private Vector3 _velocity;
    private Vector2 _moveInput;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    void OnMove(InputValue value) => _moveInput = value.Get<Vector2>();

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        Vector3 move = right * _moveInput.x + forward * _moveInput.y;

        if (move.magnitude > 1f)
            move.Normalize();

        _cc.Move(move * moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);

        if (_cc.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);
    }
}