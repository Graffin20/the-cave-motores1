using UnityEngine;
using TMPro;

public class NoteUIManager : MonoBehaviour
{
    public static NoteUIManager Instance;

    [Header("UI References")]
    public GameObject notePanel;       
    public TextMeshProUGUI noteText;    

    private void Awake()
    {
        Instance = this;
        if (notePanel != null) notePanel.SetActive(false);
    }

    public void DisplayNote(string message)
    {
        if (notePanel == null) return;

        
        noteText.text = message;
        notePanel.SetActive(true);

        
        Time.timeScale = 0f;

        
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);

        
        Time.timeScale = 1f;

        
    }
}