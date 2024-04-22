using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterController
{
    public PlayerLevel levelInfo;
    public PlayerHUD playerHUD;

    private void Start()
    {
        levelInfo = new PlayerLevel(1, 0, PlayerLevel.CalculateNextLevelXP(1));
        playerHUD.UpdateXP(levelInfo.currentXP, levelInfo.nextLevelXP);
    }

    public void GainExperience(int xp)
    {
        levelInfo.AddExperience(xp);
        playerHUD.UpdateXP(levelInfo.currentXP, levelInfo.nextLevelXP);
    }

}
