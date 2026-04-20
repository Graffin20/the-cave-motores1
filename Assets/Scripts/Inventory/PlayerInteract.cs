using UnityEngine;
using UnityEngine.InputSystem;

// Attach to the player's Camera (or a child of it).
// Wire the 'Interact' InputAction from your Input Actions asset in the Inspector.
public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 3f;
    public float capsuleRadius = 0.25f; // Increased for better detection at steep angles
    public LayerMask interactMask = ~0; // Everything by default; narrow to an "Interactable" layer in the Inspector.

    [Header("Input")]
    // Drag the Interact action from your PlayerInput component here,
    // or assign via code if you prefer.
    public InputActionReference interactAction;

    ItemPickup _currentTarget;

    void OnEnable()  => interactAction.action.performed += OnInteract;
    void OnDisable() => interactAction.action.performed -= OnInteract;

    void Update()
    {
        // Keep track of what we're looking at so you can show a prompt in your HUD.
        // Use CapsuleCast for more reliable detection at angles
        Vector3 capsuleStart = transform.position;
        Vector3 capsuleEnd = transform.position + transform.forward * 0.3f;

        _currentTarget = Physics.CapsuleCast(capsuleStart, capsuleEnd, capsuleRadius, transform.forward, out RaycastHit hit, interactRange, interactMask, QueryTriggerInteraction.UseGlobal)
            ? hit.collider.GetComponent<ItemPickup>()
            : null;
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        _currentTarget?.Interact();
    }

    void OnDrawGizmos()
    {
        // Use CapsuleCast for visualization consistency
        Vector3 capsuleStart = transform.position;
        Vector3 capsuleEnd = transform.position + transform.forward * 0.3f;

        bool hitInteractable = Physics.CapsuleCast(capsuleStart, capsuleEnd, capsuleRadius, transform.forward, out RaycastHit hit, interactRange, interactMask, QueryTriggerInteraction.UseGlobal)
            && hit.collider.GetComponent<ItemPickup>() != null;

        Gizmos.color = hitInteractable ? Color.green : Color.red;

        // Draw the capsule shape
        Gizmos.DrawLine(capsuleStart, capsuleEnd);
        Gizmos.DrawWireSphere(capsuleStart, capsuleRadius);
        Gizmos.DrawWireSphere(capsuleEnd, capsuleRadius);

        // Draw forward direction
        Gizmos.DrawRay(capsuleEnd, transform.forward * interactRange);

        if (hitInteractable)
        {
            Gizmos.DrawSphere(hit.point, 0.05f);
        }
    }

    // Optional: expose for a HUD crosshair prompt.
    public bool IsLookingAtItem => _currentTarget != null;
    public string TargetItemName => _currentTarget?.itemDefinition?.itemName ?? "";
}
