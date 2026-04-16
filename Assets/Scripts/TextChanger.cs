using UnityEngine;

public class TextChanger : MonoBehaviour
{
    public void UpdateText(TMPro.TextMeshProUGUI tmp, string text)
    {
        tmp.text = text;
    }
}
