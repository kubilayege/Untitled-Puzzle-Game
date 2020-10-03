using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TutorialManager
{
    GameObject[] tutorialLevels;
    GameObject currentLevel;
    LevelData activeLevelData;
    public int index;
    public TutorialManager(GameObject[] levels)
    {
        tutorialLevels = levels;
        index = 0;
    }

    public void LoadNextTutorial()
    {
        if (currentLevel != null)
        {
            Debug.Log("Destroying");
            MonoBehaviour.Destroy(currentLevel);
        }
        if (index > 3)
        {

            LevelManager.instance.tm = null;
            LevelManager.instance.LoadLevel(false);
            PlayerDataController.SaveData("isRefreshDataEveryLaunch", false);
            UIManager.instance.OpenScreen((int)UIManager.Screens.GamePlayUI);
            UIManager.instance.CloseScreen((int)UIManager.Screens.GoodJobUI);
            return;
        }

        UIManager.instance.CloseScreen((int)UIManager.Screens.GoodJobUI);
        currentLevel = MonoBehaviour.Instantiate(tutorialLevels[index]);
        // activeLevelData = currentLevel.GetComponent<LevelData>();
        LevelManager.instance.activeLevelData = currentLevel.GetComponent<LevelData>();
        LevelManager.instance.activeLevel = currentLevel;
        GridConstructer.instance.Init(LevelManager.instance.activeLevelData, true);

        InitCurrentTutorial();

    }

    public void InitCurrentTutorial()
    {
        GameManager.instance.CheckMode();
        //Spawn Shapes
        switch (index)
        {
            case 0:
                FillGridForSafety();
                for (int i = 0; i < 9; i++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(i, 5);
                    if (i != 4)
                    {
                        GameManager.instance.SpawnPart(GameManager.cells[pos]);
                    }
                    else
                    {
                        GameManager.cells[pos].isFull = false;
                    }
                }
                GameManager.instance.SpawnShape(0, 1);
                break;
            case 1:
                FillGridForSafety();
                for (int i = 0; i < 9; i++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(4, i);
                    if (i != 5)
                    {
                        GameManager.instance.SpawnPart(GameManager.cells[pos]);
                    }
                    else
                    {
                        GameManager.cells[pos].isFull = false;
                    }
                }

                GameManager.instance.SpawnShape(0, 1);
                break;
            case 2:
                FillGridForSafety();
                for (int i = 3; i < 6; i++)
                {
                    for (int j = 3; j < 6; j++)
                    {
                        Tuple<int, int> pos = new Tuple<int, int>(i, j);
                        if (i != 4 || j != 5)
                        {
                            GameManager.instance.SpawnPart(GameManager.cells[pos]);
                        }
                        else
                        {
                            GameManager.cells[pos].isFull = false;
                        }
                    }
                }

                GameManager.instance.SpawnShape(0, 1);
                break;
            case 3:
                FillGridForSafety();
                for (int i = 3; i < 6; i++)
                {
                    for (int j = 3; j < 6; j++)
                    {
                        Tuple<int, int> pos = new Tuple<int, int>(i, j);
                        if (i != 4 || j != 5)
                        {
                            GameManager.instance.SpawnPart(GameManager.cells[pos]);
                        }
                        else
                        {
                            GameManager.cells[pos].isFull = false;
                        }
                    }
                }
                for (int i = 3; i < 6; i++)
                {
                    for (int j = 6; j < 9; j++)
                    {
                        Tuple<int, int> pos = new Tuple<int, int>(i, j);
                        if (i != 4 || j != 6)
                        {
                            GameManager.instance.SpawnPart(GameManager.cells[pos]);
                        }
                        else
                        {
                            GameManager.cells[pos].isFull = false;
                        }
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(4, i);
                    GameManager.instance.SpawnPart(GameManager.cells[pos]);
                }
                for (int i = 0; i < 3; i++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(i, 5);
                    GameManager.instance.SpawnPart(GameManager.cells[pos]);
                }
                for (int i = 0; i < 3; i++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(i, 6);
                    GameManager.instance.SpawnPart(GameManager.cells[pos]);
                }
                for (int i = 6; i < 9; i++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(i, 5);
                    GameManager.instance.SpawnPart(GameManager.cells[pos]);
                }
                for (int i = 6; i < 9; i++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(i, 6);
                    GameManager.instance.SpawnPart(GameManager.cells[pos]);
                }

                GameManager.instance.SpawnShape(2, 1);
                break;
            default:
                break;
        }
        index++;
        LevelManager.instance.SpawnTutorialHand();
        //Spawn Current Shape
    }

    private void FillGridForSafety()
    {
        foreach (var cell in GameManager.cells)
        {
            cell.Value.isFull = true;
        }
    }
}
