using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    public ItemDefinition Key;
    public Animator doorAnimator;

    private bool isOpen = false;

    public void Interact()
    {
        if (isOpen) return;

        if (InventoryManager.Instance.HasItem(Key))
        {   

            doorAnimator.SetTrigger("Abrirpuerta");
            isOpen = true;
            InventoryManager.Instance.RemoveItem(Key);
            Debug.Log("Puerta abierta con éxito.");

            // Opcional: Podrías hacer que la llave se consuma acá
            // InventoryManager.Instance.RemoveItem(Key);
        }
        else
        {
            Debug.Log("Parece que necesito una llave...");

            // Si tenés tu sistema de diálogos que armamos el otro día, lo llamarías así:
            // DialogueManager.Instance.ShowDialogue(new string[] { "Esta puerta está trabada.", "Parece que necesito una llave específica para entrar." });
        }
    }

    public string GetInteractPrompt()
    {
        return isOpen ? "" : "Abrir Puerta"; // Si está abierta desaparece el texto, sino dice "Abrir Puerta"
    }
}