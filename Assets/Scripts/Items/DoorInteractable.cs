using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    public ItemDefinition Key;
    public Animator doorAnimator;

    [Header("Textos de Diálogo")]
    [TextArea]
    public string[] lockedDialogues = new string[] {
        "Está puerta necesita una llave",
    };

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

        }
        else
        {
            if (PopUpText.instance != null)
            {
                PopUpText.instance.MostrarDialogo(lockedDialogues);
            }
        }
    }

    public string GetInteractPrompt()
    {
        return isOpen ? "" : "Abrir Puerta"; // Si está abierta desaparece el texto, sino dice "Abrir Puerta"
    }
}