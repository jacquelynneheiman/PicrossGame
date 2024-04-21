using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject winScreen;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowWinUI()
    {
        winScreen.SetActive(true);
    }
}
