using UnityEngine;

public class InitialDialogue : MonoBehaviour
{
    [SerializeField]
    private string[] lineasDeInicio = {
        "Ugh... mi cabeza...",
        "¿Donde Cai?",
        "Está muy oscuro... Debería buscar mi celular para iluminar."
    };

    [Header("Referencias")]
    [SerializeField] private CharacterController playerController;

    private bool dialogoIniciado = false;

    void Update()
    {
        if (playerController == null) return;

        if (!dialogoIniciado && playerController.isGrounded)
        {
            dialogoIniciado = true;

            PopUpText.instance.MostrarDialogo(lineasDeInicio);

            this.enabled = false;
        }
    }
}