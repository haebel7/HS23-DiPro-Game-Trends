using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
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
    protected Transform player;
    [SerializeField]
    protected EnemySState state;
    [SerializeField]
    protected float attackDistance = 2f;
    [SerializeField]
    protected int idleChance;
    [SerializeField]
    protected int huntChance;
    [SerializeField]
    protected int dodgeChance;
    [SerializeField]
    protected float dodgeSpeed;

    private Animator anim;
    //private HurtBox hurtBox;
    private HealthTest healthTest;
    private NavMeshAgent agent;

    private EnemySState lastState;
    private float stateInterval = 2f;
    private float lastStateInterval = 0;
    private Vector3 dodgeDirection;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemySState.IDLE;
        lastState = EnemySState.HUNT;
        anim = GetComponent<Animator>();
        //hurtBox = GetComponent<HurtBox>();
        healthTest = GetComponent<HealthTest>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected void ChangeEnemyState()
    {
        // Change states
        /*if (healthTest.health.currentHealth <= 0)
        {

        }
        else */if (state == EnemySState.HUNT && Time.fixedTime > lastStateInterval + stateInterval)
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

        // Update navmesh destination to player pos
        if (!agent.isStopped) {
            agent.destination = player.position;
        }
    }

    protected void CheckEnemyState()
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
                    //GetComponent<PlanarMovement>().SetIsMoving(false);
                    agent.isStopped = true;
                    break;
                case EnemySState.ATTACK:
                    anim.SetBool("Attack", false);
                    break;
                case EnemySState.DODGE:
                    anim.SetBool("Dodge", false);
                    break;
                case EnemySState.DIE:
                    anim.SetBool("Die", false);
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
                    //GetComponent<PlanarMovement>().SetIsMoving(true);
                    agent.isStopped = false;
                    break;
                case EnemySState.ATTACK:
                    anim.SetBool("Attack", true);
                    break;
                case EnemySState.DODGE:
                    anim.SetBool("Dodge", true);
                    int dodgeAngle = Random.Range(0, 360);
                    dodgeDirection = Quaternion.AngleAxis(dodgeAngle, Vector3.up) * Vector3.forward;
                    break;
                case EnemySState.DIE:
                    anim.SetBool("Die", true);
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