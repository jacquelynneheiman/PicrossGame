using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private TextAsset dialogue;

    [SerializeField]
    private DialogueManager dialogueManager;

    public void Interact()
    {
        dialogueManager.StartDialogue(dialogue);
    }
}
