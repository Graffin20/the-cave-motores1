using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Speed of movement
    [SerializeField] private GameObject hitEffect;

    Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, 3f);
    }

    // Called when the object collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Destroy this object upon collision
        Destroy(gameObject);
        SpawnEffect();
    }
    private void SpawnEffect()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }

}
