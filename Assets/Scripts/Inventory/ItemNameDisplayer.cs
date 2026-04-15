using UnityEngine;

public class ItemNameDisplayer : MonoBehaviour
{
    [SerializeField] private PlayerInteract playerInteract;
    private TMPro.TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        text.enabled = false;
    }

    void Update()
    {
        if (!playerInteract.IsLookingAtItem)
        {
            text.enabled = false;
            return;
        }

        text.enabled = true;
        InventoryManager.Instance.UpdateText(text, playerInteract.TargetItemName);
    }
}
