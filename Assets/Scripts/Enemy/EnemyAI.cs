using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Look Settings")]
    public float lookRotationSpeed = 5f;

    [Header("Cover")]
    [Tooltip("Height offset from waypoint position used for line-of-sight checks (should match eye/chest height).")]
    public float coverCheckHeight = 1.5f;
    public LayerMask coverObstacleMask = ~0;

    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Moves to the waypoint closest to the given target.
    public void MoveToNearestWaypointTo(Transform target)
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform nearest = null;
        float bestDist = Mathf.Infinity;

        foreach (Transform wp in waypoints)
        {
            if (wp == null) continue;
            float dist = Vector3.Distance(wp.position, target.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                nearest = wp;
            }
        }

        if (nearest != null)
            MoveToPosition(nearest.position);
    }

    // Moves to a specific world position.
    public void MoveToPosition(Vector3 position)
    {
        _agent.isStopped = false;
        _agent.SetDestination(position);
    }

    // Stops all movement.
    public void StandStill()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    // Smoothly rotates to face a target Transform each frame. Call from Update or a coroutine.
    public void LookAtTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookRotationSpeed * Time.deltaTime);
    }

    // Returns true when the agent has reached its destination (within stopping distance).
    public bool HasReachedDestination()
    {
        if (_agent.pathPending) return false;
        return _agent.remainingDistance <= _agent.stoppingDistance;
    }

    // Moves to the closest waypoint from which the player's line of sight is blocked by geometry.
    // Falls back to the closest waypoint overall if no cover is found.
    public void HideBehindCover(Transform player)
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform bestCover = null;
        Transform closestFallback = null;
        float bestCoverDist = Mathf.Infinity;
        float bestFallbackDist = Mathf.Infinity;

        Vector3 playerEyes = player.position + Vector3.up * coverCheckHeight;

        foreach (Transform wp in waypoints)
        {
            if (wp == null) continue;

            float distToEnemy = Vector3.Distance(wp.position, transform.position);

            // Track closest waypoint overall as fallback.
            if (distToEnemy < bestFallbackDist)
            {
                bestFallbackDist = distToEnemy;
                closestFallback = wp;
            }

            // Cast from the waypoint toward the player. If something blocks it, the spot is valid cover.
            Vector3 waypointEyes = wp.position + Vector3.up * coverCheckHeight;
            Vector3 toPlayer = playerEyes - waypointEyes;

            bool blocked = Physics.Raycast(waypointEyes, toPlayer.normalized, toPlayer.magnitude, coverObstacleMask);

            if (blocked && distToEnemy < bestCoverDist)
            {
                bestCoverDist = distToEnemy;
                bestCover = wp;
            }
        }

        Transform destination = bestCover != null ? bestCover : closestFallback;
        if (destination != null)
            MoveToPosition(destination.position);
    }
}
