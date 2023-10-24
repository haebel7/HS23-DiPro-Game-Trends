using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class EnemySBase : MonoBehaviour
{
    [SerializeField]
    protected EnemyList enemyList;
    [SerializeField]
    protected int state;
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
    [SerializeField]
    private GameObject deadBody;
    [SerializeField]
    private GameObject loot;

    protected static Dictionary<string, int> EnemyState = new Dictionary<string, int>()
    {
        {"Spawn", 0 },
        {"Idle", 1 },
        {"Hunt", 2 },
        {"Attack", 3 },
        {"Dodge", 4 },
        {"Die", 5 },
    };

    protected Transform player;
    protected Animator anim;
    protected HurtBox hurtBox;
    protected NavMeshAgent agent;

    protected int lastState;
    protected float stateInterval = 2f;
    protected float lastStateInterval = 0;
    private Vector3 dodgeDirection;

    private void OnEnable()
    {
        enemyList.enemies.Add(gameObject);
    }

    private void OnDisable()
    {
        enemyList.enemies.Remove(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyState["Spawn"];
        lastState = EnemyState["Hunt"];
        player = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
        hurtBox = GetComponent<HurtBox>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected void ChangeEnemyState()
    {
        // Change states
        if (hurtBox.GetOwnHealth().currentHealth <= 0)
        {
            state = EnemyState["Die"];
        }
        else if (state == EnemyState["Hunt"] && Time.fixedTime > lastStateInterval + stateInterval)
        {
            if (Random.Range(1, 100) < dodgeChance)
            {
                state = EnemyState["Dodge"];
            }
            else if (Random.Range(1, 100) < idleChance)
            {
                state = EnemyState["Idle"];
            }
            lastStateInterval = Time.fixedTime;
        }
        else if (state == EnemyState["Idle"] && Time.fixedTime > lastStateInterval + stateInterval)
        {
            if (Random.Range(1, 100) < huntChance)
            {
                state = EnemyState["Hunt"];
            }
            lastStateInterval = Time.fixedTime;
        }

        // Dodge for as long as in dodge state
        if (state == EnemyState["Dodge"])
        {
            GetComponent<CharacterController>().Move(dodgeDirection * dodgeSpeed);
        }

        // Update navmesh destination to player pos
        if (!agent.isStopped)
        {
            agent.destination = player.position;
        }
    }

    protected void CheckEnemyState()
    {
        if (lastState != state)
        {
            // Cleanup last state behaviour
            if (lastState == EnemyState["Spawn"])
            {

            }
            else if (lastState == EnemyState["Idle"])
            {

            }
            else if (lastState == EnemyState["Hunt"])
            {
                anim.SetBool("Hunt", false);
                agent.isStopped = true;
            }
            else if (lastState == EnemyState["Attack"])
            {
                anim.SetBool("Attack", false);
            }
            else if (lastState == EnemyState["Dodge"])
            {
                anim.SetBool("Dodge", false);
            }
            else if (lastState == EnemyState["Die"])
            {
                anim.SetBool("Die", false);
            }

            // Trigger any behaviours of the new state
            if (state == EnemyState["Spawn"])
            {

            }
            else if (state == EnemyState["Idle"])
            {

            }
            else if (state == EnemyState["Hunt"])
            {
                anim.SetBool("Hunt", true);
                agent.isStopped = false;
            }
            else if (state == EnemyState["Attack"])
            {
                anim.SetBool("Attack", true);
            }
            else if (state == EnemyState["Dodge"])
            {
                anim.SetBool("Dodge", true);
                int dodgeAngle = Random.Range(0, 360);
                dodgeDirection = Quaternion.AngleAxis(dodgeAngle, Vector3.up) * Vector3.forward;
            }
            else if (state == EnemyState["Die"])
            {
                anim.SetBool("Die", true);
            }
        }

        lastState = state;
    }

    public virtual void ChangeEnemyStateAdditional() { }

    public virtual void CheckEnemyStateAdditional() { }

    public void LeaveTemporaryState()
    {
        state = EnemyState["Hunt"];
        CheckEnemyState();
    }

    public void ReplaceWithDeadBody()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + transform.localScale.y, transform.position.z);
        // Drop Loot on Dying Position
        for (int i = 0; i < 3; i++)
        {
            GameObject lootPiece = Instantiate(loot, spawnPos, transform.rotation);
            lootPiece.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
        }
        // Instantiate dead body prefab and destroy this enemy object
        Instantiate(deadBody, spawnPos, transform.rotation);
        Destroy(gameObject);
    }
}