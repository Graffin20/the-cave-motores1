using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private int magazineSize = 21;
    [SerializeField] private int reserveAmmo = 7;
    public int currentAmmo;
    private bool _isReloading = false;

    [Header("Firing")]
    public float fireRate = 10f;
    private float _nextFireTime = 0f;

    [Header("Reload")]
    public float reloadTime = 2f;
    private float _reloadTimer = 0f;

    [Header("References")]
    public Transform muzzlePoint;
    public GameObject bulletPrefab;
    public WeaponRecoil recoil;

    void Awake()
    {
        currentAmmo = magazineSize;
    }

    void OnAttack(InputValue value)
    {
        if (!value.isPressed || _isReloading || Time.time < _nextFireTime || InventoryUI.Instance._isOpen) return;

        if (currentAmmo > 0)
            Fire();
        else
            TryReload();
    }

    void OnReload(InputValue value)
    {
        if (value.isPressed)
            TryReload();
    }

    void Update()
    {
        if (!_isReloading) return;

        _reloadTimer -= Time.deltaTime;
        if (_reloadTimer <= 0f)
            FinishReload();
    }

    void Fire()
    {
        currentAmmo--;
        _nextFireTime = Time.time + 1f / fireRate;
        Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        recoil.ApplyRecoil();
    }

    void TryReload()
    {
        if (_isReloading || currentAmmo == magazineSize || reserveAmmo <= 0) return;

        _isReloading = true;
        _reloadTimer = reloadTime;
    }

    void FinishReload()
    {
        int taken = Mathf.Min(magazineSize - currentAmmo, reserveAmmo);
        currentAmmo += taken;
        reserveAmmo -= taken;
        _isReloading = false;
    }

    public int CurrentAmmo => currentAmmo;
    public int ReserveAmmo => reserveAmmo;
    public bool IsReloading => _isReloading;
}