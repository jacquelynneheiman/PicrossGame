using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private LevelData currentLevelData;

    public SceneLoader sceneLoader;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple Game Managers in the scene");
            Destroy(this.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartPicrossLevel(LevelData levelData)
    {
        this.currentLevelData = levelData;
        sceneLoader.LoadScene("Level");
    } 

    public LevelData GetCurrentLevel()
    {
        return currentLevelData;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

