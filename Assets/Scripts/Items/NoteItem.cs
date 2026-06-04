using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Inventory/Items/Note")]
public class NoteItem : ItemDefinition
{
    [Header("Note Settings")]
    [TextArea(5, 10)]
    public string noteContent = "Placeholder text here...";

    public override bool Use(GameObject player)
    {
        if (NoteManager.Instance != null)
        {
            NoteManager.Instance.ShowNote(noteContent);
            Debug.Log("Reading note: " + itemName);
            return true;
        }

        Debug.LogWarning("¡Falta el NoteManager en la escena!");
        return false;
    }
}
