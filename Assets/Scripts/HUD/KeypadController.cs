using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class KeypadController : MonoBehaviour
{
    [Header("Configuración del Tablero")]
    public string correctCode = "7777";
    public int maxDigits = 4;
    public TextMeshProUGUI displayText;

    [Header("Referencias Extra (Animación y Cierre)")]
    public Animator cajaFuerteAnimator; // Arrastrar la caja fuerte acá
    public KeypadInteraction keypadInteraction; // Arrastrar el script de interacción acá

    [Header("Eventos")]
    public UnityEvent onUnlock;

    private string currentInput = "";

    void Start()
    {
        ClearInput();
    }

    public void AddDigit(string digit)
    {
        if (currentInput.Length < maxDigits)
        {
            currentInput += digit;
            UpdateDisplay();
            Debug.Log(digit);
        }
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateDisplay();
    }

    public void CheckCode()
    {
        if (currentInput == correctCode)
        {
            displayText.text = "OPEN";

            // 1. Activar la animación de la caja fuerte
            if (cajaFuerteAnimator != null)
            {
                cajaFuerteAnimator.SetTrigger("AbrirCaja");
            }

            // 2. Invocar cualquier otro evento (sonidos, etc.)
            onUnlock.Invoke();

            // 3. Cerrar el teclado automáticamente después de 1 segundo
            Invoke("CloseAndClear", 1f);
        }
        else
        {
            displayText.text = "ERR";
            Invoke("ClearInput", 1.5f);
        }
    }

    private void CloseAndClear()
    {
        if (keypadInteraction != null)
        {
            keypadInteraction.CloseKeypad();
        }
        ClearInput();
    }

    private void UpdateDisplay()
    {
        if (currentInput.Length == 0)
        {
            displayText.text = "----";
        }
        else
        {
            displayText.text = currentInput;
        }
    }
}