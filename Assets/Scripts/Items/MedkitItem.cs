using UnityEngine;

[CreateAssetMenu(fileName = "New Medkit", menuName = "Inventory/Items/Medkit")]
public class MedkitItem : ItemDefinition
{
    [Header("Medkit Settings")]
    public int healAmount = 20;

    public override bool Use(GameObject player)
    {
        // 1. Check if health is already full
        if (PlayerStats.Instance.IsHealthFull())
        {
            Debug.Log("Health is already full! Keeping Medkit.");
            return false; // Tells the inventory NOT to consume it
        }

        // 2. If not full, heal them
        PlayerStats.Instance.AddHealth(healAmount);
        Debug.Log("The player wrapped their wounds. Healed " + healAmount + " HP!");

        return true; // Tells the inventory it was successfully used
    }
}