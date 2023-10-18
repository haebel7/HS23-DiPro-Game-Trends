using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public bool canMove { get; private set; }
    public bool canDash { get; private set; }
    public bool canAttack { get; private set; }
    //triggers for buffer

    private CharacterState currentState = CharacterState.IDLE;
    private Dictionary<string, Action> stateSwitchBehaviour;
    private CharacterState bufferedNextState = CharacterState.IDLE;


    private void Start()
    {
        stateSwitchBehaviour.Add("" + CharacterState.IDLE + CharacterState.KNOCKED_BACK, ACTIVEtoKNOCKBACKED);
    }

    public void goToState(CharacterState nextState)
    {
        if (nextState == CharacterState.KNOCKED_BACK)
        {
            ACTIVEtoKNOCKBACKED();
        }
        else
        {
            stateSwitchBehaviour["" + currentState + nextState]();
        }
        if (currentState == CharacterState.KNOCKED_BACK)
        {
            bufferedNextState = nextState;
        }
    }



    private void ACTIVEtoKNOCKBACKED()
    {
        canMove = false;
        canDash = false;
        canAttack = false;
        //cancel any ongoing attacks, dashes, momentum.
    }
}