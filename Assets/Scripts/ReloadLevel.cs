using UnityEngine;

public class ReloadLevel : MonoBehaviour
{
    public static bool isPhase2Active = false;
    private void Awake()
    {
        isPhase2Active = false;

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ClearInventory();
        }

    }
}