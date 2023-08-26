using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum PlayerInputActionMap {
  World,
  Dialog,
  Pause,
  Battle,
  Menu,
}

[CreateAssetMenu(fileName = "Player Input Manager", menuName = "ScriptableObjects/Managers/PlayerInputManager")]
public class PlayerInputManagerSO : ScriptableObject {
  public Controls Input;

  // World events
  public event Action<Vector2> OnWorldMove;
  public event Action OnWorldInteract;
  public event Action OnWorldPause;

  // Dialog events
  public event Action OnDialogSelect;

  // Pause events
  public event Action OnPauseResume;
  public event Action OnPauseUp;
  public event Action OnPauseDown;
  public event Action OnPauseSelect;

  // Battle events
  public event Action OnBattleBack;
  public event Action OnBattleUp;
  public event Action OnBattleDown;
  public event Action OnBattleSelect;

  // Menu Events
  public event Action OnMenuSelect;
  public event Action OnMenuLeft;
  public event Action OnMenuRight;

  private void OnEnable() {
    // Create input system instance
    Input ??= new Controls();

    // Subscribe to world events
    Input.World.Move.performed += MovementChanged;
    Input.World.Move.canceled += MovementChanged;
    Input.World.Interact.performed += InteractPerformed;
    Input.World.Pause.performed += PausePerformed;

    // Subscribe to dialog events
    Input.Dialog.Select.performed += DialogSelectPerformed;

    // Subscribe to pause events
    Input.Pause.Resume.performed += ResumePerformed;
    Input.Pause.Select.performed += SelectPerformed;
    Input.Pause.Up.performed += UpPerformed;
    Input.Pause.Down.performed += DownPerformed;

    // Subscribe to battle events
    Input.Battle.Back.performed += BattleBackPerformed;
    Input.Battle.Select.performed += BattleSelectPerformed;
    Input.Battle.Up.performed += BattleUpPerformed;
    Input.Battle.Down.performed += BattleDownPerformed;

    // Subscribe to menu events
    Input.Menu.Select.performed += MenuSelectPerformed;
    Input.Menu.Left.performed += MenuLeftPerformed;
    Input.Menu.Right.performed += MenuRightPerformed;


    // Enable the world map by default
#if UNITY_EDITOR
    Scene activeScene = SceneManager.GetActiveScene();

    if (activeScene.name == "World") {
      ChangeActionMap(PlayerInputActionMap.World);
    }
    else if (activeScene.name == "Battle") {
      ChangeActionMap(PlayerInputActionMap.Battle);
    }
    else {
      ChangeActionMap(PlayerInputActionMap.Menu);
    }
#else
        Input.Menu.Enable();
#endif
  }



  private void OnDisable() {
    // Unsubscribe from world events
    Input.World.Move.performed -= MovementChanged;
    Input.World.Move.canceled -= MovementChanged;
    Input.World.Interact.performed -= InteractPerformed;
    Input.World.Pause.performed -= PausePerformed;

    // Unsubscribe from dialog events
    Input.Dialog.Select.performed -= DialogSelectPerformed;

    // Unsubscribe from pause events
    Input.Pause.Resume.performed -= ResumePerformed;
    Input.Pause.Select.performed -= SelectPerformed;
    Input.Pause.Up.performed -= UpPerformed;
    Input.Pause.Down.performed -= DownPerformed;

    // Unsubscribe from  battle events
    Input.Battle.Back.performed -= BattleBackPerformed;
    Input.Battle.Select.performed -= BattleSelectPerformed;
    Input.Battle.Up.performed -= BattleUpPerformed;
    Input.Battle.Down.performed -= BattleDownPerformed;

    // Unsubscribe to menu events
    Input.Menu.Select.performed -= MenuSelectPerformed;
    Input.Menu.Left.performed -= MenuLeftPerformed;
    Input.Menu.Right.performed -= MenuRightPerformed;

    // Disable all input
    DisableAllActionMaps();
  }

  #region Action maps
  public void DisableAllActionMaps() {
    Input.World.Disable();
    Input.Dialog.Disable();
    Input.Pause.Disable();
    Input.Battle.Disable();
    Input.Menu.Disable();
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
      case PlayerInputActionMap.World:
        Input.World.Enable();
        break;
      case PlayerInputActionMap.Battle:
        Input.Battle.Enable();
        break;
      case PlayerInputActionMap.Menu:
        Input.Menu.Enable();
        break;
      default:
        break;
    }
  }
  #endregion

  #region World Handlers
  private void MovementChanged(InputAction.CallbackContext obj) {
    OnWorldMove?.Invoke(obj.ReadValue<Vector2>());
  }

  private void InteractPerformed(InputAction.CallbackContext obj) {
    OnWorldInteract?.Invoke();
  }

  private void PausePerformed(InputAction.CallbackContext context) {
    OnWorldPause?.Invoke();
  }
  #endregion

  #region Dialog Handlers
  private void DialogSelectPerformed(InputAction.CallbackContext obj) {
    OnDialogSelect?.Invoke();
  }
  #endregion

  #region Pause Handlers
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
  #endregion

  #region Battle Handlers
  private void BattleDownPerformed(InputAction.CallbackContext context) {
    OnBattleDown?.Invoke();
  }

  private void BattleUpPerformed(InputAction.CallbackContext context) {
    OnBattleUp?.Invoke();
  }

  private void BattleSelectPerformed(InputAction.CallbackContext context) {
    OnBattleSelect?.Invoke();
  }

  private void BattleBackPerformed(InputAction.CallbackContext context) {
    OnBattleBack?.Invoke();
  }
  #endregion

  #region Menu Handlers
  private void MenuSelectPerformed(InputAction.CallbackContext context) {
    OnMenuSelect?.Invoke();
  }

  private void MenuLeftPerformed(InputAction.CallbackContext context) {
    OnMenuLeft?.Invoke();
  }

  private void MenuRightPerformed(InputAction.CallbackContext context) {
    OnMenuRight?.Invoke();
  }
  #endregion
}
