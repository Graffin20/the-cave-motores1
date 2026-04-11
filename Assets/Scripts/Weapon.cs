using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("Ammo")]
    public int magazineSize = 30;
    public int reserveAmmo = 90;
    private int _currentAmmo;
    private bool _isReloading = false;

    public enum FiringMode { SemiAuto, FullAuto }
    public FiringMode firingMode = FiringMode.FullAuto;
    public float fireRate = 10f; // rounds per second
    private float _nextFireTime = 0f;
    private bool _triggerHeld = false;
    private bool _triggerFired = false; // semi-auto: did we already fire this press?

    [Header("Reload")]
    public float reloadTime = 2f;
    private float _reloadTimer = 0f;

    [Header("References")]
    public Transform muzzlePoint;
    public GameObject bulletPrefab;
    public WeaponRecoil recoil;

    void Awake()
    {
        _currentAmmo = magazineSize;
    }

    // --- Input callbacks (wire these up via PlayerInput / Send Messages) ---

    void OnAttack(InputValue value)
    {
        if (firingMode == FiringMode.FullAuto)
        {
            _triggerHeld = value.isPressed;
        }
        else
        {
            // Semi-auto: only register the press edge, not hold
            if (value.isPressed)
            {
                _triggerHeld = true;
                _triggerFired = false;
            }
            else
            {
                _triggerHeld = false;
                _triggerFired = false;
            }
        }
    }

    void OnReload(InputValue value)
    {
        if (value.isPressed)
            TryReload();
    }

    void OnSwitchFireMode(InputValue value)
    {
        if (value.isPressed)
        {
            firingMode = firingMode == FiringMode.FullAuto
                ? FiringMode.SemiAuto
                : FiringMode.FullAuto;

            _triggerHeld = false;
            _triggerFired = false;
        }
    }

    // --- Update ---

    void Update()
    {
        if (_isReloading)
        {
            _reloadTimer -= Time.deltaTime;
            if (_reloadTimer <= 0f)
                FinishReload();
            return;
        }

        bool canFire = firingMode == FiringMode.FullAuto
            ? _triggerHeld
            : _triggerHeld && !_triggerFired;

        if (canFire && Time.time >= _nextFireTime)
        {
            if (_currentAmmo > 0)
            {
                Fire();
                if (firingMode == FiringMode.SemiAuto)
                    _triggerFired = true;
            }
            else
            {
                TryReload();
            }
        }
    }

    // --- Firing ---

    void Fire()
    {
        _currentAmmo--;
        _nextFireTime = Time.time + 1f / fireRate;

        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);

        recoil.ApplyRecoil();
    }

    // --- Reload ---

    void TryReload()
    {
        if (_isReloading) return;
        if (_currentAmmo == magazineSize) return;
        if (reserveAmmo <= 0) return;

        _isReloading = true;
        _reloadTimer = reloadTime;
    }

    void FinishReload()
    {
        int needed = magazineSize - _currentAmmo;
        int taken = Mathf.Min(needed, reserveAmmo);

        _currentAmmo += taken;
        reserveAmmo -= taken;
        _isReloading = false;
    }

    // --- Public accessors for UI ---
    public int CurrentAmmo => _currentAmmo;
    public int ReserveAmmo => reserveAmmo;
    public bool IsReloading => _isReloading;
}