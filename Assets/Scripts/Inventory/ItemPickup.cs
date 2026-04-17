using UnityEngine;

// Place this on every item prefab that exists in the world.
// Requires a Collider so raycasts can hit it.
[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [Header("Item data")]
    public ItemDefinition itemDefinition;
    public int quantity = 1;

    // Called by PlayerInteract when the player looks at this and presses Interact.
    public void Interact()
    {
        if (itemDefinition == null) return;

        bool added = InventoryManager.Instance.AddItem(itemDefinition, quantity);
        if (added)
        {
            Debug.Log($"[Pickup] Picked up {itemDefinition.itemName}.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"[Pickup] Inventory full — couldn't pick up {itemDefinition.itemName}.");
        }
    }
}
