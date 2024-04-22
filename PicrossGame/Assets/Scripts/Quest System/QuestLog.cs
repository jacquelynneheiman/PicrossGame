using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    public static QuestLog Instance;
    private List<Quest> quests = new List<Quest>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void AddQuest(Quest quest)
    {
        if (!quests.Contains(quest))
        {
            Debug.Log("Adding Quest: " + quest.title);
            quests.Add(quest);
        }

        Debug.Log("Quest Count: " + quests.Count);
    }

    public Quest GetQuest(string questID)
    {
        return quests.Find(q => q.questID == questID);
    }

    public List<Quest> GetAllQuests()
    {
        foreach (Quest quest in quests)
        {
            Debug.Log("Getting " + quest.title);
        }

        return quests;
    }

    public void RemoveQuest(Quest quest)
    {
        if (quests.Contains(quest))
        {
            quests.Remove(quest);
        }
    }

    public void AbandonQuest(Quest quest)
    {
        RemoveQuest(quest);
        QuestManager.Instance.AbandonQuest(quest);
    }

    public void CheckQuestProgress()
    {
        foreach (Quest quest in quests)
        {
            if (quest.HasRequirementsMet())
            {
                quest.CompleteQuest();
            }
        }
    }
}
