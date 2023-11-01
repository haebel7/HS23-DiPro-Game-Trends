using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Collider))]
public class HurtBox : MonoBehaviour
{
    [SerializeField]
    private HealthObject health;
    public GameEvent damageTakenEvent;

    public List<DamageType> canBeHitBy;

    [SerializeField]
    private Transform dmgNumberpf;

    private HealthObject ownHealth;

    // Start is called before the first frame update
    void Start()
    {
        ownHealth = Instantiate(health);
    }

    public void TakeDamage(int damageAmount, DamageType damageType)
    {
        //Debug.Log(canBeHitBy[0]);
        //Debug.Log(damageType);
        if (canBeHitBy.Contains(damageType))
        {
            if(damageTakenEvent != null)
            {
                damageTakenEvent.Raise();
            }
            DamagePopup.Create(dmgNumberpf, gameObject.transform.position, damageAmount, damageType);
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

    public HealthObject GetOwnHealth()
    {
        return ownHealth;
    }
}
