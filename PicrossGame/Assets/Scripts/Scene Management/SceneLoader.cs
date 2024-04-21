using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    public static event EventHandler OnLoadScene;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
        OnLoadScene?.Invoke(this, EventArgs.Empty);
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.fillAmount = progress;
            progressText.text = (int)(progress * 100) + "%";

            Debug.Log(progress);

            if (operation.progress >= 0.9f && !operation.allowSceneActivation)
            {
                loadingScreen.SetActive(false);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        Debug.Log("Done!");
        loadingScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
