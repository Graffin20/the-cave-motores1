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
    public bool isConsumable = true;
    public int maxStackSize = 1;

    [Header("World")]
    public GameObject worldPrefab;

    public virtual bool Use(GameObject user)
    {
        return true;
    }
}