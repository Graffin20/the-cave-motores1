using UnityEngine;

public class StateAtWindow : EnemyState
{
    private float timer;

    public override void Enter(EnemyAI enemy)
    {
        if (enemy.anim != null)
        {
            enemy.anim.SetTrigger("Idle");
        }
        timer = 0f;
        enemy.gotShot = false;

        if (enemy.windowPoints.Length > 0)
        {
            Transform targetWindow = enemy.windowPoints[Random.Range(0, enemy.windowPoints.Length)];
            enemy.transform.position = targetWindow.position;

            if (Camera.main != null)
                enemy.transform.LookAt(Camera.main.transform);
        }

        enemy.Agent.enabled = true;
    }

    public override EnemyState Update(EnemyAI enemy)
    {
        if (enemy.gotShot)
        {
            return enemy.GetState(StateID.Fleeing);
        }

        timer += Time.deltaTime;

        if (Camera.main != null)
            enemy.transform.LookAt(Camera.main.transform);

        if (timer >= enemy.stats.timeToShoot)
        {
            Debug.Log("perdiste chango");
            return enemy.GetState(StateID.Waiting);
        }

        return null;
    }
}