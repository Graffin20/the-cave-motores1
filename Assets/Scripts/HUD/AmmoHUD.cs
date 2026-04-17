using TMPro;
using UnityEngine;

public class AmmoHUD : MonoBehaviour
{
    public Weapon weapon;
    public TextMeshProUGUI ammoText;

    void Update()
    {
        if (weapon == null) return;

        if (weapon.IsReloading)
        {
            ammoText.text = "RELOADING...";
        }
        else
        {
            ammoText.text = weapon.CurrentAmmo + " / " + weapon.ReserveAmmo;
        }
    }
}