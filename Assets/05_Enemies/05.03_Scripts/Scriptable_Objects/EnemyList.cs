using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyList", menuName = "EnemyList")]
public class EnemyList : ScriptableObject
{
    public List<GameObject> enemies = new List<GameObject>();
}