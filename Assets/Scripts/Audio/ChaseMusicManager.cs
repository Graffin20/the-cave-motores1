using UnityEngine;

public class ChaseMusicManager : MonoBehaviour
{
    // Hacemos que sea un Singleton para llamarlo desde cualquier lado
    public static ChaseMusicManager instance;

    [Header("Configuración de Audio")]
    public AudioSource audioSource;
    public AudioClip chaseMusic;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Esta función la vamos a llamar desde la Llave
    public void PlayChaseMusic()
    {
        if (audioSource != null && chaseMusic != null)
        {
            audioSource.clip = chaseMusic;
            audioSource.loop = true; // Para que no se corte si tardás en matar al bicho
            audioSource.Play();
        }
    }

    // Esta función la vamos a llamar desde el Enemigo al morir
    public void StopChaseMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}