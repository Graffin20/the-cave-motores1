using UnityEngine;
using UnityEngine.AI; 

public class EnemyAI : MonoBehaviour
{
    
    public Transform target; 

    
    public float health = 100f;

    private NavMeshAgent enemy;

    void Start()
    {
       
        enemy = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
       
        if (target != null)
        {
            enemy.SetDestination(target.position);
        }
    }

   
}