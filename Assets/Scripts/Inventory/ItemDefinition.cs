using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    [Header("Identity")]
    public string itemName = "New Item";
    [TextArea] public string description = "";
    public Sprite icon;

    [Header("Behaviour")]
    public bool isUsable = true;
    public bool isStackable = false;
    public int maxStackSize = 1;

    [Header("World")]
    // Prefab that gets spawned when this item is dropped into the world.
    // Assign the ItemPickup prefab with this ItemDefinition already set on it.
    public GameObject worldPrefab;

    public virtual void Use()
    {

    }
}
