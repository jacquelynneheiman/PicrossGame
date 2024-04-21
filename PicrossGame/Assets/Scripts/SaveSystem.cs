using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveSystem
{
    public static void SaveQuests(Quest[] quests)
    {
        List<QuestData> questDataList = quests.Select(q => new QuestData { questID = q.questID, status = (int)q.status }).ToList();
        string json = JsonUtility.ToJson(new {quests = questDataList});
        System.IO.File.WriteAllText(Application.persistentDataPath + "/quests.json", json);
    }

    public static void LoadQuests(Quest[] quests)
    {
        string path = Application.persistentDataPath + "/quests.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            QuestData[] questDataList = JsonUtility.FromJson<QuestData[]>(json);

            foreach (var questData in questDataList)
            {
                Quest quest = quests.FirstOrDefault(q => q.questID == questData.questID);

                if (quest != null)
                {
                    quest.status = (Quest.QuestStatus)questData.status;
                }
            }
        }
    }
}
