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

            enemy.Agent.enabled = false;
            enemy.transform.position = targetWindow.position;
            enemy.Agent.enabled = true;

            Debug.Log("El monstruo acaba de aparecer en: " + targetWindow.name);

            if (Camera.main != null)
            {
                Vector3 targetPosition = new Vector3(Camera.main.transform.position.x, enemy.transform.position.y, Camera.main.transform.position.z);
                enemy.transform.LookAt(targetPosition);
            }
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
        {
            Vector3 targetPosition = new Vector3(Camera.main.transform.position.x, enemy.transform.position.y, Camera.main.transform.position.z);
            enemy.transform.LookAt(targetPosition);
        }

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