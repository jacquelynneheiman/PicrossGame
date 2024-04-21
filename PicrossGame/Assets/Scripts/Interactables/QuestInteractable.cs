using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractable : MonoBehaviour, IInteractable
{
    public QuestTrigger triggerInfo;
    private bool isTriggered = false;

    public void Interact()
    {
        Debug.Log("Interacting!");

        if (triggerInfo != null && triggerInfo.quest != null)
        {
            isTriggered = true;
            QuestManager.Instance.ShowQuestDetails(triggerInfo);
            Debug.Log("Triggering quest: " + triggerInfo.quest.title);
        }
    }
}
