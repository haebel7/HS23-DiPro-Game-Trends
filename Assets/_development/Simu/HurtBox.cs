using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Collider))]
public class HurtBox : MonoBehaviour
{
    public HealthObject health;

    public void TakeDamage(int damageAmount)
    {
        health.currentHealth -= damageAmount;
        CheckDied();
    }

    public void CheckDied()
    {
        if (health.currentHealth < 0)
        {
            Debug.Log("IM DEAD!?");
        }
    }
}
