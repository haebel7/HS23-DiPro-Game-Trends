using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerAttack))]
public class ActionManager : MonoBehaviour
{
    //RoomExit.cs needs this getter, might have to create an enable/disable function or events
    public PlayerControls playerControls { get; private set; }
    private InputAction moveDirection;
    private InputAction mousePosition;
    private InputAction dodge;
    private InputAction attack1;
    private InputAction attack2;

    private CHARACTER_STATE currentState = CHARACTER_STATE.MOVING;
    private BUFFERED_MOVE bufferedNextMove = BUFFERED_MOVE.NONE;
    private Dictionary<string, Action> stateSwitchBehaviour;
    private bool canMove = true;
    private bool canDash = true;
    private bool canAttack = true;
    private bool canBeKnockedBack = true;

    [SerializeField] private float dashCooldown;
    private float dashEndetAtTime;
    [SerializeField] private float knockbackImmunityTime;
    private float gotOutOfKnockedBackAtTime;
    [SerializeField] private float comboWindowTime;
    private float stoppedAttackingAtTime;

    [SerializeField] private ComboAttack firstComboAttack;
    [SerializeField] private ComboAttack firstComboFinisher;
    [SerializeField] private ComboAttack dashAttack;
    [SerializeField] private ComboAttack dashFinisher;

    private Movement playerMovementScript;
    private PlayerAttack playerAttackScript;
    private ComboAttack nextComboBasic;
    private ComboAttack nextComboFinisher;
    private ComboAttack currentAttack;


    private void Start()
    {
        playerMovementScript = GetComponent<Movement>();
        playerAttackScript = GetComponent<PlayerAttack>();
        ResetAttackCombo();

        stateSwitchBehaviour = new Dictionary<string, Action>
        {
            { "" + CHARACTER_STATE.MOVING, toStateMoving },
            { "" + CHARACTER_STATE.DASHING, toStateDashing },
            { "" + CHARACTER_STATE.ATTACKING, toStateAttacking },
            { "" + CHARACTER_STATE.KNOCKED_BACK, toStateKnockback }
        };
    }

    private void Update()
    {
        Walk();
    }

    private void Awake()
    {
        playerControls = new PlayerControls();

        moveDirection = playerControls.Gameplay.Move;
        mousePosition = playerControls.Gameplay.MousePosition;
        dodge = playerControls.Gameplay.Dodge;
        attack1 = playerControls.Gameplay.Attack;
        attack2 = playerControls.Gameplay.Attack2;

        dodge.performed += Dash;
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

    private void Walk()
    {
        if (canMove)
        {
            playerMovementScript.Walk(moveDirection.ReadValue<Vector2>());
        }
    }
    private void Dash(InputAction.CallbackContext context)
    {
        if (canDash && dashEndetAtTime + dashCooldown < Time.time)
        {
            goToState(CHARACTER_STATE.DASHING);
            playerMovementScript.TriggerDash(moveDirection.ReadValue<Vector2>(), mousePosition.ReadValue<Vector2>());
        }
        else 
        {
            bufferedNextMove = BUFFERED_MOVE.DASH;
        }
    }
    private void BasicAttack(InputAction.CallbackContext context)
    {
        if (canAttack)
        {
            if (stoppedAttackingAtTime + comboWindowTime < Time.time)
            {
                ResetAttackCombo();
            }
            goToState(CHARACTER_STATE.ATTACKING);
            currentAttack = nextComboBasic;
            nextComboBasic = currentAttack.nextBasicAttack;
            nextComboFinisher = currentAttack.nextFinisherAttack;
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
            goToState(CHARACTER_STATE.DASHING);
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
    public void TriggerKnockedBack()
    {
        if (canBeKnockedBack && gotOutOfKnockedBackAtTime + knockbackImmunityTime < Time.time)
        {
            goToState(CHARACTER_STATE.KNOCKED_BACK);
            playerMovementScript.TriggerKnockback();
        }
    }

    private void ResetAttackCombo()
    {
        nextComboBasic = firstComboAttack;
        nextComboFinisher = firstComboFinisher;
    }

    private void goToState(CHARACTER_STATE nextState)
    {
        stateSwitchBehaviour[""+nextState]();
    }
    public void AttackOver()
    {
        goToState(CHARACTER_STATE.MOVING);
    }
    public void DashOver()
    {
        dashEndetAtTime = Time.time;
        goToState(CHARACTER_STATE.MOVING);
    }
    public void KnockbackOver()
    {
        gotOutOfKnockedBackAtTime = Time.time;
        goToState(CHARACTER_STATE.MOVING);
    }

    private void toStateMoving()
    {
        currentState = CHARACTER_STATE.MOVING;

        canMove = true;
        canDash = true;
        canAttack = true;
        canBeKnockedBack = true;

        stoppedAttackingAtTime = Time.time;

        triggerBufferedMove();
    }
    private void toStateDashing()
    {
        currentState = CHARACTER_STATE.DASHING;

        canMove = false;
        canDash = false;
        canAttack = false;
        canBeKnockedBack = true;

        nextComboBasic = dashAttack;
        nextComboFinisher = dashFinisher;
    }
    private void toStateAttacking()
    {
        currentState = CHARACTER_STATE.ATTACKING;

        canMove = false;
        canDash = false;
        canAttack = false;
        canBeKnockedBack = true;
    }
    private void toStateKnockback()
    {
        currentState = CHARACTER_STATE.KNOCKED_BACK;

        canMove = false;
        canDash = false;
        canAttack = false;
        canBeKnockedBack = false;

        playerMovementScript.InterruptDash();
        playerAttackScript.InterruptAttack();

        ResetAttackCombo();
    }
    private void triggerBufferedMove()
    {
        switch (bufferedNextMove)
        {
            case BUFFERED_MOVE.NONE:
                break;
            case BUFFERED_MOVE.BASIC_ATTACK:
                BasicAttack(new InputAction.CallbackContext());
                break;
            case BUFFERED_MOVE.FINISHER_ATTACK:
                FinisherAttack(new InputAction.CallbackContext());
                break;
            case BUFFERED_MOVE.DASH:
                Dash(new InputAction.CallbackContext());
                break;
        }
        bufferedNextMove = BUFFERED_MOVE.NONE;
    }
}