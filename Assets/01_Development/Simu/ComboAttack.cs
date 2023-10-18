using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAttack", menuName = "Player/ComboAttack", order = 0)]
public class ComboAttack : ScriptableObject
{
    public float damage;
    public float knockBack;
    public List<ComboAttack> possibleComboAttacks;
    public float movementDistance;
    public float movementDuration;
    public AnimationCurve movementSpeedCurve;
    public string animationName;
}
