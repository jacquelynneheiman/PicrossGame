using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicrossCell : MonoBehaviour
{
    public PicrossCellState currentState = PicrossCellState.Empty;

    [Header("Visuals")]
    public Sprite emptySprite;
    public Sprite filledSprite;
    public Sprite markedSprite;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateCell(currentState);

        // For testing, we can set the current state in the inspector to make sure the 
        // state visuals update correctly
    }

    /// <summary>
    /// Sets the cell's state and updates the visuals to match
    /// </summary>
    /// <param name="newState"></param>
    public void UpdateCell(PicrossCellState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            default:                            // default will be empty
            case PicrossCellState.Empty:
                SetCellEmpty();
                break;
            case PicrossCellState.Filled:
                SetCellFilled();
                break;
            case PicrossCellState.Marked:
                SetCellMarked();
                break;
        }
    }

    private void OnMouseDown()
    {
        // Loop the state when we click, if on last state, go back to first state
        int currentStateIndex = (int)currentState;
        currentStateIndex++;

        if (currentStateIndex >= Enum.GetValues(typeof(PicrossCellState)).Length)
        {
            currentStateIndex = 0;
        }

        UpdateCell((PicrossCellState)currentStateIndex);
    }

    private void SetCellEmpty()
    {
        // update the visual to empty state
        spriteRenderer.sprite = emptySprite;
    }

    private void SetCellFilled()
    {
        // update the visual to filled state
        spriteRenderer.sprite = filledSprite;
    }

    private void SetCellMarked()
    {
        // update the visual to marked state
        spriteRenderer.sprite = markedSprite;
    }

    /// <summary>
    /// Returns the current state of the cell
    /// </summary>
    /// <returns></returns>
    public PicrossCellState GetState()
    {
        return currentState;
    }

    public void DestroyCell()
    {
        Destroy(this.gameObject);
    }
}

public enum PicrossCellState
{
    Empty,      // No state
    Filled,     // Marked as apart of the solution
    Marked      // Marked as NOT apart of the solution
}
