using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public PicrossLevels targetLevel = PicrossLevels.MainMenu;
    public LevelData levelData;

    private void OnMouseDown()
    {
        switch (targetLevel) 
        {
            case PicrossLevels.MainMenu:
                GameManager.Instance.LoadMainMenu();
                break;
            case PicrossLevels.Level:
                GameManager.Instance.StartPicrossLevel(levelData);
                break;
        }
    }

    public void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }
}

public enum PicrossLevels
{
    MainMenu, 
    Level
}