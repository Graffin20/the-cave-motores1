using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Attack Points")]
    [SerializeField] private Transform[] entryPoints;
    private Transform currentWindow;

    [Header("Speeds")]
    [SerializeField] private float normalSpeed = 3.5f;
    [SerializeField] private float runningSpeed = 8f;

    [Header("Optimization")]
    [SerializeField] private float updateInterval = 0.2f;
    private float timer = 0f;

    [Header("Distances")]
    [SerializeField] private float escapeDistance = 8f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
    }

    public void GoToWindow()
    {
        if (entryPoints == null || entryPoints.Length == 0) return;

        if (currentWindow == null)
        {
            int index = UnityEngine.Random.Range(0, entryPoints.Length);
            currentWindow = entryPoints[index];
            agent.SetDestination(currentWindow.position);
        }
    }

    public void ChasePlayer(Vector3 playerPosition)
    {
        currentWindow = null;

        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            agent.SetDestination(playerPosition);
            timer = 0f;
        }
    }

    public void StopMoving()
    {
        if (agent.hasPath)
        {
            agent.ResetPath();
        }
    }

    public void EscapeFrom(Vector3 threat)
    {
        agent.speed = runningSpeed;

        Vector3 escapeDirection = transform.position - threat;
        Vector3 destinationPoint = transform.position + (escapeDirection.normalized * escapeDistance);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(destinationPoint, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            agent.SetDestination(destinationPoint);
        }
    }

    public void RestoreSpeed()
    {
        agent.speed = normalSpeed;
    }

    public bool ReachedDestination()
    {
        if (!agent.pathPending && agent.hasPath && agent.remainingDistance <= agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }
}