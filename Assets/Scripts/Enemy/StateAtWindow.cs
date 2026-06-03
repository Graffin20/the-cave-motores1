using UnityEngine;

public class StateAtWindow : EnemyState
{
    private float timer;

    public override void Enter(EnemyAI enemy)
    {
        if (enemy.anim != null)
        {
            enemy.anim.SetTrigger("Roar");
        }

        timer = 0f;
        enemy.gotShot = false;

        if (enemy.windowPoints.Length > 0)
        {
            Transform targetWindow = enemy.windowPoints[Random.Range(0, enemy.windowPoints.Length)];
            enemy.Agent.enabled = false;
            enemy.transform.position = targetWindow.position;
            enemy.Agent.enabled = true;

          

            if (Camera.main != null)
                enemy.transform.LookAt(Camera.main.transform);
        }
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

    public override void Exit(EnemyAI enemy)
    {
        enemy.gotShot = false;
        timer = 0f;
    }
}