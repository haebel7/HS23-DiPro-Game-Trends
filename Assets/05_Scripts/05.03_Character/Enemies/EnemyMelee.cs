using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemySBase
{
    void FixedUpdate()
    {
        if (state == EnemySState.DIE)
        {
            return;
        }

        ChangeEnemyState();
        ChangeEnemyStateMelee();
    }

    private void ChangeEnemyStateMelee()
    {
        // Melee specific states
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            state = EnemySState.ATTACK;
        }
    }
}