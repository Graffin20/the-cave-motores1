using UnityEngine;
using UnityEngine.Events;

public class BotonPrueba : MonoBehaviour
{
    public static bool iPhase2 = false;

    [Header("Optional Events")]
    public UnityEvent OnPressed;

    private bool _isPlayerNearby = false;
    private bool _isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
        }
    }

    private void Update()
    {
        if (_isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !_isUsed)
        {
            ActivatePhase2();
        }
    }

    private void ActivatePhase2()
    {
        _isUsed = true;
        iPhase2 = true;

        if (OnPressed != null)
        {
            OnPressed.Invoke();
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.green;
        }
    }
}