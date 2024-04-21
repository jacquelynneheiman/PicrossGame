using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Picross Cells")]
    public PicrossCell cellPrefab;
    public float cellSize;

    [Header("Clue Text")]
    public TextMeshPro clueTextPrefab;
    public Transform rowClueParent;
    public Transform columnClueParent;
    public float clueTextSize = 0.5f;

    [Header("Grid Lines")]
    public Material guideLineMaterial;
    public Material gridLinesMaterial;
    public Transform gridLinesParent;

    private Camera mainCamera;
    private float cameraPadding = 0.25f;

    private float tallestClue = 0f;
    private float longestClue = 0f;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public PicrossCell[,] GenerateLevel(LevelData levelData)
    {
        // Create the level grid
        int width = levelData.sourceImage.width;
        int height = levelData.sourceImage.height;

        float clueSizeX = CalculateClueSizeX(width) * clueTextSize;
        float clueSizeY = CalculateClueSizeY(height) * clueTextSize;

        PicrossCell[,] levelGrid = new PicrossCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0;  y < height; y++)
            {
                // instantiate the cell prefab into the world
                levelGrid[x, y] = Instantiate(cellPrefab, new Vector3(x * cellSize, y * cellSize, 0), Quaternion.identity);

                levelGrid[x, y].transform.SetParent(this.transform, true);
            }
        }

        // Generate the clues for the level & add them to the grid
        GenerateClues(levelData);

        // Add the grid lines to the grid
        DrawGridLines(width, height);

        // subtract the clue size since the are in the negative x
        float levelWidth = (width * cellSize) + clueSizeX;
        float levelHeight = (height * cellSize) + clueSizeY;

        // Zoom and move the camera so we can see the whole grid
        AdjustCameraZoom(levelWidth, levelHeight);
        CenterCameraOnLevel(levelWidth, levelHeight, -clueSizeX, 0f);

        return levelGrid;
    }

    private void DrawGridLines(int gridWidth, int gridHeight)
    {
        int sectionsX = Mathf.CeilToInt(gridWidth);
        int sectionsY = Mathf.CeilToInt(gridHeight);

        float lineThickness = 0.1f;

        for (int x = 0; x <= sectionsX; x++)
        {
            float xPos = x * cellSize;
            xPos = xPos > gridWidth * cellSize ? gridWidth * cellSize : xPos;

            // if it's the edge of the board, create a guideline
            if (x == sectionsX || x % 5 == 0)
            {
                // create a guide line
                CreateLine(new Vector3(xPos, 0, 0), new Vector3(xPos, gridHeight * cellSize), guideLineMaterial, 20, lineThickness);
                continue;
            }

            CreateLine(new Vector3(xPos, 0, 0), new Vector3(xPos, gridHeight * cellSize), gridLinesMaterial, 15, lineThickness);
        }

        for (int y = 0; y <= sectionsY; y++)
        {
            float yPos = y * cellSize;
            yPos = yPos > gridHeight * cellSize ? gridHeight * cellSize : yPos;

            if (y == sectionsY || y % 5 == 0)
            {
                CreateLine(new Vector3(0, yPos, 0), new Vector3(gridWidth * cellSize, yPos, 0), guideLineMaterial, 20, lineThickness);
                continue;

            }

            CreateLine(new Vector3(0, yPos, 0), new Vector3(gridWidth * cellSize, yPos, 0), gridLinesMaterial, 15, lineThickness);
        }
    }

    private void CreateLine(Vector3 start, Vector3 end, Material material, int sortOrder = 15, float width = 0.1f)
    {
        LineRenderer line = new GameObject("GridLine").AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        line.startWidth = width;
        line.endWidth = width;
        line.transform.parent = gridLinesParent;
        line.sortingOrder = sortOrder;
    }

    private void GenerateClues(LevelData levelData)
    {
        int gridWidth = levelData.sourceImage.width;
        int gridHeight = levelData.sourceImage.height;

        bool[,] grid = new bool[gridWidth, gridHeight];

        // Loop through the grid and capture the pixels that are in the puzzle
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Color pixelColor = levelData.sourceImage.GetPixel(x, y);
                grid[x, y] = (pixelColor.a > 0.1f && pixelColor.r == 0 && pixelColor.g == 0 && pixelColor.b == 0);
            }
        }

        List<List<int>> rowClues = GenerateCluesForRows(grid, gridWidth, gridHeight);
        List<List<int>> columnClues = GenerateCluesForColumns(grid, gridWidth, gridHeight);

        InstantiateClues(rowClues, rowClueParent, true);
        InstantiateClues(columnClues, columnClueParent, false);
    }

    private List<List<int>> GenerateCluesForRows(bool[,] grid, int gridWidth, int gridHeight)
    {
        List<List<int>> clues = new List<List<int>>();

        for (int y = 0; y < gridHeight; y++)
        {
            List<int> rowClue = new List<int>();
            int count = 0;

            for (int x = 0; x < gridWidth; x++)
            {
                // if the grid cell is filled, increase the count
                if (grid[x, y])
                {
                    count++;
                }
                else
                {
                    // if we have a count, add the count to the
                    // clues list and reset the counts

                    if (count > 0)
                    {
                        longestClue = (count > longestClue) ? count : longestClue;
                        rowClue.Add(count);
                        count = 0;
                    }
                }
            }

            // if we neded the loop through the row with a count, add it to the clues
            if (count > 0)
            {
                longestClue = (count > longestClue) ? count : longestClue;
                rowClue.Add(count);
            }

            // Add the clues for this row to the clues list
            clues.Add(rowClue);
        }

        return clues;
    }

    private List<List<int>> GenerateCluesForColumns(bool[,] grid, int gridWidth, int gridHeight)
    {
        List<List<int>> clues = new List<List<int>>();

        for (int x = 0; x < gridWidth; x++)
        {
            List<int> columnClues = new List<int>();
            int count = 0;

            for (int y = 0; y < gridHeight; y++)
            {
                // if the grid cell is filled
                if (grid[x, y])
                {
                    count++;
                }
                else
                {
                    // if we have consecutive filled grid cells
                    if (count > 0)
                    {
                        tallestClue = (count > tallestClue) ? count : tallestClue;
                        columnClues.Add(count);
                        count = 0;
                    }
                }
            }

            // if we neded the loop through the column with a count, add it to the clues
            if (count > 0)
            {
                tallestClue = (count > tallestClue) ? count : tallestClue;
                columnClues.Add(count);
            }

            // Add the column clues to the clues list
            clues.Add(columnClues);
        }

        return clues;
    }

    private void InstantiateClues(List<List<int>> clues, Transform parent, bool isRow)
    {
        float offset = isRow ? 0f : 0.25f;

        for (int i = 0; i < clues.Count; i++)
        {
            TextMeshPro clueText = Instantiate(clueTextPrefab, parent);
            string seperator = " ";

            if (!isRow)
            {
                clues[i].Reverse();
                seperator = "\n";
            }

            clueText.text = string.Join(seperator, clues[i]);

            Vector3 position = Vector3.zero;
            position.x = isRow ? (-cellSize / 2) : ((i * cellSize) + (cellSize / 2));
            position.y = isRow ? (i * cellSize) + (cellSize / 4) : (clues.Count * cellSize) + offset;

            clueText.transform.localPosition = position;

            if (!isRow)
            {
                clueText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            }
        }
    }

    private void AdjustCameraZoom(float width, float height)
    {
        float verticalSize = height  / 2 + cameraPadding;
        float horizontalSize = (width / 2 + cameraPadding) / mainCamera.aspect;

        mainCamera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }

    private void CenterCameraOnLevel(float width, float height, float xOffset, float yOffset)
    {
        Vector2 levelCenter = GetLevelCenter(width, height, xOffset, yOffset);
        mainCamera.transform.position = new Vector3(levelCenter.x, levelCenter.y, mainCamera.transform.position.z);
    }

    private Vector2 GetLevelCenter(float width, float height, float xOffset, float yOffset)
    {
        return new Vector2((width / 2f) + xOffset, (height / 2f) + yOffset);
    }

    private int CalculateClueSizeX(int width)
    {
        return Mathf.CeilToInt(width / 2f);
    }

    private int CalculateClueSizeY(int height)
    {
        return Mathf.CeilToInt(height / 2f);
    }

    public Transform GetRowClueParent()
    {
        return rowClueParent;
    }

    public Transform GetColumnClueParent()
    {
        return columnClueParent;
    }

    public Transform GetGridLinesParent()
    {
        return gridLinesParent;
    }
}
