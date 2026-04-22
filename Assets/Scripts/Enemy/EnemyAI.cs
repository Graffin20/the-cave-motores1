using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum EnemyState { Waiting, AtWindow, Fleeing }

public class EnemyAI : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private EnemyState currentState = EnemyState.Waiting;
    [SerializeField] private float timeToShoot = 2.0f;
    [SerializeField] private float respawnCooldown = 10f;

    [Header("References")]
    [SerializeField] private Transform[] windowPoints;
    [SerializeField] private Transform escapePoint;

    private float eventTimer = 0f;
    private float cooldownTimer = 0f;
    private NavMeshAgent agent;
    private bool isPlayerInside = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Waiting:
                if (isPlayerInside && cooldownTimer > 0)
                {
                    cooldownTimer -= Time.deltaTime;
                    if (cooldownTimer <= 0)
                    {
                        RandomSpawns();
                    }
                }
                break;

            case EnemyState.AtWindow:
                eventTimer += Time.deltaTime;
                transform.LookAt(Camera.main.transform);

                if (eventTimer >= timeToShoot)
                {
                    GameOver();
                }
                break;

            case EnemyState.Fleeing:
                if (!agent.pathPending && agent.remainingDistance < 1.5f)
                {
                    HideEnemy();
                }
                break;
        }
    }

    public void StartSpawns()
    {
        if (!isPlayerInside)
        {
            isPlayerInside = true;
            RandomSpawns();
        }
    }

    private void RandomSpawns()
    {
        if (windowPoints.Length == 0) return;

        int randomIndex = Random.Range(0, windowPoints.Length);
        Transform targetWindow = windowPoints[randomIndex];

        eventTimer = 0f;
        currentState = EnemyState.AtWindow;

        transform.position = targetWindow.position;
        agent.enabled = true;
    }

    public void TakeHit()
    {
        if (currentState == EnemyState.AtWindow)
        {
            currentState = EnemyState.Fleeing;
            agent.SetDestination(escapePoint.position);
        }
    }

    private void HideEnemy()
    {
        currentState = EnemyState.Waiting;
        agent.enabled = false;
        transform.position = new Vector3(0, -100, 0);
        cooldownTimer = respawnCooldown;
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}