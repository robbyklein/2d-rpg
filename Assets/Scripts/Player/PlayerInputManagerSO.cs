using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerInputActionMap {
    World,
    Dialog,
    Pause,
}

[CreateAssetMenu(fileName = "Player Input Manager", menuName = "ScriptableObjects/Managers/PlayerInputManager")]
public class PlayerInputManagerSO : ScriptableObject {
    public Controls Input;

    public event Action<Vector2> OnMove;
    public event Action OnInteract;
    public event Action OnDialogSelect;
    public event Action OnPause;
    public event Action OnResume;

    private void OnEnable() {
        Input ??= new Controls();

        Input.World.Move.performed += MovementChanged;
        Input.World.Move.canceled += MovementChanged;
        Input.World.Interact.performed += InteractPerformed;
        Input.World.Pause.performed += PausePerformed;

        Input.Dialog.Select.performed += DialogSelectPerformed;

        Input.Pause.Resume.performed += ResumePerformed;

        Input.World.Enable();
    }

    private void OnDisable() {
        Input.World.Move.performed -= MovementChanged;
        Input.World.Move.canceled -= MovementChanged;
        Input.World.Interact.performed -= InteractPerformed;
        Input.World.Pause.performed -= PausePerformed;

        Input.Dialog.Select.performed -= DialogSelectPerformed;

        Input.Pause.Resume.performed -= ResumePerformed;

        Input.World.Disable();
    }

    private void MovementChanged(InputAction.CallbackContext obj) {
        OnMove?.Invoke(obj.ReadValue<Vector2>());
    }

    private void InteractPerformed(InputAction.CallbackContext obj) {
        OnInteract?.Invoke();
    }

    private void DialogSelectPerformed(InputAction.CallbackContext obj) {
        OnDialogSelect?.Invoke();
    }

    private void PausePerformed(InputAction.CallbackContext context) {
        OnPause?.Invoke();
    }

    private void ResumePerformed(InputAction.CallbackContext context) {
        OnResume?.Invoke();
    }

    public void DisableAllActionMaps() {
        Input.World.Disable();
        Input.Dialog.Disable();
    }

    public void ChangeActionMap(PlayerInputActionMap playerInputActionMap) {
        // Disable app input maps
        DisableAllActionMaps();

        // Enable the desired one
        switch (playerInputActionMap) {
            case PlayerInputActionMap.Dialog:
                Input.Dialog.Enable();
                break;
            case PlayerInputActionMap.Pause:
                Input.Pause.Enable();
                break;
            default:
                Input.World.Enable();
                break;

        }
    }
}
