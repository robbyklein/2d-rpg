using System;
using UnityEngine;

public enum PlayerMovementState {
    RightIdle,
    Right,
    LeftIdle,
    Left,
    DownIdle,
    Down,
    UpIdle,
    Up
}

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private PlayerInputManagerSO inputManager;
    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody2D rb;
    private Animator animator;

    public event Action<PlayerMovementState> OnMovementDirectionChange;

    private Vector2 movementVector;
    private PlayerMovementState lastMovementState = PlayerMovementState.RightIdle;

    private void OnEnable() {
        inputManager.OnWorldMove += HandleOnChange;
    }

    private void OnDisable() {
        inputManager.OnWorldMove -= HandleOnChange;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        Move();
    }

    private void HandleOnChange(Vector2 inputVector) {
        movementVector = inputVector;
    }

    private void Move() {
        if (movementVector == Vector2.zero) {
            rb.velocity = new Vector2(0, 0);

            switch (lastMovementState) {
                case PlayerMovementState.Left: {
                        UpdateAnimationState(PlayerMovementState.LeftIdle);
                        break;
                    }
                case PlayerMovementState.Up: {
                        UpdateAnimationState(PlayerMovementState.UpIdle);
                        break;
                    }
                case PlayerMovementState.Down: {
                        UpdateAnimationState(PlayerMovementState.DownIdle);
                        break;
                    }
                default: {
                        UpdateAnimationState(PlayerMovementState.RightIdle);
                        break;
                    }
            }
        } else if (movementVector.y > 0) {
            rb.velocity = new Vector2(0, moveSpeed);
            OnMovementDirectionChange?.Invoke(PlayerMovementState.Up);
            UpdateAnimationState(PlayerMovementState.Up);
        } else if (movementVector.y < 0) {
            rb.velocity = new Vector2(0, -moveSpeed);
            OnMovementDirectionChange?.Invoke(PlayerMovementState.Down);
            UpdateAnimationState(PlayerMovementState.Down);
        } else if (movementVector.x > 0) {
            rb.velocity = new Vector2(moveSpeed, 0);
            OnMovementDirectionChange?.Invoke(PlayerMovementState.Right);
            UpdateAnimationState(PlayerMovementState.Right);
        } else if (movementVector.x < 0) {
            rb.velocity = new Vector2(-moveSpeed, 0);
            OnMovementDirectionChange?.Invoke(PlayerMovementState.Left);
            UpdateAnimationState(PlayerMovementState.Left);
        }
    }

    private void UpdateAnimationState(PlayerMovementState movementState) {
        if (lastMovementState == movementState) return;

        switch (movementState) {
            case PlayerMovementState.Right: {
                    animator.SetInteger("State", 1);
                    break;
                }
            case PlayerMovementState.LeftIdle: {
                    animator.SetInteger("State", 2);
                    break;
                }
            case PlayerMovementState.Left: {
                    animator.SetInteger("State", 3);
                    break;
                }
            case PlayerMovementState.DownIdle: {
                    animator.SetInteger("State", 4);
                    break;
                }
            case PlayerMovementState.Down: {
                    animator.SetInteger("State", 5);
                    break;
                }
            case PlayerMovementState.UpIdle: {
                    animator.SetInteger("State", 6);
                    break;
                }
            case PlayerMovementState.Up: {
                    animator.SetInteger("State", 7);
                    break;
                }
            default: {
                    animator.SetInteger("State", 0);
                    break;
                }

        }

        lastMovementState = movementState;
    }
}
