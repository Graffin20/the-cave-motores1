using UnityEngine;

public class KeypadInteraction : MonoBehaviour, IInteractable
{
    [Header("Referencias")]
    public GameObject keypadPanel;
    public MonoBehaviour PlayerMovement;
    public MonoBehaviour cameraLookScript;

    [Header("Textos de Diálogo")]
    [TextArea]
    public string[] lockedDialogues = new string[] {
        "Un tablero numerico, parece que pide un codigo para abrir la caja",
    };

    public void OpenKeypad()
    {
        keypadPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (PlayerMovement != null) PlayerMovement.enabled = false;
        if (cameraLookScript != null) cameraLookScript.enabled = false;
    }

    void Update()
    {
        if (keypadPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }
    }
    public void CloseKeypad()
    {
        keypadPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerMovement != null) PlayerMovement.enabled = true;
        if (cameraLookScript != null) cameraLookScript.enabled = true;
    }

    public void Interact()
    {
        if (PopUpText.instance != null)
        {
            PopUpText.instance.MostrarDialogo(lockedDialogues, OpenKeypad);
        }
        else
        {
            OpenKeypad();
        }
    }
    public string GetInteractPrompt() => "Interactuar con Teclado";
}