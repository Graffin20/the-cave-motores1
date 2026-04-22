using UnityEngine;

public class StateFleeing : EnemyState
{
    public override void Enter(EnemyAI enemy)
    {
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
}