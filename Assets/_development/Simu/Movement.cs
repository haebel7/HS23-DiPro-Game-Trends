using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{


    /*
        running
        ----------------------
        speedup
        maxspeed
        stopspeed
        slide?
        turnspeed?
        turning around?


        attack and move priority
        0. getting hit
        1. dash
        2. attack
        3. walk

     */



    [SerializeField] private float movespeed;

    //TODO: playerstatus with priority hierarchy/state machine
    private bool canWalk = true;
    private bool isDashing = false;


    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
                     private float dashStartTime;
                     private float dashEndTime;
    [SerializeField] private float dashCooldown;
                     private float elapsedDashCooldown;
                     private Vector3 dashStartPos, dashDestination;
    [SerializeField] private AnimationCurve dashSpeedCurve;

    private CharacterController characterController;
    private PlayerControls playerControls;
    private InputAction move;
    private InputAction mousePosition;
    private InputAction attack1;
    private InputAction attack2;
    private InputAction dodge;

    private Vector2 moveDirection;
    private Vector2 mouseCoordinates;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        playerControls = new PlayerControls();
        move = playerControls.Gameplay.Move;
        mousePosition = playerControls.Gameplay.MousePosition;
        attack1 = playerControls.Gameplay.Attack;
        attack2 = playerControls.Gameplay.Attack2;
        dodge = playerControls.Gameplay.Dodge;

        attack1.performed += Attack1;
        attack2.performed += Attack2;
        dodge.performed += TriggerDash;
    }

    private void OnEnable()
    {
        playerControls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerControls.Gameplay.Disable();
    }

    private void Update()
    {
        LookAtCursor();
        Walk();
        PerformDash();
    }

    private void Walk()
    {
        if (canWalk)
        {
            Vector2 direction = move.ReadValue<Vector2>();

            if (direction.magnitude >= 0.1f)
            {
                characterController.Move(GetPerspectiveDirection(direction) * movespeed * Time.deltaTime);
            }
        }
    }

    private void LookAtCursor()
    {
        mouseCoordinates = mousePosition.ReadValue<Vector2>();
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        float xDifference = mouseCoordinates.x - screenPos.x;
        float yDifference = mouseCoordinates.y - screenPos.y;
        Vector2 posDifference = new Vector2(xDifference, yDifference).normalized;
        float rotationToLookAtCursor = Mathf.Atan2(posDifference.x, posDifference.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, rotationToLookAtCursor, 0f);
    }

    private void Attack1(InputAction.CallbackContext context)
    {
        Debug.Log("attack1");
    }

    private void Attack2(InputAction.CallbackContext context)
    {
        Debug.Log("attack2");
    }

    private void TriggerDash(InputAction.CallbackContext context)
    {
        canWalk = false;
        isDashing = true;
        //dash to cursor?
        Vector2 inputDirection = move.ReadValue<Vector2>();
        Vector3 dashDirection = GetPerspectiveDirection(move.ReadValue<Vector2>());
        dashStartPos = transform.position;
        dashDestination= dashStartPos + dashDirection;
        dashStartTime = Time.time;
        dashEndTime = dashStartTime + dashDuration;
    }

    private void PerformDash()
    {
        if (isDashing)
        {
            //lerp from 0 to 1
            float progress = Mathf.Clamp01((Time.time - dashStartTime) / dashDuration);
            float easedProgress = dashSpeedCurve.Evaluate(progress);
            Vector3 nextPos = Vector3.Lerp(dashStartPos, dashDestination, easedProgress);

            if (dashEndTime < Time.time)
            {
                isDashing = false;
                canWalk = true;
            }
        }
    }

    private Vector3 GetPerspectiveDirection(Vector2 inputDirection)
    {
        float cameraAdjustedInputDirection = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        return Quaternion.Euler(0, cameraAdjustedInputDirection, 0) * Vector3.forward;
    }
}