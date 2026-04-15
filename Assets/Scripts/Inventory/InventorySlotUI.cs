using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Attach to the slot prefab.
// The prefab needs: an Image (icon), a TMP_Text (quantity), and a Button.
public class InventorySlotUI : MonoBehaviour
{
    [Header("References")]
    public Image iconImage;
    public TMP_Text quantityText;
    public Button slotButton;

    // InventoryUI subscribes to this.
    public event Action OnSlotClicked;

    InventorySlot _slot;

    void Awake()
    {
        slotButton.onClick.AddListener(() => OnSlotClicked?.Invoke());
    }

    public void SetSlot(InventorySlot slot)
    {
        _slot = slot;

        bool hasItem = slot != null && !slot.IsEmpty;
        iconImage.enabled = hasItem;
        quantityText.enabled = hasItem && slot.item.isStackable;

        if (hasItem)
        {
            iconImage.sprite = slot.item.icon;
            if (slot.item.isStackable)
                quantityText.text = slot.quantity.ToString();
        }
    }
}
