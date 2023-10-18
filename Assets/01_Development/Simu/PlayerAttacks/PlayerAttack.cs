using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerAttack : MonoBehaviour
{
    private bool isAttacking;
    private ComboAttack currentAttack;
    private Vector2 mouseCoordinates;

    private Animator animator;
    private Hitbox hitbox;
    private CharacterController characterController;


    private void Start()
    {
        animator = GetComponent<Animator>();
        hitbox = GetComponentInChildren<Hitbox>(true);
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PerformAttackMovement();
    }

    public void TriggerAttack(ComboAttack attackToPerform, Vector2 mouseCoords)
    {
        currentAttack = attackToPerform;
        currentAttack.moveStartTime = Time.time;
        currentAttack.moveEndTime = currentAttack.moveStartTime + currentAttack.moveDuration;
        hitbox.damageStat = currentAttack.damage;
        mouseCoordinates = mouseCoords;
        LookAtCursor();
        animator.Play(currentAttack.animationName);
        isAttacking = true;
    }

    private void PerformAttackMovement()
    {
        if (isAttacking)
        {
            float progress = Mathf.Clamp01((Time.time - currentAttack.moveStartTime) / currentAttack.moveDuration);
            float easedProgress = currentAttack.moveSpeedCurve.Evaluate(progress);
            float moveSpeed = currentAttack.moveDistance / currentAttack.moveDuration;
            float currentSpeed = moveSpeed * easedProgress;
            characterController.Move(transform.forward * currentSpeed * Time.deltaTime);

            if (Time.time >= currentAttack.moveEndTime)
            {
                isAttacking = false;
            }
        }
    }

    private void LookAtCursor()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        float xDifference = mouseCoordinates.x - screenPos.x;
        float yDifference = mouseCoordinates.y - screenPos.y;
        Vector2 posDifference = new Vector2(xDifference, yDifference).normalized;
        float rotationToLookAtCursor = Mathf.Atan2(posDifference.x, posDifference.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, rotationToLookAtCursor, 0f);
    }

    public void InterruptAttack()
    {
        isAttacking = false;
    }
}