using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private Canvas dialogueCanvas;

    [SerializeField]
    private TextMeshProUGUI segmentText;

    private Story story;
    private bool isWaitingForUserInput = false;

    public void StartDialogue(TextAsset dialogue, string storyPathString = "start")
    {
        story = new Story(dialogue.text);
        story.ChoosePathString(storyPathString);

        ShowDialogueUI();
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (story.canContinue && !isWaitingForUserInput)
        {
            string text = story.Continue();
            segmentText.text = text;
            isWaitingForUserInput = true;
        }
        else
        {
            story = null;
            HideDialogueUI();
        }
    }

    private void ShowDialogueUI()
    {
        dialogueCanvas.gameObject.SetActive(true);
    }

    private void HideDialogueUI()
    {
        dialogueCanvas.gameObject.SetActive(false);
    }

    public void OnContinueDialogue(InputAction.CallbackContext context)
    {
        if (context.performed && story != null && isWaitingForUserInput)
        {
            isWaitingForUserInput = false;
            StartCoroutine(ProgressStory());
        }
    }

    IEnumerator ProgressStory()
    {
        yield return new WaitForSeconds(0.2f);
        ContinueStory();
    }
}
