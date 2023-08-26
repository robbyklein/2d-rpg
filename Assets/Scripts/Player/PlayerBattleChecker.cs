using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleChecker : MonoBehaviour {
  [SerializeField] private GameManagerSO gameManager;
  [SerializeField] private PlayerMovement playerMovement;
  [SerializeField] private LayerMask dangerousGround;

  [SerializeField] private float battleThreshold = 0.6f;
  [SerializeField] private int battleCooldown = 2;
  [SerializeField] private int maxBattleDistance = 5;

  private int lastBattleDistance = 0;

  private bool playerIsMoving = false;
  private bool playerInDangerousGround = false;
  private GameState gameState = GameState.World;

  private void OnEnable() {
    playerMovement.OnPlayerIsMovingChange += handlePlayerIsMovingChange;
    gameManager.onGameStateChange += handleGameStateChange;
  }

  private void OnDisable() {
    playerMovement.OnPlayerIsMovingChange -= handlePlayerIsMovingChange;
    gameManager.onGameStateChange -= handleGameStateChange;
  }

  private void Start() {
    StartCoroutine(BattleCheckRoutine());
  }

  private bool ShouldFightOccur() {
    if (lastBattleDistance > maxBattleDistance) {
      return true;
    }

    float randomValue = UnityEngine.Random.value;
    return randomValue > battleThreshold && playerIsMoving && playerInDangerousGround && gameState == GameState.World && lastBattleDistance > battleCooldown;
  }

  private IEnumerator BattleCheckRoutine() {
    while (true) {

      if (playerIsMoving && playerInDangerousGround) {
        lastBattleDistance += 1;
        Debug.Log(lastBattleDistance);
      }

      if (ShouldFightOccur()) {
        gameManager.ChangeGameState(GameState.Battle);
        lastBattleDistance = 0;
      }

      yield return new WaitForSeconds(1.0f);
    }
  }

  private void handlePlayerIsMovingChange(bool moving) {
    playerIsMoving = moving;
  }

  private void handleGameStateChange(GameState state) {
    gameState = state;
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if ((dangerousGround.value & 1 << col.gameObject.layer) != 0) {
      playerInDangerousGround = true;
    }
  }

  private void OnTriggerExit2D(Collider2D col) {
    if ((dangerousGround.value & 1 << col.gameObject.layer) != 0) {
      playerInDangerousGround = false;
    }
  }

}
