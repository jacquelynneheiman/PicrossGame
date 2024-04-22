using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image xpFill;

    public void UpdateXP(float currentXP, float nextLevelXP)
    {
        xpFill.fillAmount = currentXP / nextLevelXP;
    }
}
