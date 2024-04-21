using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
    }

    public void PlayGame()
    {
        sceneLoader.LoadScene("RPGWorld");
    }

    public void ContinueGame()
    {
        // load the last saved game data...
    }

    public void Options()
    {
        // load the options level...
    }

    public void QuitGame()
    {
        sceneLoader.QuitGame();
    }
}
