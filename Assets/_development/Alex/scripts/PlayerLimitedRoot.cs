using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLimitedRoot : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private Transform playerRoot;

    void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        // Cancel following player if a wall is too close
        Collider[] hitColliders = Physics.OverlapSphere(playerRoot.position, radius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.tag == "Wall")
            {
                return;
            }
        }

        transform.position = playerRoot.position;
    }
}
