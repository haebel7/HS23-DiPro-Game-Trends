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
        // When in attack range, aim at player
        isLookingAtPlayer = isInAttackDistance;

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
        // Ranged specific state changes
        if (isInAttackDistance)
        {
            if (Time.fixedTime > timeLastShot + shotCooldown)
            {
                state = EnemyState["Attack"];
            }
            else
            {
                state = EnemyState["Idle"];
            }
        }
    }

    public void ShootProjectile()
    {
        timeLastShot = Time.fixedTime;

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + transform.localScale.y, transform.position.z);
        GameObject proj = Instantiate(projectile, spawnPos, transform.rotation);
        proj.GetComponent<Hitbox>().damageStat = attackDmg;
    }
}