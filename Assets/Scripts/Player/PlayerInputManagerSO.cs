using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Input Manager", menuName = "ScriptableObjects/Managers/PlayerInputManager")]
public class PlayerInputManagerSO : ScriptableObject {
    private Controls Input;

    public event Action<Vector2> OnMove;

    private void OnEnable() {
        Input ??= new Controls();

        Input.World.Move.performed += MovementChanged;
        Input.World.Move.canceled += MovementChanged;

        Input.World.Enable();
    }

    private void OnDisable() {
        Input.World.Move.performed -= MovementChanged;
        Input.World.Move.canceled -= MovementChanged;

        Input.World.Disable();
    }

    private void MovementChanged(InputAction.CallbackContext obj) {
        OnMove?.Invoke(obj.ReadValue<Vector2>());
    }
}
