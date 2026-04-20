using UnityEngine;

[CreateAssetMenu(fileName = "Cellphone", menuName = "Inventory/Items/Cellphone")]
public class CellphoneItem : ItemDefinition
{
    public override bool Use(GameObject player)
    {
        PlayerStats.Instance.ChangeViewmodel("Cellphone");
        Debug.Log("Cellphone Equipped");
        return true;
    }
}
