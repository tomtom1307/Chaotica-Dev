using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class PortalMapUI : MonoBehaviour
{
    public GameObject canvas;
    public GameObject buttonPrefab;
    public List<LevelData> levels = new List<LevelData>();
    public Transform levelButtons;
    public int rowCapacity = 5;

    private List<Transform> levelButtonRows;

    void Start()
    {
        //List transform of each Level Button Row
        levelButtonRows = new List<Transform>(3);
        for (int i = 0; i < levelButtons.childCount; i++)
        {
            levelButtonRows[i] = levelButtons.GetChild(i);
        }
    }

    public void OpenMap()
    {
        PopulateButtons();
        UIManager.instance.IsMenu(true, canvas);
        UIManager.instance.ShowCursor();
    }

    //Goes through SO Datas in the list
    void PopulateButtons()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            int rowIndex = i / rowCapacity;
            Debug.Log($"Instantiating Level {i} Row {rowIndex}");
            GameObject button = Instantiate(buttonPrefab, levelButtonRows[rowIndex]);
            button.GetComponent<LevelButton>().InitializeData(levels[i]);
        }
    }
}
