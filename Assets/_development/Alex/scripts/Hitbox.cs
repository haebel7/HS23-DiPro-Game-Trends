using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Player hit enemy
        if (gameObject.GetComponent<Movement>() && other.gameObject.GetComponent<EnemyMelee>())
        {

        }
        // Enemy hit player
        else if (transform.parent.GetComponent<EnemyMelee>() && other.gameObject.GetComponent<Movement>())
        {
            Debug.Log("Player hit!");
        }
    }
}
