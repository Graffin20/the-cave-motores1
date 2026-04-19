using UnityEngine;
using TMPro;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject noteCanvasPanel;
    public TMP_Text noteTextUI;

    //Variable para saber si la nota está en pantalla
    public bool isNoteOpen = false;

    void Awake()
    {
       
        Instance = this;
    }

    void Start()
    {
        if (noteCanvasPanel != null)
            noteCanvasPanel.SetActive(false);
    }

    public void ShowNote(string textToDisplay)
    {
        if (noteCanvasPanel != null && noteTextUI != null)
        {
            noteTextUI.text = textToDisplay;
            noteCanvasPanel.SetActive(true);

            //Marks that the note is open
            isNoteOpen = true;
        }
    }

    public void CloseNote()
    {
        if (noteCanvasPanel != null)
        {
            noteCanvasPanel.SetActive(false);

            //Marks that the note was closed
            isNoteOpen = false;
        }
    }
}