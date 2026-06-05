using UnityEngine;
using System.Collections;

public class KeypadInteraction : MonoBehaviour, IInteractable
{
    [Header("Referencias")]
    public GameObject keypadPanel;
    public MonoBehaviour PlayerMovement;
    public MonoBehaviour cameraLookScript;

    [Header("Textos de Diálogo")]
    [TextArea]
    public string[] lockedDialogues = new string[] {
        "Un tablero numérico, parece que pide un código para abrir la caja",
    };

    // NUEVO: Variable para saber si ya se resolvió el puzzle
    [HideInInspector]
    public bool isUnlocked = false;

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

        StartCoroutine(EnableCameraWithDelay());
    }

    private IEnumerator EnableCameraWithDelay()
    {
        yield return new WaitForSeconds(0.1f);

        if (cameraLookScript != null) cameraLookScript.enabled = true;
    }

    public void Interact()
    {
        if (isUnlocked) return;

        if (PopUpText.instance != null)
        {
            PopUpText.instance.MostrarDialogo(lockedDialogues, OpenKeypad);
        }
        else
        {
            OpenKeypad();
        }
    }

    public string GetInteractPrompt()
    {
        if (isUnlocked) return "";

        return "Interactuar con Teclado";
    }
}