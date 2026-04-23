using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Inventory/Items/Gun")]
public class GunItem : ItemDefinition
{
    public override bool Use(GameObject player)
    {
        PlayerStats.Instance.ChangeViewmodel("Weapon");
        Debug.Log("Weapon Equipped");
        return true;
    }
}