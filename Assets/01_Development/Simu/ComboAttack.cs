using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboAttack", menuName = "Player/ComboAttack", order = 0)]
public class ComboAttack : ScriptableObject
{
    public int damage;
    public float knockBack;
    public ComboAttack nextBasicAttack;
    public ComboAttack nextFinisherAttack;
    public float moveDistance;
    public float moveDuration;
    [HideInInspector] public float moveStartTime;
    [HideInInspector] public float moveEndTime;
    public AnimationCurve moveSpeedCurve;
    public string animationName;
}
