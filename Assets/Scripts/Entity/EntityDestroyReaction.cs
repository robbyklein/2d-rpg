using UnityEngine;

public class EntityDestroyReaction : MonoBehaviour {
    [SerializeField] private GameObject destroyObject;
    private EntityDialog npcDialog;

    private void OnEnable() {
        npcDialog ??= GetComponent<EntityDialog>();
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
