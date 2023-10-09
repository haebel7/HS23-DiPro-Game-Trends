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
    private bool bPlayerInRange = false;
    private bool bLowHealth = false;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemySState.HUNT;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (state == EnemySState.HUNT && Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            state = EnemySState.ATTACK;
            anim.SetTrigger("Attacking");
            GetComponent<PlanarMovement>().SetIsMoving(false);
        }
        else if (state == EnemySState.ATTACK && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "EnemySAttack")
        {
            state = EnemySState.HUNT;
            GetComponent<PlanarMovement>().SetIsMoving(true);
        }
    }

    private void checkEnemyState()
    {

    }
}
