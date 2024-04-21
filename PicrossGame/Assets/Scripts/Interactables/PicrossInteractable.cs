using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicrossInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private LevelData levelData;

    public void Interact()
    {
        GameManager.Instance.StartPicrossLevel(levelData);
    }
}
