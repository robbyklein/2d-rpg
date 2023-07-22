using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteract : MonoBehaviour {
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInputManagerSO inputManager;
    private BoxCollider2D collider2D;

    [SerializeField] private UIDocument uiDocument;

    private float interactDistance = 0.5f;
    private PlayerMovementState playerMovementState = PlayerMovementState.Right;

    private void OnEnable() {
        inputManager.OnInteract += HandleInteract;
        playerMovement.OnMovementDirectionChange += HandleMovementStateChange;
    }

    private void OnDisable() {
        inputManager.OnInteract -= HandleInteract;
        playerMovement.OnMovementDirectionChange -= HandleMovementStateChange;
    }

    private void Start() {
        collider2D = GetComponent<BoxCollider2D>();
    }

    private void HandleInteract() {
        Vector2 direction = MovementStateToDirection();
        RaycastHit2D hit = Physics2D.Raycast(collider2D.bounds.center, direction, interactDistance);

        Debug.DrawRay(collider2D.bounds.center, direction * interactDistance, Color.red);

        if (hit && hit.collider.gameObject.CompareTag("Npc")) {
            Debug.Log("Hit!" + hit.collider.gameObject.name);

            uiDocument.enabled = true;
        }
    }

    private Vector2 MovementStateToDirection() {
        switch (playerMovementState) {
            case PlayerMovementState.Left:
                return Vector2.left;
            case PlayerMovementState.Up:
                return Vector2.up;
            case PlayerMovementState.Down:
                return Vector2.down;
            default:
                return Vector2.right;
        }
    }

    private void HandleMovementStateChange(PlayerMovementState state) {
        playerMovementState = state;
    }
}
