using UnityEngine;

// Attach to the Player (or any persistent GameObject).
// dropPoint: assign an empty GameObject positioned in front of the camera.
public class ItemDropper : MonoBehaviour
{
    [Header("References")]
    public Transform dropPoint; // Empty GameObject child of the Camera, offset forward

    public void Drop(ItemDefinition item)
    {
        if (item == null) return;
        if (item.worldPrefab == null)
        {
            Debug.LogWarning($"[Dropper] {item.itemName} has no worldPrefab assigned.");
            return;
        }

        // Spawn at the drop point with a small random scatter so stacked drops don't clip.
        Vector3 scatter = new Vector3(
            Random.Range(-0.1f, 0.1f),
            0f,
            Random.Range(-0.1f, 0.1f));

        Instantiate(item.worldPrefab,
                    dropPoint.position + scatter,
                    dropPoint.rotation);

        Debug.Log($"[Dropper] Dropped {item.itemName} at {dropPoint.position}.");
    }
}