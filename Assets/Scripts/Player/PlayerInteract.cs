using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteract : MonoBehaviour {
    [SerializeField] private UIDocument interactionDisplay;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInputManagerSO inputManager;

    private BoxCollider2D coll;

    private float interactDistance = 0.5f;
    private PlayerMovementState playerMovementState = PlayerMovementState.Right;
    private bool uiBusy = false;

    private void OnEnable() {
        inputManager.OnInteract += HandleInteract;
        playerMovement.OnMovementDirectionChange += HandleMovementStateChange;
        NpcDialog.OnBusy += HandleNpcBusyChange;
    }

    private void OnDisable() {
        inputManager.OnInteract -= HandleInteract;
        playerMovement.OnMovementDirectionChange -= HandleMovementStateChange;
        NpcDialog.OnBusy -= HandleNpcBusyChange;
    }

    private void Start() {
        coll = GetComponent<BoxCollider2D>();

        StartCoroutine(SetInteractDisplay());
    }

    private void HandleInteract() {
        GameObject hitObject = CheckForHit();
        if (!hitObject) return;

        if (hitObject.CompareTag("Npc")) {
            NpcDialog dialog = hitObject.GetComponentInParent<NpcDialog>();
            dialog?.StartConversation();
        }
    }

    private IEnumerator SetInteractDisplay() {
        while (true) {
            if (!uiBusy) {
                interactionDisplay.enabled = CheckForInteractable();
            } else {
                interactionDisplay.enabled = false;
            }

            yield return new WaitForSeconds(0.1f);
        }

        interactionDisplay.enabled = false;

    }

    private bool CheckForInteractable() {
        GameObject hitObject = CheckForHit();
        if (!hitObject) return false;

        if (hitObject.CompareTag("Npc")) {
            return true;
        } else {
            return false;
        }
    }

    private GameObject CheckForHit() {
        // Check if we hit something
        Vector2 direction = MovementStateToDirection();
        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, direction, interactDistance);

        if (!hit) {
            return null;
        } else {
            return hit.collider.gameObject;
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

    private void HandleNpcBusyChange(bool busy) {
        uiBusy = busy;
    }
}
