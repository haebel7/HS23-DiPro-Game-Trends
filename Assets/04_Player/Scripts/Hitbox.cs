using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]    private DamageType damageType;
    [HideInInspector]   public int damageStat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HurtBox>())
        {
            other.GetComponent<HurtBox>().TakeDamage(damageStat, damageType);
        }
    }
}
