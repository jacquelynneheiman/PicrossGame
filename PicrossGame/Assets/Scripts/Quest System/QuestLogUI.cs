using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class QuestLogUI : MonoBehaviour
{
    public GameObject questLogPanel;
    public GameObject questLogEntryPrefab;
    public Transform questListContent;
    public TextMeshProUGUI questDetailText;
    private Quest selectedQuest;

    private void PopulateQuestLog()
    {
        List<Quest> quests = QuestLog.Instance.GetAllQuests();

        Debug.Log("Quest Count: " + quests.Count);

        foreach (Quest quest in quests)
        {
            Debug.Log("Populating Quest Details: " + quest.title);
            GameObject questLogEntry = Instantiate(questLogEntryPrefab, questListContent);
            questLogEntry.GetComponentInChildren<TextMeshProUGUI>().text = quest.title;
            questLogEntry.GetComponent<Button>().onClick.AddListener(() => ShowQuestDetails(quest));
        }

        if (selectedQuest == null)
        {
            ShowQuestDetails(selectedQuest);
            
        }
        else if (quests.FirstOrDefault() != null)
        {
            ShowQuestDetails(quests.FirstOrDefault());
        }
        else
        {
            questDetailText.text = "";
        }
    }

    private void ShowQuestDetails(Quest quest)
    {
        if (quest == null) return;

        selectedQuest = quest;
        questDetailText.text = quest.description;
    }

    public void AbandonQuest()
    {
        if (selectedQuest != null)
        {
            QuestLog.Instance.AbandonQuest(selectedQuest);
            ClearQuestLog();
            PopulateQuestLog();
            selectedQuest = null;
        }
    }

    public void ToggleQuestLogUI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isOpen = questLogPanel.activeSelf;

            if (!isOpen)
            {
                ClearQuestLog();
                PopulateQuestLog();
                OpenQuestLog();
                return;
            }

            CloseQuestLog();
        }
    }

    public void CloseQuestLog()
    {
        questLogPanel.SetActive(false);
    }

    public void OpenQuestLog()
    {
        questLogPanel.SetActive(true);
    }

    private void ClearQuestLog()
    {
        List<Button> questLogEntries = questListContent.GetComponentsInChildren<Button>().ToList();

        foreach (Button entry in  questLogEntries)
        {
            Destroy(entry.gameObject);
        }

        questDetailText.text = "";
    }
}
