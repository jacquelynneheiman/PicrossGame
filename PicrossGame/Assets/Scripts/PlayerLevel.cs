using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLevel 
{
    public int currentLevel;
    public int currentXP;
    public int nextLevelXP;

    public PlayerLevel(int level, int xp, int nextXP)
    {
        currentLevel = level;
        currentXP = xp;
        nextLevelXP = nextXP;
    } 

    /// <summary>
    /// Calculate the XP needed for each level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static int CalculateNextLevelXP(int level)
    {
        return (int)(100 * Mathf.Pow(1.5f, level - 1));
    }

    /// <summary>
    /// Add Experience and handle leveling up
    /// </summary>
    /// <param name="xp"></param>
    public void AddExperience(int xp)
    {
        currentXP += xp;

        while (currentXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;

        currentXP -= nextLevelXP;
        nextLevelXP = CalculateNextLevelXP(currentLevel);
    }

    public int GetXP()
    {
        return currentXP;
    }
}
