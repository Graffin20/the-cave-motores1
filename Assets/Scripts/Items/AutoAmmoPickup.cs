using UnityEngine;

public class AutoAmmoPickup : MonoBehaviour, IInteractable
{
    public int ammoAmount = 6;

    [Header("Tipo de Munición")]
    public bool isSpecialAmmo = false;

    public void Interact()
    {
        Weapon revolver = Object.FindAnyObjectByType<Weapon>(FindObjectsInactive.Include);

        if (revolver != null)
        {
            revolver.AddAmmo(ammoAmount);
            if (isSpecialAmmo)
            {
                revolver.tieneBalaEspecial = true;
            }

            if (revolver.CurrentAmmo == 0)
            {
                revolver.Invoke("TryReload", 0.1f);
            }

            Destroy(gameObject);
        }
    }

    public string GetInteractPrompt()
    {
        if (isSpecialAmmo)
            return "Recoger Munición Especial";
        else
            return "Recoger Munición";
    }
}