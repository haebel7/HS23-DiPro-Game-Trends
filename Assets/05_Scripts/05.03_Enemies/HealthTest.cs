using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTest : MonoBehaviour
{
    public List<DamageType> canBeHitBy;
    public HealthObject health;

    private HealthObject ownHealth;

    // Start is called before the first frame update
    void Start()
    {
        //health = ScriptableObject.CreateInstance<HealthObject>();
        ownHealth = Instantiate(health);
    }

    public void TakeDamage(int damageAmount, DamageType damageType)
    { 
        if (canBeHitBy.Contains(damageType))
        {
            //Debug.Log("Its got a little kick, " + gameObject.name + ", " + ownHealth.currentHealth);
            ownHealth.currentHealth -= damageAmount;
            CheckDied();
        }
    }

    public void CheckDied()
    {
        if (ownHealth.currentHealth < 0)
        {
            Debug.Log("IM DEAD!?");
        }
    }
}
