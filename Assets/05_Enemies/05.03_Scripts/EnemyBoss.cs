using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemySBase
{
    [SerializeField]
    private int summonChance;
    [SerializeField]
    private GameObject summonEnemy;
    [SerializeField]
    private int chargeChance;
    [SerializeField]
    private float chargeSpeed = 10;
    [SerializeField]
    private LayerMask ignoreCharactersLayer;
    [SerializeField]
    private LayerMask characterLayer;

    private bool isCharging = false;

    private static Dictionary<string, int> EnemyB1State = new Dictionary<string, int>()
    {
        {"Charge", EnemyState.Count + 0 },
        {"Summon", EnemyState.Count + 1 },
        {"CollidedWhileCharging", EnemyState.Count + 2 },
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

        // Move forward during charge attack
        if (isCharging)
        {
            GetComponent<CharacterController>().Move(transform.forward * chargeSpeed);
        }
    }

    public override void ChangeEnemyStateAdditional()
    {
        // Boss specific states
        if (state != EnemyB1State["Charge"] 
            && state != EnemyB1State["CollidedWhileCharging"] 
            && Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            state = EnemyState["Attack"];
        }
        else if (state == EnemyState["Hunt"] && Time.fixedTime > lastStateInterval + stateInterval)
        {
            if (enemyList.enemies.Count <= 1 && Random.Range(1, 100) < summonChance)
            {
                state = EnemyB1State["Summon"];
            }
            else if (Random.Range(1, 100) < chargeChance)
            {
                state = EnemyB1State["Charge"];
            }
            lastStateInterval = Time.fixedTime;
        }
    }

    public override void CheckEnemyStateAdditional()
    {
        if (lastState != state)
        {
            // Cleanup last state behaviour
            if (lastState == EnemyB1State["Charge"])
            {
                anim.SetBool("Charge", false);
                isLookingAtPlayer = false;
                gameObject.layer = (int) Mathf.Log(characterLayer.value, 2);
                agent.radius = 0.5f;
            }
            else if (lastState == EnemyB1State["Summon"])
            {
                anim.SetBool("Summon", false);
            }
            else if (lastState == EnemyB1State["CollidedWhileCharging"])
            {
                anim.SetBool("CollidedWhileCharging", false);
            }

            // Activate new state behaviour
            if (state == EnemyB1State["Charge"])
            {
                anim.SetBool("Charge", true);
                isLookingAtPlayer = true;
                gameObject.layer = (int) Mathf.Log(ignoreCharactersLayer.value, 2);
                agent.radius = 0.01f;
            }
            else if (state == EnemyB1State["Summon"])
            {
                anim.SetBool("Summon", true);
            }
            else if (state == EnemyB1State["CollidedWhileCharging"])
            {
                anim.SetBool("CollidedWhileCharging", true);
            }
        }
    }

    public void SummonEnemies()
    {
        Instantiate(summonEnemy, transform.position + Vector3.left, transform.rotation);
        Instantiate(summonEnemy, transform.position + Vector3.forward, transform.rotation);
        Instantiate(summonEnemy, transform.position + Vector3.right, transform.rotation);
        Instantiate(summonEnemy, transform.position + Vector3.back, transform.rotation);
    }

    public void StartCharge()
    {
        isCharging = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isCharging)
        {
            isCharging = false;
            state = EnemyB1State["CollidedWhileCharging"];
        }
    }
}