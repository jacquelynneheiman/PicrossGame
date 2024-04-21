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
    public bool isComplete;
    public int reward;
    public QuestStatus status = QuestStatus.NotStarted;

    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(questID))
        {
            questID = Guid.NewGuid().ToString();
        }
    }

    public void AcceptQuest()
    {
        status = QuestStatus.Accepted;
        Debug.Log(title + " accepted");
    }

    public void CompleteQuest()
    {
        if (status == QuestStatus.Completed)
        {
            status = QuestStatus.Completed;
            Debug.Log(title + " completed!");
        }
    }
}
