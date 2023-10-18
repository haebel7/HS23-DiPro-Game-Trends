using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float movespeed;

    //TODO: playerstatus with priority hierarchy/state machine
    private bool canWalk = true;
    private bool canDash = true;
    private bool isDashing = false;

    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
                     private float dashStartTime;
                     private float dashEndTime;
    [SerializeField] private float dashCooldown;
                     private Vector3 dashDirection;
    [SerializeField] private AnimationCurve dashSpeedCurve;

    private CharacterController characterController;
    [HideInInspector]
    public PlayerControls playerControls;
    private InputAction move;
    private InputAction mousePosition;
    private InputAction dodge;

    private Vector2 mouseCoordinates;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        playerControls = new PlayerControls();
        move = playerControls.Gameplay.Move;
        mousePosition = playerControls.Gameplay.MousePosition;
        dodge = playerControls.Gameplay.Dodge;
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
                float rotationToLook = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, rotationToLook, 0f);
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

    private void TriggerDash(InputAction.CallbackContext context)
    {
        Debug.Log("DASH");
        if (canDash)
        {
            canWalk = false;
            canDash = false;
            isDashing = true;
            Vector2 inputDirection = move.ReadValue<Vector2>();
            dashDirection = GetPerspectiveDirection(inputDirection);
            if (inputDirection.Equals(Vector2.zero))
            {
                LookAtCursor();
                dashDirection = transform.forward;
            } else
            {
                dashDirection = GetPerspectiveDirection(inputDirection);
            }
            dashStartTime = Time.time;
            dashEndTime = dashStartTime + dashDuration;
        }
    }

    private void PerformDash()
    {
        if (isDashing)
        {
            //lerp from 0 to 1
            float progress = Mathf.Clamp01((Time.time - dashStartTime) / dashDuration);
            float easedProgress = dashSpeedCurve.Evaluate(progress);
            float dashSpeed = dashDistance / dashDuration;
            float currentSpeed = dashSpeed * easedProgress;
            characterController.Move(dashDirection * currentSpeed * Time.deltaTime);

            if (dashEndTime < Time.time)
            {
                isDashing = false;
                canWalk = true;
            }
        }
        if (dashStartTime + dashCooldown < Time.time)
        {
                canDash = true;
        }
    }

    private Vector3 GetPerspectiveDirection(Vector2 inputDirection)
    {
        float cameraAdjustedInputDirection = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        return Quaternion.Euler(0, cameraAdjustedInputDirection, 0) * Vector3.forward;
    }
}