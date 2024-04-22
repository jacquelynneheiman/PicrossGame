using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Pixel Quest/Quest System/Quest", order = 50)]
public class Quest : ScriptableObject
{
    public enum QuestStatus { NotStarted, Accepted, Completed }

    [HideInInspector]
    public string questID;

    public string title;
    public string description;
    public QuestStatus status = QuestStatus.NotStarted;
    public List<QuestRequirement> requirements;
    public List<QuestReward> rewards;

    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(questID))
        {
            questID = Guid.NewGuid().ToString();
        }
    }

    public void AcceptQuest()
    {
        if (status != QuestStatus.Completed && status != QuestStatus.Accepted)
        {
            status = QuestStatus.Accepted;
            Debug.Log(title + " accepted");
        }
    }
    
    public void AbandonQuest()
    {
        if (status == QuestStatus.Accepted)
        {
            status = QuestStatus.NotStarted;
            Debug.Log(title + " abandoned");
        }
    }

    public bool HasRequirementsMet()
    {
        foreach(var requirement in requirements)
        {
            if (!requirement.IsSatisfied())
            {
                return false;
            }
        }

        return true;
    }

    public void CompleteQuest()
    {
        if (status != QuestStatus.Completed)
        {
            if (HasRequirementsMet())
            {
                status = QuestStatus.Completed;
                Debug.Log("Completed " + title);
            }
        }
    }
}
