using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Inventory/Items/Note")]
public class NoteItem : ItemDefinition
{
    [Header("Note Settings")]
    [TextArea(5, 10)] // This makes text box in the Unity Inspector
    public string noteContent = "Placeholder text here...";

    public override bool Use(GameObject player)
    {
        // Find the NoteManager in the scene to handle the UI
        NoteManager noteManager = FindAnyObjectByType<NoteManager>();

        if (noteManager != null)
        {
            noteManager.ShowNote(noteContent);
            Debug.Log("Reading note: " + itemName);
            return true; // Tells the inventory it was successfully used
        }

        Debug.LogWarning("Could not find a NoteManager in the scene!");
        return false; // Tells the inventory it failed, so it won't be consumed
    }
}
