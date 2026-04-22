using UnityEngine;
public abstract class EnemyState
{
    public abstract void Enter(EnemyAI enemy);

    public abstract EnemyState Update(EnemyAI enemy);
}