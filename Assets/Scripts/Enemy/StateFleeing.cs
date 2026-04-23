using UnityEngine;

public class StateFleeing : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
        if (enemy.anim != null)
        {
          enemy.anim.SetTrigger("Walk");
        }
        enemy.Agent.enabled = true;
        enemy.Agent.ResetPath();
        enemy.Agent.SetDestination(enemy.escapePoint.position);
    }

    public override EnemyState Update(EnemyAI enemy)
    {
        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= 1.5f)
        {
            return enemy.GetState(StateID.Waiting);
        }
        return null;
    }
    public override void Exit(EnemyAI enemy)
    {
        if (enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.ResetPath();
        }
        enemy.gotShot = false;
    }
}