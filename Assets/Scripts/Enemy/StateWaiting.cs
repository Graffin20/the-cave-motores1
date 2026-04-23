using UnityEngine;

public class StateWaiting : EnemyState
{
    private float timer;

    public override void Enter(EnemyAI enemy)
    {
        if (enemy.anim != null)
        {
            enemy.anim.SetTrigger("Idle");
        }

        if (enemy.firstspawn)
        {
            timer = enemy.stats.respawnCooldown;
        }
        else
        {
            timer = enemy.stats.Firstspawntime;
        }

        enemy.Agent.enabled = false;
        enemy.transform.position = new Vector3(0, -100, 0);
    }

    public override EnemyState Update(EnemyAI enemy)
    {
        if (!enemy.isPlayerInside)
        {
            return null;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            enemy.firstspawn = true;
            return enemy.GetState(StateID.AtWindow);
        }

        return null;
    }

    public override void Exit(EnemyAI enemy)
    {
    }
}