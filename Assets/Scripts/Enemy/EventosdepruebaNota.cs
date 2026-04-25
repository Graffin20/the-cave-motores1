using UnityEngine;
using UnityEngine.Events;

public class EventosdepruebaNota : MonoBehaviour
{
    public GameObject textoUI; // Este es el "Click para iniciar" (SpawnMonster)

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
        // 1. LLAMAMOS AL PANEL DE TEXTO (PopUpText)
        if (PopUpText.instance != null)
        {
            PopUpText.instance.MostrarMensaje("Ese sonido vino de abajo...");
        }

        // 2. DISPARAMOS EL EVENTO (Aquí es donde probablemente se activa el spawn del enemigo)
        OnNotePlaced.Invoke();

        // 3. LIMPIEZA
        if (textoUI != null)
        {
            textoUI.SetActive(false);
        }

        // Desactivamos este script para que no se pueda clickear de nuevo
        this.enabled = false;

        // OPCIONAL: Si el objeto tiene un collider y no quieres que se 
        // pueda clickear NADA más de este objeto, podrías usar:
        // GetComponent<Collider>().enabled = false;
    }
}