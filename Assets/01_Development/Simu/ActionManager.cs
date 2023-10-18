using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerAttack))]
public class ActionManager : MonoBehaviour
{

    [SerializeField] private ComboAttack firstComboAttack;
    [SerializeField] private ComboAttack firstComboFinisher;

    private PlayerControls playerControls;
    private InputAction mousePosition;
    private InputAction attack1;
    private InputAction attack2;

    private BUFFERED_MOVE bufferedNextMove = BUFFERED_MOVE.NONE;

    private PlayerAttack playerAttackScript;
    private bool canAttack = true;
    private ComboAttack nextComboBasic;
    private ComboAttack nextComboFinisher;
    private ComboAttack currentAttack;

    //private CHARACTER_STATE currentState = CHARACTER_STATE.MOVING;
    //private Dictionary<string, Action> stateSwitchBehaviour;

    private void Start()
    {
        playerAttackScript = GetComponent<PlayerAttack>();
        nextComboBasic = firstComboAttack;
        nextComboFinisher = firstComboFinisher;

        //stateSwitchBehaviour = new Dictionary<string, Action>
        //{
        //    //{ "" + CharacterState.IDLE + CharacterState.ATTACKING, ACTIVEtoKNOCKBACKED }
        //};
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
        mousePosition = playerControls.Gameplay.MousePosition;
        attack1 = playerControls.Gameplay.Attack;
        attack2 = playerControls.Gameplay.Attack2;

        attack1.performed += BasicAttack;
        attack2.performed += FinisherAttack;
    }

    private void OnEnable()
    {
        playerControls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerControls.Gameplay.Disable();
    }

    private void BasicAttack(InputAction.CallbackContext context)
    {
        if (canAttack)
        {
            currentAttack = nextComboBasic;
            //nextComboBasic = currentAttack.nextBasicAttack;
            //nextComboFinisher = currentAttack.nextFinisherAttack;
            playerAttackScript.TriggerAttack(currentAttack, mousePosition.ReadValue<Vector2>());
        }
        else
        {
            bufferedNextMove = BUFFERED_MOVE.BASIC_ATTACK;
        }
    }

    private void FinisherAttack(InputAction.CallbackContext context)
    {
        if (canAttack)
        {
            currentAttack = nextComboFinisher;
            nextComboBasic = currentAttack.nextBasicAttack;
            nextComboFinisher = currentAttack.nextFinisherAttack;
            playerAttackScript.TriggerAttack(currentAttack, mousePosition.ReadValue<Vector2>());
        }
        else
        {
            bufferedNextMove = BUFFERED_MOVE.FINISHER_ATTACK;
        }
    }

    //public void goToState(CHARACTER_STATE nextState)
    //{
    //    Debug.Log(currentState);
    //    Debug.Log(nextState);
    //    Debug.Log(stateSwitchBehaviour["" + currentState + nextState]);
    //    stateSwitchBehaviour["" + currentState + nextState]();
    //}

    //private void ACTIVEtoKNOCKBACKED()
    //{
    //    canMove = false;
    //    canDash = false;
    //    canAttack = false;
    //    cancel any ongoing attacks, dashes, momentum.
    //}
}