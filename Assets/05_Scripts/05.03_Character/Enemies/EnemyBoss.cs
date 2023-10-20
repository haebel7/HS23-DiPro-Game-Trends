using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class EnemyBoss : EnemySBase
{
    [SerializeField]
    private int summonChance;
    [SerializeField]
    private GameObject summonEnemy;

    private static Dictionary<string, int> EnemyB1State = new Dictionary<string, int>()
    {
        {"Charge", EnemyState.Count + 0 },
        {"Summon", EnemyState.Count + 1 },
    };

    void FixedUpdate()
    {
        if (state == EnemyState["Die"])
        {
            return;
        }

        ChangeEnemyStateAdditional();
        ChangeEnemyState();
        CheckEnemyStateAdditional();
        CheckEnemyState();
    }

    public override void ChangeEnemyStateAdditional()
    {
        // Boss specific states
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            state = EnemyState["Attack"];
        }
        else if (state == EnemyState["Hunt"] && Time.fixedTime > lastStateInterval + stateInterval)
        {
            if (enemyList.enemies.Count <= 1 && Random.Range(1, 100) < summonChance)
            {
                state = EnemyB1State["Summon"];
            }
            lastStateInterval = Time.fixedTime;
        }
    }

    public override void CheckEnemyStateAdditional()
    {
        // Cleanup last state behaviour
        if (lastState == EnemyB1State["Charge"])
        {

        }
        else if (lastState == EnemyB1State["Summon"])
        {
            anim.SetBool("Summon", false);
        }

        // Activate new state behaviour
        if (state == EnemyB1State["Charge"])
        {

        }
        else if (state == EnemyB1State["Summon"])
        {
            anim.SetBool("Summon", true);
        }
    }

    public void SummonEnemies()
    {
        Instantiate(summonEnemy, transform.position + Vector3.left, transform.rotation);
        Instantiate(summonEnemy, transform.position + Vector3.forward, transform.rotation);
        Instantiate(summonEnemy, transform.position + Vector3.right, transform.rotation);
        Instantiate(summonEnemy, transform.position + Vector3.back, transform.rotation);
    }
}