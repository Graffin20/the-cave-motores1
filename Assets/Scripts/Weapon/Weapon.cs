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
    public Transform cameraTransform;
    public GameObject bulletPrefab;

    [Header("Special Ammo")]
    public GameObject bulletPrefabEspecial;
    public bool tieneBalaEspecial = false;

    [Header("Extras")]
    public WeaponRecoil recoil;
    public Animator gunAnimator;
    public GameObject casingPrefab;
    public Transform casingSpawnPoint;
    public GameObject[] bulletGameobjects = new GameObject[6];
    public AudioSource audioSource;

    [Header("Audio")]
    public AudioClip[] gunShotSounds = new AudioClip[4];
    public AudioClip casingEjectionSound;

    private int _bulletIndex = 0;

    void Awake()
    {
        currentAmmo = magazineSize;
    }

    void OnAttack(InputValue value)
    {
        if (!value.isPressed || _isReloading || Time.time < _nextFireTime || (InventoryUI.Instance != null && InventoryUI.Instance._isOpen) || PlayerStats.Instance.CurrentViewmodel != "Weapon") return;

        if (currentAmmo > 0)
            Fire();
        else
            TryReload();
    }

    void OnReload(InputValue value)
    {
        if (value.isPressed && PlayerStats.Instance.CurrentViewmodel == "Weapon")
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

        GameObject balaAUsar = tieneBalaEspecial && bulletPrefabEspecial != null ? bulletPrefabEspecial : bulletPrefab;

        GameObject bullet = Instantiate(balaAUsar, shootPosition, Quaternion.LookRotation(shootDirection));

        if (tieneBalaEspecial)
        {
            tieneBalaEspecial = false;
        }

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

        if (casingPrefab != null)
        {
            GameObject casing = Instantiate(casingPrefab, casingSpawnPoint != null ? casingSpawnPoint.position : muzzlePoint.position, Quaternion.identity);
            Rigidbody casingRb = casing.GetComponent<Rigidbody>();
            if (casingRb != null)
            {
                float upwardForce = Random.Range(3f, 5f);
                float leftwardForce = Random.Range(2f, 4f);

                Vector3 localVelocity = new Vector3(-leftwardForce, upwardForce, 0f);
                Vector3 casingVelocity = transform.TransformDirection(localVelocity);

                casingRb.linearVelocity = casingVelocity;
                casingRb.angularVelocity = Random.onUnitSphere * 10f;
            }

            if (audioSource != null && casingEjectionSound != null)
                audioSource.PlayOneShot(casingEjectionSound);
        }

        if (_bulletIndex < bulletGameobjects.Length && bulletGameobjects[_bulletIndex] != null)
        {
            bulletGameobjects[_bulletIndex].SetActive(false);
            _bulletIndex++;
        }
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;

        if (CurrentAmmo == 0)
        {
            TryReload();
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