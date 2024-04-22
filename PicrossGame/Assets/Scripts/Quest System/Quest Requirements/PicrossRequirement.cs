using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Picross Requirement", menuName = "Pixel Quest/Quest System/Quest Requirement", order = 50)]
public class PicrossRequirement : QuestRequirement
{
    public LevelData levelData;

    public override bool IsSatisfied()
    {
        return levelData.isComplete;
    }
}
