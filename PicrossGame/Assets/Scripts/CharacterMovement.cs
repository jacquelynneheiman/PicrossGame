using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Animator animator;

    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Vector2 smoothMoveInput;

    private Vector2 lastMoveDirection;

    private float inputSmoothTime = 0.1f;
    private float velocityX;
    private float velocityY;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMoveCanceled;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput.normalized;
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void Update()
    {
        smoothMoveInput.x = Mathf.SmoothDamp(smoothMoveInput.x, moveInput.x, ref velocityX, inputSmoothTime);
        smoothMoveInput.y = Mathf.SmoothDamp(smoothMoveInput.y, moveInput.y, ref velocityY, inputSmoothTime);

        AnimateMovement();
    }

    private void FixedUpdate()
    {
        Vector2 move = new Vector2(moveInput.x, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    private void AnimateMovement()
    {
        if (moveInput.magnitude > 0)
        {
            animator.SetFloat("Horizontal", smoothMoveInput.x);
            animator.SetFloat("Vertical", smoothMoveInput.y);
        }

        animator.SetFloat("Speed", smoothMoveInput.sqrMagnitude);
    }

    private void OnDestroy()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMoveCanceled;
    }

    public Vector2 GetMoveDirection()
    {
        return lastMoveDirection;
    }
}
