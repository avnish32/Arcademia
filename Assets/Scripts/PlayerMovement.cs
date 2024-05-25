using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ResearchArcade;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float movementSpeed;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float speedModifier = 1f;
    private Animator animator;
    private GameStateController gameStateController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameStateController = FindObjectOfType<GameStateController>();
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
        if (gameStateController.isGamePaused)
        {
            return;
        }

        if (ArcadeInput.Exit.Down)
        {
            Debug.Log("Exit pressed.");
            Navigation.ExitGame();
        } else if (ArcadeInput.Player1.Start.Down)
        {
            gameStateController.OnGamePaused();
        }
    }

    private void FixedUpdate()
    {
        //rb.velocity = movementInput * movementSpeed * Time.fixedDeltaTime;
        

        if (ResearchArcade.ArcadeInput.Player1.JoyDown.HeldDown)
        {
            MoveInDirection(Vector2.down);
            animator.Play("down_walk");
        }
        else if (ResearchArcade.ArcadeInput.Player1.JoyUp.HeldDown)
        {
            MoveInDirection(Vector2.up);
            animator.Play("up_walk");
        }
        else if (ResearchArcade.ArcadeInput.Player1.JoyLeft.HeldDown)
        {
            MoveInDirection(Vector2.left);
            animator.Play("left_walk");
        }
        else if (ResearchArcade.ArcadeInput.Player1.JoyRight.HeldDown)
        {
            MoveInDirection(Vector2.right);
            animator.Play("right_walk");
        }
        else
        {
            StopMoving();
            animator.Play("Idle");
        }
    }

    private void MoveInDirection(Vector2 direction)
    {
        //Debug.Log("Moving " + direction);
        rb.velocity = direction * movementSpeed * Time.fixedDeltaTime * speedModifier;
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    private IEnumerator SlowDownTemporarily()
    {
        SetSpeedModifier(0.5f);
        yield return new WaitForSeconds(5f);
        SetSpeedModifier(1f);
    }

    private IEnumerator SpeedUpTemporarily()
    {
        SetSpeedModifier(1.5f);
        yield return new WaitForSeconds(5f);
        SetSpeedModifier(1f);
    }

    void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
        //Debug.Log("Movement input: " + movementInput);
    }

    public void SetSpeedModifier(float speedModifier)
    {
        this.speedModifier = speedModifier;
    }

    public void OnSpeedReducerPickup()
    {
        StartCoroutine(SlowDownTemporarily());
    }

    public void OnSpeedBoosterPickup()
    {
        StartCoroutine(SpeedUpTemporarily());
    }
}
