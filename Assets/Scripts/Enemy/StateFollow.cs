using UnityEngine;

public class StateFollow : EnemyState
{
    private Transform playerTransform;

    private float stunTimer;
    private float spawnDelay;
    private float attackCooldown;

    private bool isSpawning = true;

    public override void Enter(EnemyAI enemy)
    {
        isSpawning = true;
        spawnDelay = enemy.stats.roartime;

        if (enemy.anim != null)
        {
            enemy.anim.SetTrigger("Roar");
        }

        if (enemy.Agent != null)
        {
            enemy.Agent.enabled = true;
            enemy.Agent.speed = 0f;
            enemy.Agent.stoppingDistance = 2.6f;
        }

        enemy.gotShot = false;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public override EnemyState Update(EnemyAI enemy)
    {
        if (isSpawning)
        {
            spawnDelay -= Time.deltaTime;

            if (playerTransform != null)
            {
                Vector3 lookPos = new Vector3(playerTransform.position.x, enemy.transform.position.y, playerTransform.position.z);
                enemy.transform.LookAt(lookPos);
            }

            if (spawnDelay <= 0)
            {
                isSpawning = false;
                enemy.Agent.speed = enemy.stats.FollowSpeed;
                if (enemy.anim != null) enemy.anim.SetTrigger("Walk");
            }
            return null;
        }

        if (enemy.gotShot)
        {
            enemy.gotShot = false;
            stunTimer = enemy.stats.Stuntime;
            enemy.Agent.speed = 0f;

            if (enemy.Agent.isOnNavMesh)
            {
                enemy.Agent.SetDestination(enemy.transform.position);
            }

            if (enemy.anim != null) enemy.anim.SetTrigger("damage");
        }

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                enemy.Agent.speed = enemy.stats.FollowSpeed;
                if (enemy.anim != null) enemy.anim.SetTrigger("Walk");
            }
            return null;
        }

        if (playerTransform != null && enemy.Agent != null && enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.SetDestination(playerTransform.position);

            if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance && !enemy.Agent.pathPending)
            {
                Vector3 lookPos = new Vector3(playerTransform.position.x, enemy.transform.position.y, playerTransform.position.z);
                enemy.transform.LookAt(lookPos);

                attackCooldown -= Time.deltaTime;

                if (attackCooldown <= 0)
                {
                    if (enemy.anim != null)
                    {
                        enemy.anim.SetTrigger("Attack");
                    }

                    attackCooldown = 2f;
                }
            }
            else
            {
                attackCooldown = 0f;
            }
        }

        return null;
    }

    public override void Exit(EnemyAI enemy)
    {
        if (enemy.Agent != null && enemy.Agent.isOnNavMesh)
        {
            enemy.Agent.ResetPath();
            enemy.Agent.stoppingDistance = 0f;
        }
    }
}