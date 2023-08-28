using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteract : MonoBehaviour {
    [SerializeField] private AudioManagerSO audioManager;
    // Components
    [SerializeField] private UIDocument interactionDisplay;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInputManagerSO inputManager;

    private BoxCollider2D coll;

    // Settings
    private float interactDistance = 0.5f;

    // State
    private PlayerMovementState playerMovementState = PlayerMovementState.Right;
    private bool uiBusy = false;

    private void OnEnable() {
        inputManager.OnWorldInteract += HandleInteract;
        playerMovement.OnMovementDirectionChange += HandleMovementStateChange;
        EntityDialog.OnDialogBusy += HandleNpcBusyChange;
    }

    private void OnDisable() {
        inputManager.OnWorldInteract -= HandleInteract;
        playerMovement.OnMovementDirectionChange -= HandleMovementStateChange;
        EntityDialog.OnDialogBusy -= HandleNpcBusyChange;
    }

    private void Start() {
        coll = GetComponent<BoxCollider2D>();
        StartCoroutine(SetInteractDisplay());
    }

    private void HandleInteract() {
        GameObject hitObject = CheckForHit();
        if (!hitObject) return;

        // See if it's interactable
        IInteractable interactableComponent = hitObject.GetComponent<IInteractable>();

        if (interactableComponent != null) {
            interactableComponent.OnInteract();
            audioManager.PlaySFX(SoundClip.MenuChange);
        }
    }

    private IEnumerator SetInteractDisplay() {
        UIDocument el = interactionDisplay.GetComponent<UIDocument>();
        VisualElement root = el.rootVisualElement;

        while (true) {
            if (!uiBusy) {
                if (CheckForInteractable()) {
                    root.style.display = DisplayStyle.Flex;
                }
                else {
                    root.style.display = DisplayStyle.None;
                }
            }
            else {
                root.style.display = DisplayStyle.None;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private bool CheckForInteractable() {
        // Check that we hit something
        GameObject hitObject = CheckForHit();
        if (!hitObject) return false;

        IInteractable interactable = hitObject.GetComponent<IInteractable>();
        if (interactable == null) return false;

        return true;
    }

    private GameObject CheckForHit() {
        // Check if we hit something
        Vector2 direction = MovementStateToDirection();
        RaycastHit2D hit = Physics2D.Raycast(coll.bounds.center, direction, interactDistance);

        if (!hit) {
            return null;
        }
        else {
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
