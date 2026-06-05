using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class KeypadController : MonoBehaviour
{
    [Header("Configuración del Tablero")]
    public string correctCode = "7777";
    public int maxDigits = 4;
    public TextMeshProUGUI displayText;

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
            onUnlock.Invoke();
        }
        else
        {
            displayText.text = "ERR";
            Invoke("ClearInput", 1.5f);
        }
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