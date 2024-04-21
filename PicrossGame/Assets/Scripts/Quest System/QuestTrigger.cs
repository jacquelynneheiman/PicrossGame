using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Trigger", menuName = "Pixel Quest/Quest System/Quest Trigger")]
public class QuestTrigger : ScriptableObject
{
    public Quest quest;
    public string triggerDescription;
}
