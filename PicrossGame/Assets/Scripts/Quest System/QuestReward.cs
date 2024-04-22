using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward : ScriptableObject, IQuestReward
{
    public virtual void ApplyReward()
    {
        // implemented by children...
    }
}
