using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float timeBetweenAttacks = 1.5f;

    private float cooldownTimer = 0f;

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void TryAttack(Transform target)
    {
        if (cooldownTimer <= 0f)
        {
            PerformAttack(target);
            cooldownTimer = timeBetweenAttacks;
        }
    }

    private void PerformAttack(Transform target)
    {
        Debug.Log("Prueba de daño: " + damage);
    }
}