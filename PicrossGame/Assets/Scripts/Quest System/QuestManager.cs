using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public Quest[] quests;

    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescriptionText;
    public GameObject questUIPanel;
    public Button acceptButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void StartQuest(string questID)
    {
        Quest quest = GetQuestByID(questID);

        if (quest != null && quest.status != Quest.QuestStatus.Completed)
        {
            quest.AcceptQuest();
        }
    }

    public void StartQuest(Quest quest)
    {
        if (quest != null && quest.status != Quest.QuestStatus.Completed)
        {
            quest.AcceptQuest();
        }
    }

    private void AcceptQuest(Quest quest)
    {
        Debug.Log("Accepting Quest: " + quest.title);

        // add the quest to the players quest log
        QuestLog.Instance.AddQuest(quest);

        // start the quest and hide the ui
        StartQuest(quest);
        HideQuestDetails();
    }

    public void AbandonQuest(Quest quest)
    {
        quest.AbandonQuest();
    }

    public void ShowQuestDetails(QuestTrigger questTrigger)
    {
        Debug.Log("Show quest details: " + questTrigger.quest.title);

        questTitleText.text = questTrigger.quest.title;
        questDescriptionText.text = questTrigger.quest.description;
        questUIPanel.SetActive(true);
        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(() => AcceptQuest(questTrigger.quest));
    }

    public void HideQuestDetails()
    {
        questUIPanel.SetActive(false);
    }

    public void CompleteQuest(string questID)
    {
        Quest quest = GetQuestByID(questID);

        if (quest != null && quest.status != Quest.QuestStatus.Completed)
        {
            quest.CompleteQuest();
        }
    }

    private Quest GetQuestByID(string questID)
    {
        return quests.ToList().Where((quest) => quest.questID == questID).FirstOrDefault();
    }

    public void ApplyQuestReward(QuestReward reward)
    {
        IQuestReward questReward = (IQuestReward)reward;

        if (questReward != null)
        {
            questReward.ApplyReward();
        }
    }
}
