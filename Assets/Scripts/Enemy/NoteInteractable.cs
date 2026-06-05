using UnityEngine;

public class NoteInteractable : MonoBehaviour, IInteractable
{
    [Header("Inventario")]
    public ItemDefinition itemDefinition;
    public int quantity = 1;

    [Header("Referencias de la Nota")]
    public GameObject notePanelUI;

    [Header("Evento Fase 1 (Spawn Juan Carlos)")]
    public EnemyAI juanCarlos;
    public Transform spawnZone;

    public void Interact()
    {
        if (itemDefinition != null && InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemDefinition, quantity);
        }

        if (notePanelUI != null)
        {
            notePanelUI.SetActive(true);
        }

        if (juanCarlos != null && spawnZone != null)
        {
            juanCarlos.gameObject.SetActive(true);
            juanCarlos.Agent.Warp(spawnZone.position);
            juanCarlos.ChangeState(StateID.Follow);
            juanCarlos.StartRoar();
        }

        gameObject.SetActive(false);
    }

    public string GetInteractPrompt()
    {
        return "Leer Nota";
    }
}