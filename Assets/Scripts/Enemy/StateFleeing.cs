using UnityEngine;

public class StateFleeing : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        if (enemy.anim != null)
        {
            enemy.anim.SetTrigger("damage");
        }

        if (enemy.shotreceived != null)
        {
            enemy.shotreceived.Invoke();
        }

        if (enemy.Agent != null)
        {
            enemy.Agent.enabled = true;
            enemy.Agent.speed = 8f;

            enemy.Agent.ResetPath();
            enemy.Agent.SetDestination(enemy.escapePoint.position);
        }
    }

    public override EnemyState Update(EnemyAI enemy)
    {
        if (enemy.Agent != null && enemy.Agent.isOnNavMesh)
        {
            bool reachedDestination = enemy.Agent.remainingDistance <= 2.5f;
            bool isStuck = enemy.Agent.velocity.sqrMagnitude < 0.1f && enemy.Agent.remainingDistance <= 4f;

            if (!enemy.Agent.pathPending && (reachedDestination || isStuck))
            {
                return enemy.GetState(StateID.Waiting);
            }
        }
        return null;
    }

    public override void Exit(EnemyAI enemy)
    {
        if (enemy.Agent != null && enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.ResetPath();
        }

        if (enemy.Agent != null)
        {
            enemy.Agent.speed = 3.5f;
        }

        enemy.gotShot = false;
    }
}