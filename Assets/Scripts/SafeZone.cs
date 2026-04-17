using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
            foreach (EnemyAI enemy in enemies)
            {
                enemy.isPlayerInSafeZone = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
            foreach (EnemyAI enemy in enemies)
            {
                enemy.isPlayerInSafeZone = false;
            }
        }
    }
}