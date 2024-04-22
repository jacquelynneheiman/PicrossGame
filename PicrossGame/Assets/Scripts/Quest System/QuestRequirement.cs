using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class QuestRequirement : ScriptableObject
{
    public abstract bool IsSatisfied();   
}
