using UnityEngine;
using UnityEngine.AI;
public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float range;
    public float extraRange;

    public float updateInterval = 0.2f;
    private float timer = 0f;

    private NavMeshAgent enemy;
    private bool tracking = false;
    private float distance;

    void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target == null) return;

        distance = Vector3.Distance(transform.position, target.position);

       
        if (distance <= range && !tracking)
        {
            tracking = true;
        }
        else if (distance > range + extraRange && tracking)
        {
            tracking = false;
            enemy.ResetPath();
        }

        if (tracking)
        {
            timer += Time.deltaTime;

            if (timer >= updateInterval)
            {
                enemy.SetDestination(target.position);
                timer = 0f;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range + extraRange);
    }
}