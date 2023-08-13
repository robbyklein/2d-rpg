using System;
using UnityEngine;

public class BattlePlayerTurn : MonoBehaviour {
    [SerializeField] private PlayerInputManagerSO playerInput;
    [SerializeField] private BattleManager battleManager;

    public event Action OnPlayerTurnFinished;

    private void OnEnable() {
        battleManager.OnEnemyTurnFinished += HandleEnemyTurnFinished;
    }

    private void OnDisable() {
        battleManager.OnEnemyTurnFinished -= HandleEnemyTurnFinished;
    }

    public void Attack() {
        playerInput.DisableAllActionMaps();

        Debug.Log(OnPlayerTurnFinished);
        OnPlayerTurnFinished?.Invoke();
    }

    public void UseItem() {
        playerInput.DisableAllActionMaps();

        Debug.Log("Player used an item");
        OnPlayerTurnFinished?.Invoke();
    }

    private void HandleEnemyTurnFinished() {
        playerInput.ChangeActionMap(PlayerInputActionMap.Battle);
    }

}
