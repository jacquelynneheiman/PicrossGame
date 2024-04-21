using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicrossLevelActions : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
    }

    public void CompletePicrossLevel()
    {
        sceneLoader.LoadScene("RPGWorld");
    }
}
