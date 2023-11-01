using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthObject", menuName = "Stats/HealthObject", order = 0)]
public class HealthObject : ScriptableObject
{
    public int currentHealth;
    public int maxHealth;
    //maybe health color one day
}
