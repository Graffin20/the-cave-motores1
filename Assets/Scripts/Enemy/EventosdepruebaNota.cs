using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EventodepruebaNota : MonoBehaviour, IInteractable
{
    [Header("Configuración del Enemigo")]

    public EnemyStats EnemyStats;

    [Header("Eventos")]
    public UnityEvent OnNotePlaced;

    public void Interact()
    {
        // 1. Llamamos al diálogo
        string[] dialogo = { "Ese sonido vino de abajo..." };

        PopUpText.instance.MostrarDialogo(dialogo, () => {

            if (EnemyStats != null)
            {
                StartCoroutine(EsperarYSpawnear(EnemyStats.Firstspawntime));
            }
        });

        GetComponent<Collider>().enabled = false;
    }

    private IEnumerator EsperarYSpawnear(float tiempoDeEspera)
    {
        yield return new WaitForSeconds(tiempoDeEspera);

        OnNotePlaced?.Invoke();
    }

    public string GetInteractPrompt()
    {
        return "Leer";
    }
}