using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SafeInteractable : MonoBehaviour, IInteractable
{
    [Header("Estado")]
    public bool isLocked = true;

    [Header("Interfaz de la Caja")]
    public GameObject safePanelUI; // Arrastrá acá el Panel 2D de la caja fuerte

    // Esta función la va a disparar el Keypad cuando pongas la clave correcta
    public void UnlockSafe()
    {
        isLocked = false;
    }

    // Esta función la llama tu PlayerInteract al apretar la 'E'
    public void Interact()
    {
        if (isLocked)
        {
            Debug.Log("La caja está trabada. Necesito el código.");
        }
        else
        {
            // Abrimos el panel de la caja fuerte y liberamos el mouse
            safePanelUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // El texto que lee tu Raycast para mostrar en el medio de la pantalla
    public string GetInteractPrompt()
    {
        return isLocked ? "Caja Fuerte (Bloqueada)" : "Revisar Caja Fuerte";
    }
}
