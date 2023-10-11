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
    DODGE,
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
    [SerializeField]
    private int idleChance;
    [SerializeField]
    private int huntChance;

    private Animator anim;
    //private HurtBox hurtBox;

    private EnemySState lastState;
    private float stateInterval = 2f;
    private float lastStateInterval = 0;

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
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            state = EnemySState.ATTACK;
        }
        else if (state == EnemySState.HUNT && Time.fixedTime > lastStateInterval + stateInterval)
        {
            if (Random.Range(1, 100) < idleChance)
            {
                state = EnemySState.IDLE;
            }
            lastStateInterval = Time.fixedTime;
        }
        else if (state == EnemySState.IDLE && Time.fixedTime > lastStateInterval + stateInterval)
        {
            if (Random.Range(1, 100) < huntChance)
            {
                state = EnemySState.HUNT;
            }
            lastStateInterval = Time.fixedTime;
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
                case EnemySState.DODGE:

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
                case EnemySState.DODGE:

                    break;
                default:
                    break;
            }
        }

        lastState = state;
    }
}
