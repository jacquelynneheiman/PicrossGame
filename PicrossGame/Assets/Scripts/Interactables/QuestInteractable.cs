using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractable : MonoBehaviour, IInteractable
{
    public QuestTrigger triggerInfo;

    private bool hasFinishedQuestLoop = false;

    public void Interact()
    {
        if (!hasFinishedQuestLoop)
        {
            bool hasTriggerInfo = triggerInfo != null && triggerInfo.quest != null;

            if (hasTriggerInfo)
            {
                bool hasQuest = QuestLog.Instance.GetQuest(triggerInfo.quest.questID) != null;

                if (!hasQuest)
                {
                    QuestManager.Instance.ShowQuestDetails(triggerInfo);
                }
                else
                {
                    bool isComplete = triggerInfo.quest.HasRequirementsMet() || triggerInfo.quest.status == Quest.QuestStatus.Completed;

                    if (isComplete)
                    {
                        foreach (QuestReward questReward in triggerInfo.quest.rewards)
                        {
                            QuestManager.Instance.ApplyQuestReward(questReward);
                        }

                        hasFinishedQuestLoop = true;
                    }
                }
            }
        }
    }
}
