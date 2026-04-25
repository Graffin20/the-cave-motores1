using UnityEngine;
using TMPro;
using System.Collections;

public class PopUpText : MonoBehaviour
{
    public static PopUpText instance;

    [SerializeField] private GameObject elPanel;
    [SerializeField] private TextMeshProUGUI elTexto;

    void Awake()
    {
        // 1. Lógica de Singleton Correcta
        if (instance == null)
        {
            instance = this;
            // Opcional: DontDestroyOnLoad(gameObject); // Solo si querés que persista entre escenas
        }
        else
        {
            Destroy(gameObject);
            return; // Salimos para no ejecutar el resto si se destruye
        }

        // 2. Apagar el panel al inicio
        if (elPanel != null)
        {
            elPanel.SetActive(false);
        }
    }

    public void MostrarMensaje(string mensaje, float tiempo = 4f)
    {
        if (elPanel == null || elTexto == null)
        {
            Debug.LogError("Faltan referencias en el Inspector de PopUpText en el Canvas!");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(MensajeCo(mensaje, tiempo));
    }

    private IEnumerator MensajeCo(string mensaje, float tiempo)
    {
        elTexto.text = mensaje;
        elPanel.SetActive(true);
        yield return new WaitForSeconds(tiempo);
        elPanel.SetActive(false);
    }
}