using UnityEngine;
using UnityEngine.Events;

public class EventosdepruebaNota : MonoBehaviour
{
    [Header("Eventos")]
    public UnityEvent OnNotePlaced;

    private void OnMouseDown()
    {
        OnNotePlaced.Invoke();
    }
}