using Friedforfun.ContextSteering.Demo;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public enum EnemySState
{
    SPAWN,
    IDLE,
    HUNT,
    ATTACK,
    DIE
}

public class EnemyMelee : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private EnemySState state;
    [SerializeField]
    private float attackDistance = 2f;

    // Additional Behaviours
    //private bool bPlayerInRange = false;
    //private bool bLowHealth = false;

    private Animator anim;
    //private HurtBox hurtBox;

    private EnemySState lastState;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemySState.HUNT;
        lastState = state;
        anim = GetComponent<Animator>();
        //hurtBox = GetComponent<HurtBox>();
    }

    void FixedUpdate()
    {
        if (/*state == EnemySState.HUNT &&*/ Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            state = EnemySState.ATTACK;
        }

        CheckEnemyState();
    }

    public void LeaveAttackState()
    {
        state = EnemySState.HUNT;
        CheckEnemyState();
    }

    private void CheckEnemyState()
    {
        if (lastState != state)
        {
            // Cleanup last state behaviour
            switch (lastState)
            {
                case EnemySState.IDLE:
                    // stop idle anim
                    break;
                case EnemySState.HUNT:
                    anim.SetBool("Hunt", false);
                    GetComponent<PlanarMovement>().SetIsMoving(false);
                    break;
                case EnemySState.ATTACK:
                    anim.SetBool("Attack", false);
                    break;
                default:
                    break;
            }

            // Trigger any behaviours of the new state
            switch (state)
            {
                case EnemySState.IDLE:
                    // start idle anim
                    break;
                case EnemySState.HUNT:
                    anim.SetBool("Hunt", true);
                    GetComponent<PlanarMovement>().SetIsMoving(true);
                    break;
                case EnemySState.ATTACK:
                    anim.SetBool("Attack", true);
                    break;
                default:
                    break;
            }
        }

        lastState = state;
    }
}
