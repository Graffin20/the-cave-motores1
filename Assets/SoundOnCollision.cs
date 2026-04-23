using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    public AudioClip[] soundEffects = new AudioClip[0];
    public float detectionRadius = 0.5f;
    private AudioSource audioSource;
    private bool hasPlayedOnce = false;
    private float lastPlayTime = -1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Use OverlapSphere to detect player proximity without relying on collider callbacks
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        bool playerNearby = false;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerNearby = true;
                break;
            }
        }

        // Play sound once per proximity detection
        if (playerNearby && !hasPlayedOnce && Time.time - lastPlayTime > 0.5f)
        {
            PlayRandomSound();
            Debug.Log("Collision with player detected!");
            hasPlayedOnce = true;
        }
        else if (!playerNearby)
        {
            hasPlayedOnce = false;
        }
    }

    void PlayRandomSound()
    {
        if (audioSource == null || soundEffects.Length == 0) return;

        int index = Random.Range(0, soundEffects.Length);
        audioSource.PlayOneShot(soundEffects[index]);
        lastPlayTime = Time.time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}