using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject bonusLevel;
    public GameObject[] levels;
    public Text levelText;

    public GameObject[] tutorialLevels;
    public GameObject activeLevel;
    public LevelData activeLevelData;
    PartTheme lastPartTheme;
    public GameObject tutorialHand;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void LoadLevel(bool refreshLevelColors)
    {
        var realLevelNum = PlayerDataController.data.levelNum;
        var fakeLevelNum = (realLevelNum) % levels.Length;

        if (GameManager.instance.getNextShapeCor != null)
            GameManager.instance.StopCoroutine(GameManager.instance.getNextShapeCor);


        GameManager.emptyCellSlots = 81;
        Player.main.ApplyAndResetGoldInfo();
        if (activeLevel != null)
        {
            Destroy(activeLevel);
        }

        activeLevel = Instantiate(levels[fakeLevelNum]);
        activeLevelData = activeLevel.GetComponent<LevelData>();
        if (refreshLevelColors)
        {

            PartTheme newPartTheme = FlatHelper.GetRandomEnumType<PartTheme>();
            while (newPartTheme == lastPartTheme)
            {
                newPartTheme = FlatHelper.GetRandomEnumType<PartTheme>();
            }
            var loadedTheme = Resources.LoadAll("Parts/" + newPartTheme.ToString(), typeof(Sprite));
            lastPartTheme = newPartTheme;
            activeLevelData.partTheme = newPartTheme;
            activeLevelData.levelPartSprites = new List<Sprite>();
            foreach (var theme in loadedTheme)
            {
                activeLevelData.levelPartSprites.Add(theme as Sprite);
            }
            PlayerDataController.SaveData("levelPartTheme", (int)newPartTheme);
        }
        else
        {
            lastPartTheme = activeLevelData.partTheme;
        }

        GridConstructer.instance.Init(activeLevelData, false);
        PlayerDataController.SaveData("isBonusLevel", false);

        levelText.text = "LEVEL " + realLevelNum;

        if (PlayerDataController.data.isLevelCompleted)
        {
            PlayerDataController.ResetLevelData();
            foreach (var mission in activeLevelData.missions)
            {
                SaveMissionProgress(mission);
            }

            GameManager.instance.GameModeType = activeLevelData.startingMode;
            GameManager.instance.getNextShapeCor = GameManager.instance.StartCoroutine(GameManager.instance.GetNextShapes());
            GameManager.instance.CheckEmptyCellCount();
            UIManager.instance.OpenScreen((int)UIManager.Screens.LevelInfoUI);
        }
        else
        {
            LoadLevelDataFromSave();
        }






        // AdManager.instance.ShowIntersititial("", (name, state) =>
        // {
        //     //state check
        // }, (name, state) =>
        // {
        //     //complete check
        // });
    }

    public void PassNewLevel()
    {

        PlayerDataController.SaveData("isLevelCompleted", true);
        PlayerDataController.SaveData("levelNum", PlayerDataController.data.levelNum + 1);
        LoadLevel(false);
    }
    public TutorialManager tm;
    public void InitializeTutorial()
    {
        tm = new TutorialManager(tutorialLevels);
        UIManager.instance.CloseScreen((int)UIManager.Screens.GamePlayUI);
        tm.LoadNextTutorial();
    }
    public void SpawnTutorialHand()
    {
        tutorialHand.transform.position = new Vector3(6, -10, 0);
        tutorialHand.SetActive(true);
        StartCoroutine(tutorialHand.EaseMove(new Vector3(3, -14, 0), 1f, HoldAndMoveTutorialHand));
    }
    GameObject tempShape;
    private void HoldAndMoveTutorialHand()
    {
        tempShape = Instantiate(GameManager.instance.currentShapes[0], tutorialHand.transform.position, Quaternion.identity, tutorialHand.transform);
        tempShape.transform.position += new Vector3(-1f, 1f, 0f);
        tempShape.transform.localScale = Vector3.one * 0.01f;
        foreach (var part in tempShape.GetComponentsInChildren<SpriteRenderer>())
        {
            part.color = new Color(1f, 1f, 1f, 0.7f);
        }
        StartCoroutine(tempShape.EaseScale(Vector3.one, 0.3f, MoveHandToSpot));
    }
    private void MoveHandToSpot()
    {
        StartCoroutine(tempShape.EaseScale(Vector3.one * 0.8f, 0.6f, () => { }));
        StartCoroutine(tutorialHand.EaseMove(new Vector3(3f, 3.18f, 0f), 1f, () =>
        {
            tutorialHand.SetActive(false);
            Destroy(tempShape);

            GameManager.playerState = Player.States.Empty;
        }));
    }
    public void LoadLevelDataFromSave()
    {
        activeLevelData.partTheme = (PartTheme)Enum.GetValues(typeof(PartTheme)).GetValue(PlayerDataController.data.levelPartTheme);
        var loadedTheme = Resources.LoadAll("Parts/" + activeLevelData.partTheme.ToString(), typeof(Sprite));
        lastPartTheme = activeLevelData.partTheme;
        activeLevelData.levelPartSprites = new List<Sprite>();
        foreach (var theme in loadedTheme)
        {
            activeLevelData.levelPartSprites.Add(theme as Sprite);
        }


        foreach (var cell in PlayerDataController.data.uncompletedLevel)
        {
            if (cell.isFull)
            {
                GameManager.instance.SpawnPart(GameManager.cells[new Tuple<int, int>(cell.x, cell.y)]);
            }
        }
        int k = 0;
        bool isLevelCompleted = true;
        foreach (var mission in activeLevelData.missions)
        {
            string animalPlayerPrefsName = "animalProgress" + LevelManager.instance.activeLevelData.animalIndex[k];
            mission.RewindMissionCount(PlayerDataController.GetData<int>(animalPlayerPrefsName));
            k++;
            if (!mission.missionCompleted)
            {
                isLevelCompleted = false;
            }
        }
        GameManager.instance.GameModeType = (GameManager.ModeTypes)Enum.GetValues(typeof(GameManager.ModeTypes))
                                                                       .GetValue(PlayerDataController.data.levelModIndex);

        GameManager.instance.CheckMode();

        foreach (var shape in PlayerDataController.data.savedShapes)
        {
            GameManager.instance.SpawnShape(shape.x, shape.y);
        }

        Player.main.goldAccuiredThisLevel = PlayerDataController.data.goldAccuiredThisLevel;
        if (isLevelCompleted)
        {
            UIManager.instance.OpenScreen((int)UIManager.Screens.SuccessUI);
        }
        else
        {
            UIManager.instance.OpenScreen((int)UIManager.Screens.LevelInfoUI);
        }

        GameManager.instance.CheckEmptyCellCount();
        GameManager.instance.CanShapesFit();
    }

    public void SaveMissionProgress(Mission mission)
    {
        // int k = 0;
        // foreach (var mission in activeLevelData.missions)
        // {
        string animalPlayerPrefsName = "animalProgress" + LevelManager.instance.activeLevelData.animalIndex[activeLevelData.missions.IndexOf(mission)];
        PlayerDataController.SaveData(animalPlayerPrefsName, mission.Count);
        //     k++;
        // }
    }

    public void SaveBlockStatusChange(CellManager placement, bool placed)
    {
        MyTuples myTuple = PlayerDataController.data.uncompletedLevel.GetTuple(placement.index.x, placement.index.y);
        if (myTuple.isFull != placed)
        {
            myTuple.isFull = placed;

            PlayerDataController.SaveData("uncompletedLevel", FlatHelper.ToJson<MyTuples>(PlayerDataController.data.uncompletedLevel));
        }
    }

    public void SaveShapes()
    {
        PlayerDataController.data.savedShapes = new List<MyTuples>();
        foreach (var shape in GameManager.instance.currentShapes)
        {
            Shape shapeData = shape.GetComponent<Shape>();
            PlayerDataController.data.savedShapes.Add(new MyTuples(shapeData.id, shapeData.spawnIndex));
        }
        PlayerDataController.SaveData("savedShapes", FlatHelper.ToJson<MyTuples>(PlayerDataController.data.savedShapes));
    }
}
