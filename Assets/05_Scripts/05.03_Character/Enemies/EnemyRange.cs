using System.Collections;
using UnityEngine;

public class EnemyRange : EnemySBase
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float shotCooldown;

    private bool isInAttackDistance = false;
    private float timeLastShot = 0;

    void FixedUpdate()
    {
        isInAttackDistance = Vector3.Distance(transform.position, player.position) < attackDistance;

        if (/*state == EnemySState.DIE*/state == EnemyState["Die"])
        {
            return;
        }

        ChangeEnemyStateAdditional();
        ChangeEnemyState();
        CheckEnemyState();

        // When in attack range, aim at player
        if (isInAttackDistance)
        {
            var targetPosition = player.transform.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
        }
    }

    public override void ChangeEnemyStateAdditional()
    {
        // Ranged specific state changes
        if (isInAttackDistance)
        {
            if (Time.fixedTime > timeLastShot + shotCooldown)
            {
                //state = EnemySState.ATTACK;
                state = EnemyState["Attack"];
            }
            else
            {
                //state = EnemySState.IDLE;
                state = EnemyState["Idle"];
            }
        }
    }

    public void ShootProjectile()
    {
        timeLastShot = Time.fixedTime;

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + transform.localScale.y, transform.position.z);
        Instantiate(projectile, spawnPos, transform.rotation);
    }
}