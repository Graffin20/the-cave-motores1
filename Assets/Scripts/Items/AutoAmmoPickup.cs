using UnityEngine;

public class AutoAmmoPickup : MonoBehaviour, IInteractable
{
    public int ammoAmount = 6;

    public void Interact()
    {
        // Busca el arma en el jugador
        Weapon revolver = Object.FindAnyObjectByType<Weapon>(FindObjectsInactive.Include);

        if (revolver != null)
        {
            // Sumamos al bolsillo (Reserva)
            revolver.AddAmmo(ammoAmount);

            // Si el arma está vacía, forzamos la recarga automática
            if (revolver.CurrentAmmo == 0)
            {
                // Este método lo llamamos por Reflection o directamente si es público
                // Si te da error, fijate cómo se llama exactamente tu método de recarga
                revolver.Invoke("TryReload", 0.1f);
            }

            Debug.Log("Balas cargadas instantáneamente");

            // Destruimos el objeto del mundo
            Destroy(gameObject);
        }
    }

    public string GetInteractPrompt() => "Recoger Munición";
}