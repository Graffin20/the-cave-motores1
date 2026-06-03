using UnityEngine;

public class AnimatorSolution : MonoBehaviour
{
    public EnemyAI enemyAI;

    public void StartRoar()
    {
        if (enemyAI != null)
        {
            enemyAI.StartRoar();
        }
    }

    public void StopRoar()
    {
        if (enemyAI != null)
        {
            enemyAI.StopRoar();
        }
    }
}