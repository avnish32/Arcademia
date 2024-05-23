using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float movementSpeed;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*upAction.action.performed += (ctx) =>
        {
            MoveInDirection(Vector2.up);
        };
        downAction.action.performed += (ctx) =>
        {
            MoveInDirection(Vector2.down);
        };
        leftAction.action.performed += (ctx) =>
        {
            MoveInDirection(Vector2.left);
        };
        rightAction.action.performed += (ctx) =>
        {
            MoveInDirection(Vector2.right);
        };

        upAction.action.canceled += StopMoving;
        downAction.action.canceled += StopMoving;
        leftAction.action.canceled += StopMoving;
        rightAction.action.canceled += StopMoving;*/
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        rb.velocity = movementInput * movementSpeed * Time.fixedDeltaTime;
    }

    private void MoveInDirection(Vector2 direction)
    {
        Debug.Log("Moving " + direction);
        rb.velocity = direction * movementSpeed * Time.fixedDeltaTime;
    }

    private void StopMoving(InputAction.CallbackContext ctx)
    {
        rb.velocity = Vector2.zero;
    }

    void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
        //Debug.Log("Movement input: " + movementInput);
    }
}
