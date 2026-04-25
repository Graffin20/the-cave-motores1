using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Thecave/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Tiempos")]
    public float Firstspawntime = 10f;
    public float timeToShoot = 2.0f;
    public float respawnCooldown = 10f;
}