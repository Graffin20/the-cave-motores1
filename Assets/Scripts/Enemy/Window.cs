using UnityEngine;

public class Window : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyAI enemy = FindFirstObjectByType<EnemyAI>();

            if (enemy != null)
            {
                enemy.StartSpawns();
            }

            gameObject.SetActive(false);
        }
    }
}