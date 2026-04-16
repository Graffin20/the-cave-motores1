using UnityEngine;

// Change the menu path to whatever category makes sense.
[CreateAssetMenu(fileName = "NewTestItem", menuName = "Inventory/Items/Test Item")]
public class TestItem : ItemDefinition
{
    // public int healAmount = 25;

    public override void Use(GameObject user)
    {
        // var health = user.GetComponent<PlayerHealth>();
        //if (health != null)
        // {
            // health.Heal(healAmount);
            Debug.Log($"Used {itemName}");
        // }
    }
}