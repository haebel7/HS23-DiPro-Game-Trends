using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class PlayerLimitedRoot : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private Transform playerRoot;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        // TODO: deactivate agent when not in use
        Collider[] hitColliders = Physics.OverlapSphere(playerRoot.position, radius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.tag == "Wall")
            {
                agent.destination = playerRoot.position;
                return;
            }
            else
            {
                if(agent.destination != null)
                {
                    agent.ResetPath();
                }
            }
        }

        transform.position = playerRoot.position;
    }
}
