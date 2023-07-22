using UnityEngine;

public class PlayerInteract : MonoBehaviour {
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInputManagerSO inputManager;
    private BoxCollider2D collider2D;

    private float interactDistance = 0.5f;
    private PlayerMovementState playerMovementState = PlayerMovementState.Right;

    private void OnEnable() {
        Debug.Log("On enable!");
        inputManager.OnInteract += HandleInteract;
        playerMovement.OnMovementDirectionChange += HandleMovementStateChange;
    }

    private void OnDisable() {
        Debug.Log("On disable!");

        inputManager.OnInteract -= HandleInteract;
        playerMovement.OnMovementDirectionChange -= HandleMovementStateChange;
    }

    private void Start() {
        collider2D = GetComponent<BoxCollider2D>();
    }

    private void HandleInteract() {
        // Check if we hit something
        Vector2 direction = MovementStateToDirection();
        RaycastHit2D hit = Physics2D.Raycast(collider2D.bounds.center, direction, interactDistance);

        if (!hit) return;

        // Then do the right thing
        GameObject gameObject = hit.collider.gameObject;

        if (gameObject.CompareTag("Npc")) {
            NpcDialog dialog = gameObject.GetComponentInParent<NpcDialog>();
            dialog?.StartConversation();
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
