using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemySBase
{

    void FixedUpdate()
    {
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