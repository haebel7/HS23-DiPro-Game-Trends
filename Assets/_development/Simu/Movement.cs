using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField]
    private float movespeed = 1;

    private PlayerControls playerControls;
    private InputAction move;
    private InputAction mousePosition;
    private InputAction attack1;
    private InputAction attack2;
    private InputAction dodge;

    private Vector2 moveDirection;


    public void EnableMovement()
    {
        //caller?
        Debug.Log("i listened to event :D - ENABLE MOVEMENT");
    }
    public void DisableMovement()
    {
        //caller?
        Debug.Log("i listened to event :D - DISABLE MOVEMENT");
    }

    private void Awake()
    {
        playerControls = new PlayerControls();
        move = playerControls.Gameplay.Move;
        mousePosition = playerControls.Gameplay.MousePosition;
        attack1 = playerControls.Gameplay.Attack;
        attack2 = playerControls.Gameplay.Attack2;
        dodge = playerControls.Gameplay.Dodge;

        attack1.performed += Attack1;
        attack2.performed += Attack2;
        dodge.performed += Dodge;
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
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        GetComponent<Transform>().position += new Vector3(moveDirection.y, 0, moveDirection.x);
    }

    private void Attack1(InputAction.CallbackContext context)
    {
        Debug.Log("attack1");
    }

    private void Attack2(InputAction.CallbackContext context)
    {
        Debug.Log("attack2");
    }

    private void Dodge(InputAction.CallbackContext context)
    {
        Debug.Log("dodge");
    }
}
