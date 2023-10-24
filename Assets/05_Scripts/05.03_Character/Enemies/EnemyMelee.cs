using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemySBase
{
    void FixedUpdate()
    {
        if (state == EnemyState["Die"])
        {
            return;
        }

        ChangeEnemyStateAdditional();
        ChangeEnemyState();
        CheckEnemyState();
    }

    public override void ChangeEnemyStateAdditional()
    {
        // Melee specific states
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            state = EnemyState["Attack"];
        }
    }
}