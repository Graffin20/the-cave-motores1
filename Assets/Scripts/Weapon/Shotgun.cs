using UnityEngine;
using UnityEngine.InputSystem;

public class Shotgun : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private int magazineSize = 8; // Ajustá según tu escopeta
    [SerializeField] private int reserveAmmo = 24;
    public int currentAmmo;
    private bool _isReloading = false;

    [Header("Firing")]
    public float fireRate = 1f; // Las escopetas suelen ser más lentas
    private float _nextFireTime = 0f;

    [Header("Reload")]
    public float reloadTime = 3f;
    private float _reloadTimer = 0f;

    [Header("References")]
    public Transform muzzlePoint;
    public Transform cameraTransform;
    public GameObject bulletPrefab;
    public WeaponRecoil recoil;
    public Animator gunAnimator;
    public GameObject casingPrefab;
    public Transform casingSpawnPoint;
    public AudioSource audioSource;

    [Header("Audio")]
    public AudioClip[] gunShotSounds = new AudioClip[4];
    public AudioClip casingEjectionSound;

    void Awake()
    {
        currentAmmo = magazineSize;
    }



    void OnAttack(InputValue value)
    {
        Debug.Log("Intentando disparar. Viewmodel actual: " + PlayerStats.Instance.CurrentViewmodel);
        if (!value.isPressed || _isReloading || Time.time < _nextFireTime || (InventoryUI.Instance != null && InventoryUI.Instance._isOpen) || PlayerStats.Instance.CurrentViewmodel != "Shotgun") return;

        if (currentAmmo > 0)
            Fire();
        else
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

        Vector3 shootPosition = muzzlePoint.position;
        Vector3 shootDirection = cameraTransform != null ? cameraTransform.forward : muzzlePoint.forward;

        // Aquí podrías instanciar múltiples balas si querés el efecto "spread" (dispersión)
        GameObject bullet = Instantiate(bulletPrefab, shootPosition, Quaternion.LookRotation(shootDirection));

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = shootDirection * 50f;
        }

        recoil.ApplyRecoil();

        if (gunAnimator != null)
            gunAnimator.SetTrigger("GunShoot");

        if (audioSource != null && gunShotSounds.Length > 0)
        {
            AudioClip randomGunShot = gunShotSounds[Random.Range(0, gunShotSounds.Length)];
            if (randomGunShot != null)
                audioSource.PlayOneShot(randomGunShot);
        }

        // Casquillo (Casing)
        if (casingPrefab != null)
        {
            GameObject casing = Instantiate(casingPrefab, casingSpawnPoint != null ? casingSpawnPoint.position : muzzlePoint.position, Quaternion.identity);
            Rigidbody casingRb = casing.GetComponent<Rigidbody>();
            if (casingRb != null)
            {
                casingRb.linearVelocity = transform.TransformDirection(new Vector3(-2f, 3f, 0f));
                casingRb.angularVelocity = Random.onUnitSphere * 10f;
            }
            if (audioSource != null && casingEjectionSound != null)
                audioSource.PlayOneShot(casingEjectionSound);
        }
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