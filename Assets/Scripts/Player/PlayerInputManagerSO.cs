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

    public event Action<Vector2> OnWorldMove;
    public event Action OnWorldInteract;
    public event Action OnWorldPause;

    public event Action OnDialogSelect;

    public event Action OnPauseResume;
    public event Action OnPauseUp;
    public event Action OnPauseDown;
    public event Action OnPauseSelect;

    private void OnEnable() {
        Input ??= new Controls();

        Input.World.Move.performed += MovementChanged;
        Input.World.Move.canceled += MovementChanged;
        Input.World.Interact.performed += InteractPerformed;
        Input.World.Pause.performed += PausePerformed;

        Input.Dialog.Select.performed += DialogSelectPerformed;

        Input.Pause.Resume.performed += ResumePerformed;
        Input.Pause.Select.performed += SelectPerformed;
        Input.Pause.Up.performed += UpPerformed;
        Input.Pause.Down.performed += DownPerformed;

        Input.World.Enable();
    }


    private void OnDisable() {
        Input.World.Move.performed -= MovementChanged;
        Input.World.Move.canceled -= MovementChanged;
        Input.World.Interact.performed -= InteractPerformed;
        Input.World.Pause.performed -= PausePerformed;

        Input.Dialog.Select.performed -= DialogSelectPerformed;

        Input.Pause.Resume.performed -= ResumePerformed;
        Input.Pause.Select.performed -= SelectPerformed;
        Input.Pause.Up.performed -= UpPerformed;
        Input.Pause.Down.performed -= DownPerformed;

        Input.World.Disable();
    }

    private void MovementChanged(InputAction.CallbackContext obj) {
        OnWorldMove?.Invoke(obj.ReadValue<Vector2>());
    }

    private void InteractPerformed(InputAction.CallbackContext obj) {
        OnWorldInteract?.Invoke();
    }

    private void DialogSelectPerformed(InputAction.CallbackContext obj) {
        OnDialogSelect?.Invoke();
    }

    private void PausePerformed(InputAction.CallbackContext context) {
        OnWorldPause?.Invoke();
    }

    private void ResumePerformed(InputAction.CallbackContext context) {
        OnPauseResume?.Invoke();
    }

    private void DownPerformed(InputAction.CallbackContext context) {
        OnPauseDown?.Invoke();
    }

    private void UpPerformed(InputAction.CallbackContext context) {
        OnPauseUp?.Invoke();
    }

    private void SelectPerformed(InputAction.CallbackContext context) {
        OnPauseSelect?.Invoke();
    }

    public void DisableAllActionMaps() {
        Input.World.Disable();
        Input.Dialog.Disable();
        Input.Pause.Disable();
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
