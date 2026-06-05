using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SafeInteractable : MonoBehaviour, IInteractable
{
    [Header("Estado")]
    public bool isLocked = true;
    private bool isPanelOpen = false;

    [Header("Interfaz")]
    public GameObject safePanelUI;

    [Header("Referencias de Control")]
    public MonoBehaviour playerMovement;
    public MonoBehaviour cameraLookScript;

    void Update()
    {
        if (isPanelOpen && !safePanelUI.activeSelf)
        {
            CloseSafe();
        }


        if (isPanelOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSafe();
        }
    }

    public void UnlockSafe() => isLocked = false;

    public void Interact()
    {
        if (!isLocked) OpenSafe();
        else Debug.Log("Caja trabada");
    }

    private void OpenSafe()
    {
        safePanelUI.SetActive(true);
        isPanelOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerMovement != null) playerMovement.enabled = false;
        if (cameraLookScript != null) cameraLookScript.enabled = false;
    }

    public void CloseSafe()
    {
        safePanelUI.SetActive(false);
        isPanelOpen = false; // Le avisamos que ya se cerró

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerMovement != null) playerMovement.enabled = true;
        if (cameraLookScript != null) cameraLookScript.enabled = true;
    }

    public string GetInteractPrompt() => isLocked ? "Caja Fuerte (Bloqueada)" : "Revisar Caja Fuerte";
}