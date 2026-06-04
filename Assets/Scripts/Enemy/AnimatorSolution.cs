using UnityEngine;

public class AnimatorSolution : MonoBehaviour
{
    public EnemyAI enemyAI;
    public void StartRoar()
    {
        if (enemyAI != null) enemyAI.StartRoar();
    }
    public void StopRoar()
    {
        if (enemyAI != null) enemyAI.StopRoar();
    }
    public void PlayWalkSound()
    {
        if (enemyAI != null) enemyAI.PlayWalkSound();
    }
    public void PlayAttackSound()
    {
        if (enemyAI != null) enemyAI.PlayAttackSound();
    }
    public void PlayDeathSound()
    {
        if (enemyAI != null) enemyAI.PlayDeathSound();
    }

    public void DealDamage()
    {
        if (enemyAI != null) enemyAI.DealDamage();
    }

}