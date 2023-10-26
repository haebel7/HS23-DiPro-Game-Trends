using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRollAnimationLink : MonoBehaviour
{
    [SerializeField]
    private EnemyRoll parentScript;

    public void ReplaceWithDeadBody()
    {
        parentScript.ReplaceWithDeadBody();
    }

    public void LeaveTemporaryState()
    {
        parentScript.LeaveTemporaryState();
    }

    public void StopLookingAtPlayer()
    {
        parentScript.StopLookingAtPlayer();
    }

    public void StartCharge()
    {
        parentScript.StartCharge();
    }
}
