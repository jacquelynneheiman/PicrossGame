using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Experience Reward", menuName = "Pixel Quest/Quest System/Rewards/Experience Reward", order = 50)]
public class ExperienceReward : QuestReward
{
    public int experince;

    public override void ApplyReward()
    {
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

        if (player != null)
        {
            player.GainExperience(experince);
        }
    }
}
