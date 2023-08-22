using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleChecker : MonoBehaviour {
  [SerializeField] private PlayerMovement playerMovement;
  [SerializeField] private LayerMask dangerousGround;

  private bool playerIsMoving = false;
  private bool playerInDangerousGround = false;

  private void OnEnable() {
    playerMovement.OnPlayerIsMovingChange += handlePlayerIsMovingChange;
  }

  private void OnDisable() {
    playerMovement.OnPlayerIsMovingChange -= handlePlayerIsMovingChange;
  }

  private void Start() {
    StartCoroutine(BattleCheckRoutine());
  }

  private IEnumerator BattleCheckRoutine() {
    while (true) {
      if (playerIsMoving && playerInDangerousGround) {
        Debug.Log("Should we fight?");
      }

      yield return new WaitForSeconds(1.0f);
    }
  }

  private void handlePlayerIsMovingChange(bool moving) {
    playerIsMoving = moving;
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
