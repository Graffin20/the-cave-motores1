using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemContextMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform panel;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] Button useButton;
    [SerializeField] Button dropButton;
    [SerializeField] ItemDropper itemDropper;
    [SerializeField] Canvas canvas;

    [Header("Player")]
    public GameObject player;

    int activeSlot = -1;

    void Awake()
    {
        useButton.onClick.AddListener(UseItem);
        dropButton.onClick.AddListener(DropItem);

        panel.gameObject.SetActive(false);
    }
    void Update()
    {
        if (panel.gameObject.activeSelf &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            !RectTransformUtility.RectangleContainsScreenPoint(panel, Mouse.current.position.ReadValue()))
        {
            Hide();
        }
    }

    public void Show(InventorySlot slot, int slotIndex)
    {
        activeSlot = slotIndex;

        itemNameText.text = slot.item.itemName;
        useButton.gameObject.SetActive(slot.item.isUsable);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out Vector2 pos
        );

        panel.anchoredPosition = pos;
        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
        activeSlot = -1;
    }

    void UseItem()
    {
        if (activeSlot < 0) return;
        var slot = InventoryManager.Instance.Slots[activeSlot];
        if (slot.IsEmpty) { Hide(); return; }

        // AQUÍ ESTÁ LA MAGIA: Guardamos el resultado (true o false)
        bool wasSuccessfullyUsed = slot.item.Use(player);

        // Solo consumimos el ítem si realmente se usó con éxito
        if (wasSuccessfullyUsed)
        {
            Debug.Log($"[Inventory] Used: {slot.item.itemName}");
            if (slot.item.isConsumable)
            {
                var item = InventoryManager.Instance.RemoveFromSlot(activeSlot, 1);
            }
        }

        Hide();
    }

    void DropItem()
    {
        if (activeSlot < 0) return;
        var item = InventoryManager.Instance.RemoveFromSlot(activeSlot, 1);
        if (item != null)
        {
            if (item.name == "Cellphone") 
            {
                PlayerStats.Instance.ChangeViewmodel("None");
            }
            itemDropper.Drop(item);
        }

        Hide();
    }
}