using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("References")]
    public Transform cameraTransform;
    public Animator[] viewmodelAnimators;
    public AudioSource audioSource;

    [Header("Footstep Sounds")]
    public AudioClip[] grassFootsteps = new AudioClip[20];
    public AudioClip[] woodFootsteps = new AudioClip[0];

    private CharacterController _cc;
    private Vector3 _velocity;
    private Vector2 _moveInput;
    [SerializeField] private float _footstepCooldown = 0f;
    [SerializeField] private float _footstepInterval = 0.4f;

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

        bool isMoving = move.magnitude > 0f;
        foreach (Animator animator in viewmodelAnimators)
        {
            if (animator != null && animator.gameObject.activeInHierarchy && animator.runtimeAnimatorController != null)
                animator.SetBool("Walk", isMoving);
        }

        _cc.Move(move * moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);

        if (_cc.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);

        // Handle footstep sounds
        if (isMoving && _cc.isGrounded)
        {
            _footstepCooldown -= Time.deltaTime;
            if (_footstepCooldown <= 0f)
            {
                PlayFootstepSound();
                _footstepCooldown = _footstepInterval;
            }
        }
    }

    void PlayFootstepSound()
    {
        if (audioSource == null) return;

        // Raycast downward to detect surface
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 2f))
        {
            AudioClip[] soundArray = null;

            if (hit.collider.CompareTag("Grass") && grassFootsteps.Length > 0)
            {
                soundArray = grassFootsteps;
                audioSource.volume = 1f;
            }
            else if (hit.collider.CompareTag("Wood") && woodFootsteps.Length > 0)
            {
                soundArray = woodFootsteps;
                audioSource.volume = 0.5f;
            }

            if (soundArray != null && soundArray.Length > 0)
            {
                AudioClip randomSound = soundArray[Random.Range(0, soundArray.Length)];
                if (randomSound != null)
                    audioSource.PlayOneShot(randomSound);
            }
        }
    }
}