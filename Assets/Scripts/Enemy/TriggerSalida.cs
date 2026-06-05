using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public EnemyAI Enemy;
    public Transform SpawnExterior;

    private bool _isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isActivated)
        {
            if (ReloadLevel.isPhase2Active)
            {
                Debug.Log("probando si me tomo el tigger " + ReloadLevel.isPhase2Active);
                ActivateExitSequence();
            }
        }
    }

    private void ActivateExitSequence()
    {
        _isActivated = true;

        if (Enemy != null && Enemy.Agent != null && SpawnExterior != null)
        {
            Enemy.Agent.enabled = false;
            Enemy.transform.position = SpawnExterior.position;
            Enemy.Agent.enabled = true;
            Enemy.ChangeState(StateID.Follow);
        }
    }
}