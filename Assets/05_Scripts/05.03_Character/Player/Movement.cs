using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    [SerializeField] private float movespeed;

    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
                     private float dashStartTime;
                     public float dashEndTime { get; private set; }
                     private Vector3 dashDirection;
    [SerializeField] private AnimationCurve dashSpeedCurve;
                     private bool isDashing = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PerformDash();
    }

    public void Walk(Vector2 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            float rotationToLook = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, rotationToLook, 0f);
            characterController.Move(GetPerspectiveDirection(direction) * movespeed * Time.deltaTime);
        }
    }

    public void TriggerDash(Vector2 inputDirection, Vector2 mouseCoords)
    {
        isDashing = true;
        dashDirection = GetPerspectiveDirection(inputDirection);
        if (inputDirection.Equals(Vector2.zero))
        {
            LookAtCursor(mouseCoords);
            dashDirection = transform.forward;
        } else
        {
            dashDirection = GetPerspectiveDirection(inputDirection);
        }
        animator.Play("dash");
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
            float dashSpeed = dashDistance / dashDuration;
            float currentSpeed = dashSpeed * easedProgress;
            characterController.Move(dashDirection * currentSpeed * Time.deltaTime);

            if (dashEndTime < Time.time)
            {
                isDashing = false;
            }
        }
    }

    private void LookAtCursor(Vector2 mouseCoordinates)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        float xDifference = mouseCoordinates.x - screenPos.x;
        float yDifference = mouseCoordinates.y - screenPos.y;
        Vector2 posDifference = new Vector2(xDifference, yDifference).normalized;
        float rotationToLookAtCursor = Mathf.Atan2(posDifference.x, posDifference.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, rotationToLookAtCursor, 0f);
    }

    private Vector3 GetPerspectiveDirection(Vector2 inputDirection)
    {
        float cameraAdjustedInputDirection = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        return Quaternion.Euler(0, cameraAdjustedInputDirection, 0) * Vector3.forward;
    }

    public void InterruptDash()
    {
        isDashing = false;
    }

    public void TriggerKnockback()
    {
        Debug.Log("DO A FLIP! (knockbacked)");
    }

    private void PerformKnockback()
    {

    }
}