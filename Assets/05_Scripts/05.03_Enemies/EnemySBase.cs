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

public class EnemySBase : MonoBehaviour
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
    [SerializeField]
    private int dodgeChance;
    [SerializeField]
    private float dodgeSpeed;

    private Animator anim;
    //private HurtBox hurtBox;

    private EnemySState lastState;
    private float stateInterval = 2f;
    private float lastStateInterval = 0;
    private Vector3 dodgeDirection;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemySState.IDLE;
        lastState = state;
        anim = GetComponent<Animator>();
        //hurtBox = GetComponent<HurtBox>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }



    protected void ChangeEnemyState()
    {
        // Change states
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            state = EnemySState.ATTACK;
        }
        else if (state == EnemySState.HUNT && Time.fixedTime > lastStateInterval + stateInterval)
        {
            if (Random.Range(1, 100) < dodgeChance)
            {
                state = EnemySState.DODGE;
            }
            else if (Random.Range(1, 100) < idleChance)
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

        // Dodge for as long as in dodge state
        if (state == EnemySState.DODGE)
        {
            GetComponent<CharacterController>().Move(dodgeDirection * dodgeSpeed);
        }
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
                    anim.SetBool("Dodge", false);
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
                    anim.SetBool("Dodge", true);
                    int dodgeAngle = Random.Range(0, 360);
                    dodgeDirection = Quaternion.AngleAxis(dodgeAngle, Vector3.up) * Vector3.forward;
                    break;
                default:
                    break;
            }
        }

        lastState = state;
    }

    public void LeaveAttackState()
    {
        state = EnemySState.HUNT;
        CheckEnemyState();
    }

    public void LeaveDodgeState()
    {
        state = EnemySState.HUNT;
        CheckEnemyState();
    }
}
