using UnityEngine;
using UnityEngine.InputSystem;

// Attach to the player's Camera (or a child of it).
// Wire the 'Interact' InputAction from your Input Actions asset in the Inspector.
public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 3f;
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
        Ray ray = new Ray(transform.position, transform.forward);
        _currentTarget = Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask)
            ? hit.collider.GetComponent<ItemPickup>()
            : null;
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        _currentTarget?.Interact();
    }

    void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        bool hitInteractable = Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask)
            && hit.collider.GetComponent<ItemPickup>() != null;

        Gizmos.color = hitInteractable ? Color.green : Color.red;

        Gizmos.DrawRay(transform.position, transform.forward * interactRange);

        if (hitInteractable)
        {
            Gizmos.DrawSphere(hit.point, 0.05f);
        }
    }

    // Optional: expose for a HUD crosshair prompt.
    public bool IsLookingAtItem => _currentTarget != null;
    public string TargetItemName => _currentTarget?.itemDefinition?.itemName ?? "";
}
