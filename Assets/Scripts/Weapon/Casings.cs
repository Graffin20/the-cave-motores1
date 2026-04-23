using UnityEngine;

public class Casings : MonoBehaviour
{
    public AudioClip[] casingSounds = new AudioClip[4];
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        Invoke(nameof(DestroyCasing), 3f);
    }

    void DestroyCasing()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            PlayCasingSound();
        }
    }

    void PlayCasingSound()
    {
        if (casingSounds.Length == 0 || _audioSource == null) return;

        int index = Random.Range(0, casingSounds.Length);
        _audioSource.PlayOneShot(casingSounds[index]);
    }
}