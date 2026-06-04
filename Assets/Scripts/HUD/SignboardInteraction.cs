using UnityEngine;

public class SignboardInteraction : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string[] textosDelLetrero = {
        "Cabaña a 5 metros.",
        "Debo ir para allá de inmediato"
    };

    [Header("Referencias")]
    [SerializeField] private PlayerMovement player;

    public void Interact()
    {
        // Llamamos al diálogo y le decimos: "Cuando termines, ejecuta esta acción"
        PopUpText.instance.MostrarDialogo(textosDelLetrero, () =>
        {
            // ESTO ocurre justo después de que el jugador hace el último clic
            if (player != null)
            {
                player.moveSpeed = 5f;
            }
        });

        // Desactivamos el collider para no leerlo dos veces
        GetComponent<Collider>().enabled = false;
    }

    public string GetInteractPrompt()
    {
        return "Leer Letrero";
    }
}