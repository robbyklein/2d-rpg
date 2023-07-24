using UnityEngine;

public class NpcDestroy : MonoBehaviour {
    [SerializeField] private GameObject destroyObject;
    [SerializeField] private NpcDialog npcDialog;

    private void OnEnable() {
        npcDialog.OnConversationFinished += HandleConversationFinished;
    }

    private void OnDisable() {
        npcDialog.OnConversationFinished -= HandleConversationFinished;
    }

    private void HandleConversationFinished(string npcKey) {
        DestroyObject();
    }
    private void DestroyObject() {
        destroyObject.SetActive(false);
    }
}
