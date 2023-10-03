using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{

    [SerializeField]
    private float movespeed;
    private CharacterController characterController;

    private PlayerControls playerControls;
    private InputAction move;
    private InputAction mousePosition;
    private InputAction attack1;
    private InputAction attack2;
    private InputAction dodge;

    private Vector2 moveDirection;
    private Vector2 mouseCoordinates;

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
        characterController = GetComponent<CharacterController>();

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
        LookAtCursor();
        Walk();
    }

    private void Walk()
    {
        Vector2 direction = move.ReadValue<Vector2>();

        if (direction.magnitude >= 0.1f)
        {
            float cameraAdjustedInputDirection = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, cameraAdjustedInputDirection, 0) * Vector3.forward;
            characterController.Move(moveDirection * movespeed * Time.deltaTime);
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

    private void Dodge(InputAction.CallbackContext context)
    {
        Debug.Log("dodge");
    }
}
