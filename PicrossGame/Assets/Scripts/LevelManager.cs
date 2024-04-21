using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LevelManager : MonoBehaviour
{
    public enum LevelState
    {
        Initialize,
        Gameplay,
        Win,
        Cleanup
    }

    private LevelData currentLevel;
    private LevelGenerator levelGenerator;
    private PicrossCell[,] currentLevelGrid;

    private LevelState currentState;
    private PicrossCellState[,] solution;

    private void Start()
    {
        levelGenerator = GetComponent<LevelGenerator>();
        ChangeState(LevelState.Initialize);
    }

    private void ChangeState(LevelState newState)
    {
        currentState = newState;

        // Possibly notify UI & other components of the state change?
    }

    private void Update()
    {
        switch (currentState) 
        {
            case LevelState.Initialize:
                LoadLevel(GameManager.Instance.GetCurrentLevel());
                break;
            case LevelState.Gameplay:
                HandleGameplay();
                break;
            case LevelState.Win:
                HandleWinCondition();
                break;
        }
    }

    public void LoadLevel(LevelData levelData)
    {
        currentLevel = levelData;
        currentLevelGrid = levelGenerator.GenerateLevel(levelData);
        ChangeState(LevelState.Gameplay);
    }

    private void HandleGameplay()
    {
        if (CheckIfSolved())
        {
            ChangeState(LevelState.Win);
        }
    }

    private void HandleWinCondition()
    {
        int width = currentLevel.sourceImage.width;
        int height = currentLevel.sourceImage.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                currentLevelGrid[x, y].DestroyCell();
            }
        }

        Destroy(levelGenerator.GetRowClueParent().gameObject);
        Destroy(levelGenerator.GetColumnClueParent().gameObject);
        Destroy(levelGenerator.GetGridLinesParent().gameObject);

        UIManager.Instance.ShowWinUI();
        ChangeState(LevelState.Cleanup);
    }

    private bool CheckIfSolved()
    {
        // if we don't already have a solution, find it
        if (solution == null || solution.Length == 0)
        {
            solution = FindSolution();
        }

        int width = currentLevel.sourceImage.width;
        int height = currentLevel.sourceImage.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PicrossCellState currentCellState = currentLevelGrid[x, y].GetState();

                if (currentCellState != solution[x, y])
                {
                    if (solution[x, y] == PicrossCellState.Marked && currentCellState == PicrossCellState.Empty)
                    {
                        continue;
                    }

                    return false;
                }
            }
        }

        return true;
    }

    private PicrossCellState[,] FindSolution()
    {
        int levelWidth = currentLevel.sourceImage.width;
        int levelHeight = currentLevel.sourceImage.height;

        PicrossCellState[,] solution = new PicrossCellState[levelWidth, levelHeight];

        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                Color pixelColor = currentLevel.sourceImage.GetPixel(x, y);
                bool isFilled = (pixelColor.a > 0.1f && pixelColor.r == 0 && pixelColor.g == 0 && pixelColor.b == 0);

                if (isFilled)
                {
                    solution[x, y] = PicrossCellState.Filled;
                }
                else
                {
                    solution[x, y] = PicrossCellState.Marked;
                }
            }
        }

        return solution;
    }
}
