using UnityEngine;
using UnityEngine.EventSystems;

public class DisableOnClickOutside : MonoBehaviour, IPointerDownHandler
{
    private bool _clickedInside;

    public void OnPointerDown(PointerEventData eventData) => _clickedInside = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!_clickedInside)
                gameObject.SetActive(false);
            _clickedInside = false;
        }
    }
}