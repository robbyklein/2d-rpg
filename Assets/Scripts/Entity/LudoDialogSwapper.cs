using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudoDialogSwapper : MonoBehaviour {
    [SerializeField] private EntityDialog dialog;
    [SerializeField] private EntityDestroyReaction reaction;

    private void OnEnable() {
        reaction.OnDestroyReaction += SwapLudoLanguageKey;
    }

    private void OnDisable() {
        reaction.OnDestroyReaction -= SwapLudoLanguageKey;
    }

    private void SwapLudoLanguageKey(GameObject obj) {
        dialog.SwapNpcKey("ludo2");
    }
}
