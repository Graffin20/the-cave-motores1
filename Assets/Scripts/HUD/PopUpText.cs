using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class PopUpText : MonoBehaviour
{
    public static PopUpText instance;

    [SerializeField] private GameObject elPanel;
    [SerializeField] private TextMeshProUGUI elTexto;

    [Header("Sonido de Diálogo")]
    public AudioSource audioSource;
    public AudioClip dialogueSound;

    private string[] lineasActivas;
    private int indiceActual;
    private bool enDialogo = false;
    private Action accionAlTerminar;

    void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        if (elPanel != null) elPanel.SetActive(false);
    }

    // Nuevo método que recibe varias líneas y una acción opcional
    public void MostrarDialogo(string[] lineas, Action alTerminar = null)
    {
        lineasActivas = lineas;
        indiceActual = 0;
        enDialogo = true;
        accionAlTerminar = alTerminar;

        elPanel.SetActive(true);
        elTexto.text = lineasActivas[indiceActual];

        // Pausar el juego
        Time.timeScale = 0f;
    }

    void Update()
    {

        if (enDialogo && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            AvanzarDialogo();

            if (audioSource != null && dialogueSound != null)
            {
                audioSource.PlayOneShot(dialogueSound);
            }
        }
    }

    void AvanzarDialogo()
    {
        indiceActual++;

        // Si aún hay líneas, mostramos la siguiente
        if (indiceActual < lineasActivas.Length)
        {
            elTexto.text = lineasActivas[indiceActual];
        }
        else
        {
            // Si no hay más líneas, terminamos
            enDialogo = false;
            elPanel.SetActive(false);

            // Reanudar el juego
            Time.timeScale = 1f;

            // Ejecutar la acción final (si es que enviamos alguna)
            accionAlTerminar?.Invoke();
        }
    }
}