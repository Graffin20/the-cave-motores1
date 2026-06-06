using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    [Header("Configuración de Audio")]
    public AudioSource audioSource;
    public bool playOnlyOnce = true; // Si está en true, suena 1 sola vez en todo el juego

    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        // Preguntamos si el que entró al trigger es el jugador y si no sonó antes
        if (other.CompareTag("Player") && !hasPlayed)
        {
            if (audioSource != null)
            {
                audioSource.Play();

                // Si marcaste que suene una sola vez, lo bloqueamos para el futuro
                if (playOnlyOnce)
                {
                    hasPlayed = true;
                }
            }
        }
    }
}
