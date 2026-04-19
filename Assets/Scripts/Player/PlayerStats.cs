using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    private void Awake()
    {
        Instance = this;
    }

    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
    }

    public bool IsHealthFull()
    {
        return currentHealth >= maxHealth;
    }
}
