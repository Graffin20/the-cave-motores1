using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Thecave/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Tiempos")]
    public float Firstspawntime;
    public float timeToShoot;
    public float respawnCooldown;
    public float FollowSpeed;
    public float Stuntime;
    public float roartime;

    [Header("Sonidos que uso en codigo")]
    public AudioClip sfxSpawn;
}