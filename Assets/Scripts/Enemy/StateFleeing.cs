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

        if (enemy.escapePoints != null && enemy.escapePoints.Length > 0)
        {
            Transform closestPoint = null;
            float minDistance = Mathf.Infinity;
            Vector3 currentPosition = enemy.transform.position;

            foreach (Transform point in enemy.escapePoints)
            {
                float distance = Vector3.Distance(currentPosition, point.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = point;
                }
            }

            if (closestPoint != null)
            {
                enemy.Agent.SetDestination(closestPoint.position);
            }
        }
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