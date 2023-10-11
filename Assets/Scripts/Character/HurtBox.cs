using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Collider))]
public class HurtBox : MonoBehaviour
{
    public HealthObject health;
    public List<DamageType> canBeHitBy;

    public void TakeDamage(int damageAmount, DamageType damageType)
    {
        Debug.Log(canBeHitBy[0]);
        Debug.Log(damageType);
        if (canBeHitBy.Contains(damageType))
        {
            health.currentHealth -= damageAmount;
            CheckDied();
        }
    }

    public void CheckDied()
    {
        if (health.currentHealth < 0)
        {
            Debug.Log("IM DEAD!?");
        }
    }
}
