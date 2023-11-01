using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoll : EnemySBase
{
    [SerializeField]
    private float chargeSpeed = 0.1f;
    [SerializeField]
    private float chargeCooldown = 5;
    [SerializeField]
    private float chargeMaxTime = 5;

    private bool isCharging = false;
    private float timeChargeCooldownStarted = 0;
    private float timeChargeStarted = 0;

    private static Dictionary<string, int> EnemyAddState = new Dictionary<string, int>()
    {
        {"Charge", EnemyState.Count + 0 },
        {"ChargeEnd", EnemyState.Count + 1 },
    };

    public override void StartAdditional()
    {
        // Roll Enemy requires animator from child object
        anim = GetComponentInChildren<Animator>();

        hitBox.damageStat = attackDmg;
    }

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

        // Move forward during roll
        if (isCharging)
        {
            GetComponent<CharacterController>().Move(transform.forward * chargeSpeed);
        }
    }

    public override void ChangeEnemyStateAdditional()
    {
        // RollingEnemy specific states
        if (state == EnemyState["Hunt"]
            && Vector3.Distance(transform.position, player.position) < attackDistance
            && Time.fixedTime > timeChargeCooldownStarted + chargeCooldown)
        {
            state = EnemyAddState["Charge"];
        }
        else if (state == EnemyAddState["Charge"] && Time.fixedTime > timeChargeStarted + chargeMaxTime)
        {
            state = EnemyAddState["ChargeEnd"];
        }
    }

    public override void CheckEnemyStateAdditional()
    {
        if (lastState != state)
        {
            // Cleanup last state behaviour
            if (lastState == EnemyAddState["Charge"])
            {
                anim.SetBool("Charge", false);
                isLookingAtPlayer = false;
                agent.radius = 1f;
                isCharging = false;
            }
            else if (lastState == EnemyAddState["ChargeEnd"])
            {
                anim.SetBool("ChargeEnd", false);
                timeChargeCooldownStarted = Time.fixedTime;
            }

            // Activate new state behaviour
            if (state == EnemyAddState["Charge"])
            {
                anim.SetBool("Charge", true);
                isLookingAtPlayer = true;
                agent.radius = 0.01f;
                timeChargeStarted = Time.fixedTime;
            }
            else if (state == EnemyAddState["ChargeEnd"])
            {
                anim.SetBool("ChargeEnd", true);
            }
        }
    }

    public void StartCharge()
    {
        isCharging = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isCharging && (hit.transform == player || hit.gameObject.tag != "Agent"))
        {
            state = EnemyAddState["ChargeEnd"];
        }
    }
}