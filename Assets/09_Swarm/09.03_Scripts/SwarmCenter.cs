using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmCenter : MonoBehaviour
{
    public bool spawnPointIsCenter;

    private void Start()
    {
        if (spawnPointIsCenter)
        {
            Debug.Log(name + ": parent position: "+transform.parent.position);
            Debug.Log(name + ": position: " + transform.position);
            transform.position += transform.parent.position;
            Debug.Log(name + ": position: " + transform.position);
        }
    }
}
