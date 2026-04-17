using UnityEngine;

public enum EnemyState
{
    Stalking,
    Chasing,
    Attacking,
    Fleeing
}

public class EnemyAI : MonoBehaviour
{
    [Header("Safe Zone")]
    public bool isPlayerInSafeZone = false;

    [Header("Settings")]
    [SerializeField] private EnemyState currentState = EnemyState.Stalking;
    [SerializeField] private Transform target;

    [Header("Distances")]
    [SerializeField] private float chaseRange = 15f;
    [SerializeField] private float attackRange = 2f;

    [Header("Timers")]
    [SerializeField] private float windowWaitTime = 10f;
    [SerializeField] private float fleeingDuration = 4f;
    private float windowTimer = 0f;

    private EnemyMovement movement;
    private EnemyAttack attack;

    void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case EnemyState.Stalking:
                movement.GoToWindow();

                windowTimer += Time.deltaTime;

                if (!isPlayerInSafeZone)
                {
                    if (windowTimer >= windowWaitTime || distance <= 5f)
                    {
                        ChangeState(EnemyState.Chasing);
                    }
                }
                break;

            case EnemyState.Chasing:
                movement.ChasePlayer(target.position);

                if (isPlayerInSafeZone)
                {
                    ChangeState(EnemyState.Stalking);
                    break;
                }

                if (distance <= attackRange)
                {
                    ChangeState(EnemyState.Attacking);
                }
                break;

            case EnemyState.Attacking:
                movement.StopMoving();
                attack.TryAttack(target);

                if (distance > attackRange)
                {
                    ChangeState(EnemyState.Chasing);
                }
                break;

            case EnemyState.Fleeing:
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState == EnemyState.Stalking) windowTimer = 0f;

        currentState = newState;
    }

    public void TakeHit()
    {
        if (currentState == EnemyState.Fleeing) return;

        ChangeState(EnemyState.Fleeing);
        movement.EscapeFrom(target.position);
        Invoke(nameof(EndFleeing), fleeingDuration);
    }

    private void EndFleeing()
    {
        movement.RestoreSpeed();
        ChangeState(EnemyState.Stalking);
    }
}