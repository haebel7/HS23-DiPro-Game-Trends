using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageSource", menuName = "Stats/DamageSource", order = 0)]
public class DamageSource : ScriptableObject
{
    public string damageSourceName;
    public List<DamageSource> canBeHitByDamageSource;
}
