using UnityEngine;

public class SafeUI : MonoBehaviour
{
    [Header("Configuración del Ítem")]
    public ItemDefinition itemInside; // Arrastrá tu ScriptableObject (LlaveBunker) acá
    public int quantity = 1;

    [Header("Visuales")]
    public GameObject itemButton; // El botón de la llave para ocultarlo cuando la agarres

    // Esta función va en el OnClick del botón de la llave
    public void TakeItem()
    {
        if (itemInside == null) return;

        // Intentamos mandarlo a tu inventario
        bool added = InventoryManager.Instance.AddItem(itemInside, quantity);

        if (added)
        {
            // Ocultamos el botón de la llave porque ya la agarramos
            itemButton.SetActive(false);
            ClosePanel();
        }
    }

    // Esta función va en el OnClick del botón "X" para salir
    public void ClosePanel()
    {
        gameObject.SetActive(false);

        // Ocultamos el mouse y se lo devolvemos a la cámara del jugador
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}