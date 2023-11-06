using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmCenterManager : MonoBehaviour
{
    public StayInRadiusBehavior stayInRadiusBehavior;

    void Start()
    {
        stayInRadiusBehavior.center = transform;
    }
}
