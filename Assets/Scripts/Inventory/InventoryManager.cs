using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Grid settings")]
    public int columns = 4;
    public int rows = 2;

    // Each slot holds an item definition (null = empty) and a quantity.
    public InventorySlot[] Slots { get; private set; }

    // Subscribe to this to refresh the UI whenever the inventory changes.
    public event Action OnInventoryChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Slots = new InventorySlot[columns * rows];
        for (int i = 0; i < Slots.Length; i++)
            Slots[i] = new InventorySlot();
    }

    // Returns true if the item was successfully added.
    public bool AddItem(ItemDefinition item, int quantity = 1)
    {
        // Try stacking first.
        if (item.isStackable)
        {
            foreach (var slot in Slots)
            {
                if (slot.item == item && slot.quantity < item.maxStackSize)
                {
                    int space = item.maxStackSize - slot.quantity;
                    int toAdd = Mathf.Min(quantity, space);
                    slot.quantity += toAdd;
                    quantity -= toAdd;
                    if (quantity <= 0) { OnInventoryChanged?.Invoke(); return true; }
                }
            }
        }

        // Place in the first empty slot.
        foreach (var slot in Slots)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.quantity = quantity;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        Debug.Log($"[Inventory] No room for {item.itemName}.");
        return false;
    }

    // Removes one instance (or a quantity) of an item by definition.
    public bool RemoveItem(ItemDefinition item, int quantity = 1)
    {
        for (int i = Slots.Length - 1; i >= 0; i--)
        {
            if (Slots[i].item == item)
            {
                Slots[i].quantity -= quantity;
                if (Slots[i].quantity <= 0) Slots[i].Clear();
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public void UpdateText(TMPro.TextMeshProUGUI tmp, string text)
    {
        tmp.text = text;
    }

    // Remove item at a specific slot index and return its definition.
    public ItemDefinition RemoveAtSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Slots.Length) return null;
        var item = Slots[slotIndex].item;
        Slots[slotIndex].Clear();
        OnInventoryChanged?.Invoke();
        return item;
    }

    public ItemDefinition RemoveFromSlot(int slotIndex, int quantity = 1)
    {
        if (slotIndex < 0 || slotIndex >= Slots.Length)
            return null;

        var slot = Slots[slotIndex];
        if (slot.IsEmpty)
            return null;

        var item = slot.item;

        slot.quantity -= quantity;

        if (slot.quantity <= 0)
            slot.Clear();

        OnInventoryChanged?.Invoke();

        return item;
    }

    // Quick check — useful from other scripts, e.g. quest or crafting systems.
    public bool HasItem(ItemDefinition item)
    {
        foreach (var slot in Slots)
            if (slot.item == item) return true;
        return false;
    }

    public int CountItem(ItemDefinition item)
    {
        int total = 0;
        foreach (var slot in Slots)
            if (slot.item == item) total += slot.quantity;
        return total;
    }

    public void ClearInventory()
    {
        if (Slots == null) return;

        foreach (var slot in Slots)
        {
            slot.Clear();
        }
        OnInventoryChanged?.Invoke();
    }
}



[Serializable]
public class InventorySlot
{
    public ItemDefinition item;
    public int quantity;

    public bool IsEmpty => item == null;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
