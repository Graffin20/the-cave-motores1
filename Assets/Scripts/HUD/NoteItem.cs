using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Inventory/Items/Note")]
public class NoteItem : ItemDefinition
{
    [Header("Note Content")]
    [TextArea(5, 10)]
    public string noteMessage;

    
    public override void Use(GameObject player)
    {
        if (NoteUIManager.Instance != null)
        {
            NoteUIManager.Instance.DisplayNote(noteMessage);
        }
    }
}