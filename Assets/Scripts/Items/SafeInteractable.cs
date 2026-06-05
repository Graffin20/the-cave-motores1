using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SafeInteractable : MonoBehaviour, IInteractable
{
    [Header("Estado")]
    public bool isLocked = true;

    [Header("Interfaz de la Caja")]
    public GameObject safePanelUI;

    public void UnlockSafe()
    {
        isLocked = false;
        BotonPrueba.iPhase2 = true;
    }

    public void Interact()
    {
        if (!isLocked)
        {
            safePanelUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public string GetInteractPrompt()
    {
        if (isLocked)
        {
            return "Caja Fuerte (Bloqueada)";
        }
        else
        {
            return "Revisar Caja Fuerte";
        }
    }
}