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

        if (quest != null && !quest.isComplete)
        {
            quest.AcceptQuest();
        }
    }

    private void AcceptQuest(Quest quest)
    {
        QuestManager.Instance.StartQuest(quest.questID);
        HideQuestDetails();
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

        if (quest != null && !quest.isComplete)
        {
            quest.CompleteQuest();
        }
    }

    private Quest GetQuestByID(string questID)
    {
        return quests.ToList().Where((quest) => quest.questID == questID).FirstOrDefault();
    }
}
