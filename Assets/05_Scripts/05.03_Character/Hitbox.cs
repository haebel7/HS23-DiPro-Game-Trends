using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private DamageType damageType;
    [SerializeField]
    private DamageStat damageStat;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HurtBox>())
        {
            other.GetComponent<HurtBox>().TakeDamage(damageStat.damage, damageType);
        }
    }
}
