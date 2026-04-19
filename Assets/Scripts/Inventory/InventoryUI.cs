using UnityEngine;
using UnityEngine.InputSystem;

// Attach to the root Canvas GameObject that contains the inventory panel.
// Assign the slot prefab and the grid container in the Inspector.
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("References")]
    public GameObject inventoryPanel;       // The panel to show/hide
    public Transform slotContainer;         // The GridLayoutGroup parent
    public GameObject slotPrefab;           // InventorySlotUI prefab
    public ItemContextMenu contextMenu;     // The shared context menu

    [Header("Input")]
    public InputActionReference openInventoryAction;

    InventorySlotUI[] _slotUIs;
    public bool _isOpen;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        int total = InventoryManager.Instance.Slots.Length;
        _slotUIs = new InventorySlotUI[total];

        for (int i = 0; i < total; i++)
        {
            var go = Instantiate(slotPrefab, slotContainer);
            _slotUIs[i] = go.GetComponent<InventorySlotUI>();

            int idx = i;
            _slotUIs[i].OnSlotClicked += () => OnSlotClicked(idx);
        }

        InventoryManager.Instance.OnInventoryChanged += Refresh;

        inventoryPanel.SetActive(false);
    }

    void OnEnable()
    {
        openInventoryAction.action.performed += ToggleInventory;
    }

    void OnDisable()
    {
        openInventoryAction.action.performed -= ToggleInventory;

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
        Time.timeScale = 1f;
    }

    void ToggleInventory(InputAction.CallbackContext ctx)
    {
        _isOpen = !_isOpen;

        inventoryPanel.SetActive(_isOpen);

        if (_isOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Refresh();
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            contextMenu.Hide();
        }
    }

    void Refresh()
    {
        var slots = InventoryManager.Instance.Slots;
        for (int i = 0; i < _slotUIs.Length; i++)
            _slotUIs[i].SetSlot(slots[i]);
    }

    void OnSlotClicked(int slotIndex)
    {
        var slot = InventoryManager.Instance.Slots[slotIndex];
        if (slot.IsEmpty) { contextMenu.Hide(); return; }

        // Show context menu at the cursor position
        contextMenu.Show(slot, slotIndex);
    }
}
