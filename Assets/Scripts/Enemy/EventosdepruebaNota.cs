using UnityEngine;
using UnityEngine.Events;

public class EventosdepruebaNota : MonoBehaviour
{
    public GameObject textoUI;

    [Header("Eventos")]
    public UnityEvent OnNotePlaced;

    private void Start()
    {
        if (textoUI != null)
        {
            textoUI.SetActive(false);
        }
    }
    private void OnMouseEnter()
    {
        if (textoUI != null)
        {
            textoUI.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        if (textoUI != null)
        {
            textoUI.SetActive(false);
        }
    }
    private void OnMouseDown()
    {
        OnNotePlaced.Invoke();
        if (textoUI != null)
        {
            textoUI.SetActive(false);
        }
        this.enabled = false;
    }
}