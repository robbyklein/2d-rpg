using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    enum MovementState {
        RightIdle,
        Right,
        LeftIdle,
        Left,
        DownIdle,
        Down,
        UpIdle,
        Up
    }

    [SerializeField] private PlayerInputManagerSO inputManager;
    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movementVector;
    private MovementState lastMovementState = MovementState.RightIdle;

    private void OnEnable() {
        inputManager.OnMove += HandleOnChange;
    }

    private void OnDisable() {
        inputManager.OnMove -= HandleOnChange;
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        Move();
    }

    private void HandleOnChange(Vector2 inputVector) {
        movementVector = inputVector;
    }

    private void Move() {
        if (movementVector == Vector2.zero) {
            rb.velocity = new Vector2(0, 0);

            switch (lastMovementState) {
                case MovementState.Left: {
                        UpdateAnimationState(MovementState.LeftIdle);
                        break;
                    }
                case MovementState.Up: {
                        UpdateAnimationState(MovementState.UpIdle);
                        break;
                    }
                case MovementState.Down: {
                        UpdateAnimationState(MovementState.DownIdle);
                        break;
                    }
                default: {
                        UpdateAnimationState(MovementState.RightIdle);
                        break;
                    }
            }
        } else if (movementVector.y > 0) {
            rb.velocity = new Vector2(0, 1);
            UpdateAnimationState(MovementState.Up);
        } else if (movementVector.y < 0) {
            rb.velocity = new Vector2(0, -1);
            UpdateAnimationState(MovementState.Down);
        } else if (movementVector.x > 0) {
            rb.velocity = new Vector2(1, 0);
            UpdateAnimationState(MovementState.Right);
        } else if (movementVector.x < 0) {
            rb.velocity = new Vector2(-1, 0);
            UpdateAnimationState(MovementState.Left);
        }
    }

    private void UpdateAnimationState(MovementState movementState) {
        if (lastMovementState == movementState) return;

        switch (movementState) {
            case MovementState.Right: {
                    animator.SetInteger("State", 1);
                    break;
                }
            case MovementState.LeftIdle: {
                    animator.SetInteger("State", 2);
                    break;
                }
            case MovementState.Left: {
                    animator.SetInteger("State", 3);
                    break;
                }
            case MovementState.DownIdle: {
                    animator.SetInteger("State", 4);
                    break;
                }
            case MovementState.Down: {
                    animator.SetInteger("State", 5);
                    break;
                }
            case MovementState.UpIdle: {
                    animator.SetInteger("State", 6);
                    break;
                }
            case MovementState.Up: {
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
