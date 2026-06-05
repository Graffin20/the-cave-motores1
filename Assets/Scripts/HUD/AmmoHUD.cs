using TMPro;
using UnityEngine;

public class AmmoHUD : MonoBehaviour
{
    public Weapon weapon;
    public TextMeshProUGUI ammoText;

    void Update()
    {
        if (weapon == null) return;

        bool isWeaponView = PlayerStats.Instance.CurrentViewmodel == "Weapon";

        ammoText.enabled = isWeaponView;

        if (!isWeaponView) return;

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