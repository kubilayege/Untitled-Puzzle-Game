using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#region types
public enum Animal
{
    None,
    Alpaca,
    BoarB,
    BuffaloA,
    Camel_B,
    Chicken,
    Cow,
    CrocodileA,
    DogB,
    Flamingo,
    GoatB,
    Gorilla_B,
    Horse,
    Kangaroo,
    OwlA,
    Pig,
    SheepB
}

public enum MissionBlocker
{
    None,
    Grass,
    Ice,
    Jelly
}

public enum LevelBlocker
{
    None,
    GreyMetall,
    Wood_ikili,
    Wood_Uclu
}
//@THEME FOR PARTS
public enum PartTheme
{
    Blue,
    Green,
    Pink,
    Purple,
    Yellow
}

public enum BlockerType
{
    Breakable,
    Unbreakable
}
#endregion

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    // Cells her pozisyondaki grid bloklarını tutan bir dictionary
    // Key: Tuple (x , y) -> Pozisyon, Value: CellManager -> cellData scripti referansı
    public static Dictionary<Tuple<int, int>, CellManager> cells;
    public static Dictionary<Tuple<int, int>, CellManager> lastEmptyCells;
    [SerializeField]
    public static Player.States playerState;
    public static GameObject holdingObject;
    public static GameObject holdingBooster;
    public GameObject holdingAnimal;
    public Shape lastPlacedShape;
    public List<int> lastMissionInfo;
    public GameObject partPrefab;
    public List<Vector3> spawnPositions;
    public List<GameObject> currentShapes;
    public List<SpriteRenderer> candidateCells;
    public TextAsset jsonFile;
    public TextAsset modesJSON;
    public List<Vector3> animalpositions;

    public static int emptyCellSlots;

    public float SCALELOSSOFPARTS;
    public enum GameStates
    {
        WAIT_FOR_PLAY,
        PLAY,
        SUCCESS_FINISH,
        FAIL_FINISH
    }
    public GameStates gameStates;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        defaultModes = JsonUtility.FromJson<ModeModel>(modesJSON.text);

        candidateCells = new List<SpriteRenderer>();
        lastEmptyCells = new Dictionary<Tuple<int, int>, CellManager>();

        lastColToHighlight = new List<int>();
        lastRowToHighlight = new List<int>();
        lastSquareToHighlight = new List<Tuple<int, int>>();
    }

    void Start()
    {
        GameInit();
        Player.onPlayerTryHold.AddListener(TryHoldingObject);
        Player.PlayerHolding.AddListener(MoveHoldingObject);
        Player.onPlayerRelease.AddListener(ReleaseHoldingObject);
        partCoroutines = new List<Coroutine>();
    }

    public void StartGame()
    {
        ChangeGameStates(GameStates.PLAY);
    }
    public void GameInit()
    {
        // PlayerDataController.SetDefaultDataForFirstTime(jsonFile.text);

        SetAspectRatio();
        StartGame();
        #region Examples
        //###########################################################################
        //################## Veri KAydetme ve Çekme Örnekleri #######################
        //###########################################################################

        //*Düz Veri Çekme*
        //      => PlayerDataController.data.isVibrationOpen;
        //      => PlayerDataController.GetData<bool>("isVibrationOpen");

        //*Düz Veri Kaydetme*
        //      => PlayerDataController.SaveData("isVibrationOpen", false);           => Key-Value mantığı ile kaydetme işlemini yapar.
        //      => PlayerDataController.SaveData("levelNum", 21);                     => Key-Value mantığı ile kaydetme işlemini yapar.

        //*List Verisi Çekme*
        //      => PlayerDataController.data.GetVideoReward("gold");                  => gold sonunda verilecek reward verisini döner;
        //      => PlayerDataController.data.GetThemePrice(0, "item1");               => temanın ücretini döner;  (whichTheme(ilk parametre) 0 ya da 1 olabilir)
        //      => PlayerDataController.data.GetThemeState(1, "item3");               => temanın durumunu(Açık mı kapalı mı) döner;  (whichTheme(ilk parametre) 0 ya da 1 olabilir)

        //*List Veri Kaydetme*
        //      => PlayerDataController.ChangeValueInList("theme1", "item5", true);   => tema listindeki "item5" name'ine sahip bir alanı değiştirir.
        //      => PlayerDataController.ChangeValueInList("videoAds", "gold", 124);   => videoAds listindeki "gold" name'ine sahip bir alanı değiştirir.

        //###########################################################################
        //########################## Reklam Method ları #############################
        //###########################################################################

        //      => AdManager.instance.ShowRewarded(whichTheme, onAdState, onAdComplete);
        //      => AdManager.instance.ShowIntersititial(whichTheme, onAdState, onAdComplete);
        //
        //Örnek;
        //
        //      => AdManager.instance.ShowRewarded("gold", onAdState, onAdComplete);
        //
        //public void onAdState(string whichAd, AdManager.AD_STATES state)
        //{
        //    if (whichAd == "continue")
        //    {
        //        if (state == AdManager.AD_STATES.READY)
        //            Debug.Log(whichAd + " Reklamı Hazır ve izleniyor!");
        //        else
        //        {
        //            Debug.Log(whichAd + " Reklamı Hazir Degil!");
        //            onClickNoThanks();
        //        }
        //    }
        //    else if (whichAd == "continue")
        //    {
        //        if (state == AdManager.AD_STATES.READY)
        //            Debug.Log(whichAd + " Reklamı Hazır ve izleniyor!");
        //        else
        //            Debug.Log(whichAd + " Reklamı Hazir Degil!");
        //    }
        //    else
        //    {
        //        if (state == AdManager.AD_STATES.READY)
        //            Debug.Log("Reklam Hazır ve izleniyor!");
        //        else
        //            Debug.Log("Reklam Hazır Degil!");
        //    }
        //}
        //
        //Video ve Interstitial reklamların kullanımlarını UIManager içindeki örneklerden inceleyebilirsiniz.

        //###########################################################################
        //######################## Timer TimeSteps Method u #########################
        //###########################################################################

        //      => Timer.TimeSteps(totalTime,stepTime,onStart,onUpdate,onComplete);
        //
        //Örnek;
        //
        //      => StartCoroutine(Timer.TimeSteps(10,1,onStartEvent,onUpdateEvent,onCompleteEvent));

        //public void onStartEvent()
        //{
        //    //Do something on start
        //}

        //public void onUpdateEvent(float elapsedValue)
        //{
        //    //Do something on update
        //}

        //public void onCompleteEvent()
        //{
        //    //Do something on complete
        //}
        #endregion

        ////Alttaki degiskenlerle oynayarak mode u degistirebilirsin.
        //_moveCount = 0; //Grid sistemine eklenilen kup sayisi. (Ornegin; yatay2 bir block yerlestirdin, _moveCount += 2; artması gerekiyor.)
        //_killCount = 0; //Grid sisteminde yok edilen kup sayisi. 
        //GameModeType = LevelManager.instance.activeLevel.GetComponent<LevelData>().startingMode;

    }

    private void SetAspectRatio()
    {
        float aspect = (float)Screen.width / (float)Screen.height; // Portrait
        //aspect = (float)Screen.width / (float)Screen.height; // Landscape
        Debug.Log(Camera.main.aspect);
        animalpositions = new List<Vector3>();
        spawnPositions = new List<Vector3>();
        if (aspect <= 0.52)
        {
            for (int i = 0; i < 3; i++)
            {
                animalpositions.Add(new Vector3(-4.15f + (i * 5.9f), 18.25f, 10f));
                spawnPositions.Add(new Vector3(-1.74f + (i * 3.74f), -15.0f, 0f));
            }
            Camera.main.orthographicSize = 21;
            Debug.Log("9:18"); // iPhone X                  
        }
        else // 16:9
        {
            for (int i = 0; i < 3; i++)
            {
                animalpositions.Add(new Vector3(-5.3f + (i * 7.1f), 16.25f, 10f));
                spawnPositions.Add(new Vector3(-2.29f + (i * 4.29f), -13.59f, 0f));
            }
            Camera.main.orthographicSize = 20;
            Debug.Log("16:9");
        }
    }
    public void ChangeGameStates(GameStates gmstts)
    {
        gameStates = gmstts;

        switch (gmstts)
        {
            case GameStates.WAIT_FOR_PLAY:

                UIManager.instance.OpenScreen((int)UIManager.Screens.MainScreen);

                break;
            case GameStates.PLAY:
                // UIManager.instance.CloseScreen((int)UIManager.Screens.MainScreen);
                if (!PlayerDataController.SetDefaultDataForFirstTime(jsonFile.text))
                {
                    Player.main.Gold = PlayerDataController.data.gold;
                    UIManager.instance.OpenScreen((int)UIManager.Screens.GameplayScreen);
                    LevelManager.instance.LoadLevel(false);
                    UIManager.instance.UpdateGoldText();
                }
                else
                {
                    Debug.Log(" ab'");
                    Player.main.Gold = PlayerDataController.data.gold;
                    UIManager.instance.OpenScreen((int)UIManager.Screens.GameplayScreen);
                    LevelManager.instance.InitializeTutorial();
                    UIManager.instance.UpdateGoldText();
                }
                break;
            case GameStates.SUCCESS_FINISH:
                if (LevelManager.instance.tm != null)
                {
                    LevelManager.instance.tm.LoadNextTutorial();
                }
                else
                {
                }
                break;
            case GameStates.FAIL_FINISH:
                if (AdManager.instance.IsRewardedReady())
                {
                    StartCoroutine(Timer.TimeSteps(2, 1, () => { }, (x) => { }, () =>
                    {
                        UIManager.instance.OpenScreen((int)UIManager.Screens.Failscreen);
                        UIManager.instance.StartContinueCountdown();
                    }));
                }
                else
                    UIManager.instance.onClickNoThanks();
                break;
            default:
                break;
        }
    }
    public void UndoGrid()
    {
        emptyCellSlots = 0;
        if (lastPlacedShape == null)
            return;

        foreach (var cell in cells)
        {
            if (lastEmptyCells.ContainsKey(cell.Key))
            {
                cell.Value.isFull = false;
                LevelManager.instance.SaveBlockStatusChange(cell.Value, false);
                Destroy(cell.Value.fruitOnTop);
                emptyCellSlots++;
                _moveCount--;
            }
            else
            {
                if (!cell.Value.isFull)
                {
                    SpawnPart(cell.Value);
                    LevelManager.instance.SaveBlockStatusChange(cell.Value, true);
                    _killCount--;
                }
            }
        }
        if (getNextShapeCor != null || currentShapes.Count == 3)
        {
            StopCoroutine(getNextShapeCor);
            currentShapes.ForEach((x) => Destroy(x));
            currentShapes.Clear();
            currentShapes.Capacity = 0;
        }

        int i = 0;
        foreach (var mission in LevelManager.instance.activeLevelData.missions)
        {
            mission.RewindMissionCount(lastMissionInfo[i]);
            i++;
        }
        Debug.Log(Player.main.comboCount + " <==   " + Player.main.lastComboCount);
        int goldRewingAmount = Player.main.goldAccuiredThisLevel - Player.main.lastGainedAmount;
        Player.main.comboCount = Player.main.lastComboCount;
        Player.main.breakCount = Player.main.lastBreakCount;
        Player.main.goldAccuiredThisLevel = Player.main.lastGainedAmount;
        CheckEmptyCellCount();
        Player.main.UpdatePlayerGold(-100 - goldRewingAmount);
        CheckMode();
        SpawnShape(lastPlacedShape.id, lastPlacedShape.spawnIndex);
        Destroy(lastPlacedShape.gameObject);
        LevelManager.instance.SaveShapes();
        CanShapesFit();

    }


    public void SpawnPart(CellManager cell)
    {
        SpriteRenderer sr = Instantiate(partPrefab, new Vector3(cell.transform.position.x, cell.transform.position.y, 0), Quaternion.identity, LevelManager.instance.activeLevelData.transform).GetComponent<SpriteRenderer>();
        sr.sprite = LevelManager.instance.activeLevelData.levelPartSprites[UnityEngine.Random.Range(0, LevelManager.instance.activeLevelData.levelPartSprites.Count)];
        sr.sortingOrder = 4;
        sr.transform.localScale = Vector3.one;
        cell.isFull = true;
        cell.fruitOnTop = sr.gameObject;
        Part part = sr.GetComponent<Part>();
        part.cellBellow = cell;
        part.isPlaced = true;
        part.GetComponent<BoxCollider2D>().enabled = false;
        emptyCellSlots--;
    }

    #region Object Handling
    private void TryHoldingObject()
    {
        float ratio = (Camera.main.pixelWidth / 20);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x + i * ratio,
                    Input.mousePosition.y + j * ratio));
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Camera.main.transform.forward);
                Debug.DrawRay(new Vector3(rayOrigin.x, rayOrigin.y, -3), Camera.main.transform.forward, Color.blue, 2f);
                if (hit.transform != null)
                {
                    if (hit.transform.TryGetComponent<Part>(out Part holdingPart))
                    {
                        if (!holdingPart.isPlaced)
                        {
                            holdingObject = hit.transform.gameObject;
                            holdingObject.transform.parent.position += Vector3.back;
                            StartCoroutine(holdingObject.transform.parent.gameObject.EaseSnap(
                                (Vector3.one * 2), 0.1f, true));
                            // if (parentCoroutine == null)
                            //     parentCoroutine = StartCoroutine(holdingObject.transform.parent.gameObject.EaseSnap(
                            //                                             Vector3.one * 1.2f * 1 / SCALELOSSOFPARTS, 0.1f, true));
                            // else
                            // {
                            //     StopCoroutine(parentCoroutine);
                            //     parentCoroutine = StartCoroutine(holdingObject.transform.parent.gameObject.EaseSnap(
                            //                                             Vector3.one * 1.2f * 1 / SCALELOSSOFPARTS, 0.1f, true));
                            // }
                            playerState = Player.States.Holding;
                            return;
                        }
                    }

                }
                else
                {
                    RaycastHit hit3D;
                    Vector3 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Physics.Raycast(origin, Camera.main.transform.forward, out hit3D))
                    {
                        Debug.Log(hit3D.transform.name);
                        if (hit3D.transform.CompareTag("Animal"))
                        {
                            foreach (var mission in LevelManager.instance.activeLevelData.missions)
                            {
                                if (hit3D.transform.gameObject == mission.animalModel)
                                {
                                    holdingAnimal = hit3D.transform.gameObject;
                                    playerState = Player.States.Holding;
                                    return;
                                }
                            }
                            holdingAnimal = null;
                        }
                    }
                }

            }
        }
    }
    private void RotateHoldingAnimal()
    {
        Debug.Log(holdingAnimal.name);
        holdingAnimal.transform.Rotate(0,
            (Input.GetAxis("Mouse X") * -100f * Time.deltaTime), 0);
    }

    private void MoveHoldingObject()
    {
        if (holdingObject == null)
        {
            if (holdingAnimal != null)
            {
                RotateHoldingAnimal();
            }
            return;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = holdingObject.transform.parent.position;
        float distance = 0f - mousePos.z;
        Vector3 offset = holdingObject.transform.position - holdingObject.transform.parent.position;
        Vector3 targetPos = new Vector3(mousePos.x - offset.x, mousePos.y - offset.y, -1);

        holdingObject.transform.parent.position = Vector3.Lerp(pos, targetPos + new Vector3(0, 4f, 0), 12 * Time.deltaTime);

        //Under
        if (IndicateCellsUnder())
            IndicateCellsThatCanPop();
    }

    public List<SpriteRenderer> highlightedCells;
    public List<int> lastRowToHighlight;
    public List<int> lastColToHighlight;
    public List<Tuple<int, int>> lastSquareToHighlight;
    private void IndicateCellsThatCanPop()
    {
        if (lastColToHighlight.Count != 0 || lastRowToHighlight.Count != 0 || lastSquareToHighlight.Count != 0)
            CleanLastHighlights();
        Dictionary<Tuple<int, int>, CellManager> popCells = new Dictionary<Tuple<int, int>, CellManager>();
        foreach (var cell in candidateCells)
        {
            CellManager cm = cell.GetComponent<CellManager>();
            Tuple<int, int> newCellIndex = new Tuple<int, int>(cm.index.x, cm.index.y);
            if (!popCells.ContainsKey(newCellIndex))
                popCells.Add(newCellIndex, cm);
        }
        FindEveryBlockToHighlight(popCells, out lastRowToHighlight, out lastColToHighlight, out lastSquareToHighlight);
        foreach (var rowIndex in lastRowToHighlight)
        {
            for (int i = 0; i < 9; i++)
            {
                Tuple<int, int> pos = new Tuple<int, int>(i, rowIndex);
                if (cells[pos].isFull && cells[pos].fruitOnTop != null)
                    cells[pos].fruitOnTop.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                cells[pos].GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }

        foreach (var colIndex in lastColToHighlight)
        {
            for (int i = 0; i < 9; i++)
            {
                Tuple<int, int> pos = new Tuple<int, int>(colIndex, i);
                if (cells[pos].isFull && cells[pos].fruitOnTop != null)
                    cells[pos].fruitOnTop.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                cells[pos].GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }

        foreach (var squareIndex in lastSquareToHighlight)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(squareIndex.Item1 + i, squareIndex.Item2 + j);
                    if (cells[pos].isFull && cells[pos].fruitOnTop != null)
                        cells[pos].fruitOnTop.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    cells[pos].GetComponent<SpriteRenderer>().color = Color.gray;
                }
            }
        }

    }

    private void CleanLastHighlights()
    {

        foreach (var rowIndex in lastRowToHighlight)
        {
            for (int i = 0; i < 9; i++)
            {
                Tuple<int, int> pos = new Tuple<int, int>(i, rowIndex);
                if (cells[pos].isFull && cells[pos].fruitOnTop != null)
                    cells[pos].fruitOnTop.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                if (!candidateCells.Contains(cells[pos].GetComponent<SpriteRenderer>()))
                    cells[pos].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        foreach (var colIndex in lastColToHighlight)
        {
            for (int i = 0; i < 9; i++)
            {
                Tuple<int, int> pos = new Tuple<int, int>(colIndex, i);
                if (cells[pos].isFull && cells[pos].fruitOnTop != null)
                    cells[pos].fruitOnTop.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                if (!candidateCells.Contains(cells[pos].GetComponent<SpriteRenderer>()))
                    cells[pos].GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        foreach (var squareIndex in lastSquareToHighlight)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(squareIndex.Item1 + i, squareIndex.Item2 + j);
                    if (cells[pos].isFull && cells[pos].fruitOnTop != null)
                        cells[pos].fruitOnTop.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    if (!candidateCells.Contains(cells[pos].GetComponent<SpriteRenderer>()))
                        cells[pos].GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        lastColToHighlight.Clear();
        lastColToHighlight.Capacity = 0;
        lastRowToHighlight.Clear();
        lastRowToHighlight.Capacity = 0;
        lastSquareToHighlight.Clear();
        lastSquareToHighlight.Capacity = 0;
    }

    private bool IndicateCellsUnder()
    {
        List<SpriteRenderer> tempCandidates = new List<SpriteRenderer>();
        SpriteRenderer sr;
        bool cantSnap = false;
        bool isNew = false;
        foreach (var part in holdingObject.transform.parent.GetComponentsInChildren<Part>())
        {
            CellManager cm;
            if (part.gameObject.CastsRayToGet(out cm))
            {
                sr = cm.GetComponent<SpriteRenderer>();
                if (!candidateCells.Contains(sr))
                {
                    isNew = true;
                }
                if (cm.isFull || cm.blocked)
                {
                    cantSnap = true;
                }
                tempCandidates.Add(sr);
            }
            else
            {
                foreach (var partO in holdingObject.transform.parent.GetComponentsInChildren<Part>())
                {
                    partO.transform.localScale = Vector3.Lerp(partO.transform.localScale, new Vector3(0.50f, 0.50f, 0.50f), 20 * Time.deltaTime);
                }
                // Clean last cells under
                CleanCellHighlight();
                CleanLastHighlights();
                return false;
            }
        }
        // Leave When no new cell under
        if (cantSnap)
        {
            tempCandidates.Clear();
            tempCandidates.Capacity = 0;
            foreach (var partO in holdingObject.transform.parent.GetComponentsInChildren<Part>())
            {
                partO.transform.localScale = Vector3.Lerp(partO.transform.localScale, new Vector3(0.50f, 0.50f, 0.50f), 15 * Time.deltaTime);
            }
        }
        else
        {
            foreach (var partO in holdingObject.transform.parent.GetComponentsInChildren<Part>())
            {
                partO.transform.localScale = Vector3.Lerp(partO.transform.localScale, new Vector3(0.40f, 0.40f, 0.40f), 15 * Time.deltaTime);
            }
        }
        if (!isNew)
            return false;

        CleanCellHighlight();

        // Paint cells under
        foreach (var temp in tempCandidates)
        {
            candidateCells.Add(temp);
            temp.color = Color.gray;
        }
        return true;
    }
    MaterialPropertyBlock _propertyBlock;
    private void CleanCellHighlight()
    {
        foreach (var cell in candidateCells)
        {
            cell.color = Color.white;
        }
        candidateCells.Clear();
        candidateCells.Capacity = 0;

    }
    public List<Coroutine> partCoroutines;
    public Coroutine parentCoroutine;
    private void ReleaseHoldingObject()
    {
        if (holdingAnimal != null)
        {
            StartCoroutine(holdingAnimal.EaseRotate(Quaternion.Euler(8, -165, 0), 0.5f));
            Debug.Log(holdingAnimal.name);
            holdingAnimal = null;
            playerState = Player.States.Empty;
        }

        if (holdingObject == null)
            return;

        playerState = Player.States.Empty;
        Dictionary<GameObject, CellManager> placementCells = new Dictionary<GameObject, CellManager>();
        if (CheckGridCelssForPlacement(holdingObject.transform.parent.gameObject, ref placementCells))
        {
            SnapShape(placementCells);
            CheckAndDeleteRowsAndColoumns(placementCells);
        }
        else
        {
            // if (partCoroutines.Count != 0 && partCoroutines[0] != null)
            // {
            //     foreach (var cor in partCoroutines)
            //     {
            //         StopCoroutine(cor);
            //     }
            //     partCoroutines.Clear();
            //     partCoroutines.Capacity = 0;
            //     foreach (var partholding in holdingObject.transform.parent.GetComponentsInChildren<Part>())
            //     {
            //         partCoroutines.Add(StartCoroutine(partholding.gameObject.EaseSnap(new Vector3(0.5f, 0.5f, 0.5f), 0.1f, false)));
            //     }
            // }
            // else
            // {
            //     partCoroutines.Clear();
            //     partCoroutines.Capacity = 0;
            foreach (var partholding in holdingObject.transform.parent.GetComponentsInChildren<Part>())
            {
                partCoroutines.Add(StartCoroutine(partholding.gameObject.EaseSnap(new Vector3(0.5f, 0.5f, 0.5f), 0.1f, false)));
            }
            // }
            StartCoroutine(holdingObject.transform.parent.gameObject.EaseSnap(holdingObject.transform.parent.GetComponent<Shape>().SpawnPos,
                Vector3.one * 2 * SCALELOSSOFPARTS, 0.2f, true));
            // if (parentCoroutine == null)
            //     parentCoroutine = StartCoroutine(holdingObject.transform.parent.gameObject.EaseSnap(holdingObject.transform.parent.GetComponent<Shape>().SpawnPos,
            //                                             Vector3.one * 2 * SCALELOSSOFPARTS, 0.2f, true));
            // else
            // {
            //     StopCoroutine(parentCoroutine);
            //     parentCoroutine = StartCoroutine(holdingObject.transform.parent.gameObject.EaseSnap(holdingObject.transform.parent.GetComponent<Shape>().SpawnPos,
            //                                             Vector3.one * 2 * SCALELOSSOFPARTS, 0.2f, true));
            // }
            // bool TrashIt = false;
            // foreach (var part in holdingObject.transform.parent.GetComponentsInChildren<Part>())
            // {
            //     Transform im;
            //     if (part.gameObject.CastsRayToGet(out im))
            //     {
            //         if (im.CompareTag("Trash"))
            //         {

            //             Player.main.Trash(part.transform.parent.GetComponent<Shape>());
            //         }
            //     }
            // }
            //Debug.Log(holdingObject.transform.parent.GetComponent<Shape>().SpawnPos);
            // holdingObject.transform.parent.position = holdingObject.transform.parent.GetComponent<Shape>().SpawnPos;

            FlatHelper.Vibrate(FlatHelper.VibrationType.Failure);
        }

        CheckEmptyCellCount();
        CleanCellHighlight();
        CleanLastHighlights();
        holdingObject = null;
    }

    public void CheckEmptyCellCount()
    {
        Debug.Log(emptyCellSlots);
        if (emptyCellSlots < 30)
        {
            UIManager.instance.ChangeRainbowButtonState(true);
        }
        else
        {
            UIManager.instance.ChangeRainbowButtonState(false);
        }
    }

    public void ActivateRainbowEffect()
    {
        List<Mission> missionsToProgress = new List<Mission>();
        int brokencellCount = 0;
        foreach (var cell in cells)
        {
            if (cell.Value.isFull)
            {
                if (lastEmptyCells.ContainsKey(cell.Key))
                {
                    lastEmptyCells.Remove(cell.Key);
                }
                if (cell.Value.mission != null && cell.Value.mission.Count != 0)
                {
                    if (!missionsToProgress.Contains(cell.Value.mission))
                    {
                        //missionsToProgress.Add(LevelManager.instance.activeLevelData.missions[
                        //    LevelManager.instance.activeLevelData.missions.IndexOf(cells[pos].mission)
                        //    ]);
                        if (cell.Value.mission.Count != 0)
                            missionsToProgress.Add(LevelManager.instance.activeLevelData.missions.Find(x => x == cell.Value.mission));
                    }
                    cell.Value.mission.missionShouldProgress = true;
                }
                cell.Value.RemoveAndDeleteObjectOnTop();
                brokencellCount++;
            }
        }
        lastPlacedShape = null;
        CalculatePlayerCombo(Mathf.FloorToInt(brokencellCount / 9));
        foreach (var mission in missionsToProgress)
        {
            mission.missionShouldProgress = true;
            LevelManager.instance.SaveMissionProgress(mission);
        }
        Player.main.UpdatePlayerGold(-400);
        CheckEmptyCellCount();
        CanShapesFit();
    }

    #endregion
    // public static List<BlockerBlock> GetBlockersOnTheSides(List<BlockerBlock> blockers, Tuple<int, int> pos)
    // {
    //     List<Tuple<int, int>> offsets = new List<Tuple<int, int>> {
    //         new Tuple<int, int>(1, 0),
    //         new Tuple<int, int>(-1, 0),
    //         new Tuple<int, int>(0, 1),
    //         new Tuple<int, int>(0, -1),
    //     };
    //     foreach (var offset in offsets)
    //     {
    //         Tuple<int, int> cpos = new Tuple<int, int>(pos.Item1 + offset.Item1, pos.Item2 + offset.Item2);
    //         if (cells.TryGetValue(cpos, out CellManager breakableCell))
    //         {
    //             if (breakableCell.blockerBlock != null && breakableCell.blockerBlock.blockerType == BlockerType.Breakable)
    //             {
    //                 if (!blockers.Contains(breakableCell.blockerBlock))
    //                 {
    //                     blockers.Add(breakableCell.blockerBlock);
    //                 }
    //             }
    //         }
    //     }

    //     return blockers;

    // }

    #region placing Object
    public void CheckAndDeleteRowsAndColoumns(Dictionary<GameObject, CellManager> placementCells)
    {
        List<int> rowToDelete, colToDelete;
        List<Tuple<int, int>> squareToDelete;
        FindEveryBlockToDelete(placementCells, out rowToDelete, out colToDelete, out squareToDelete);
        List<Mission> missionsToProgress = new List<Mission>();

        int brokenCount = rowToDelete.Count + colToDelete.Count + squareToDelete.Count;
        CalculatePlayerCombo(brokenCount);

        // List<BlockerBlock> breakableBlocks = new List<BlockerBlock>();
        foreach (var squareIndex in squareToDelete)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(squareIndex.Item1 + i, squareIndex.Item2 + j);

                    //GetBlockersOnTheSides(breakableBlocks, pos);

                    _killCount++;

                    if (cells[pos].mission != null)
                    {
                        if (!missionsToProgress.Contains(cells[pos].mission))
                        {
                            //missionsToProgress.Add(LevelManager.instance.activeLevelData.missions[
                            //    LevelManager.instance.activeLevelData.missions.IndexOf(cells[pos].mission)
                            //    ]);
                            if (cells[pos].mission.Count != 0)
                                missionsToProgress.Add(LevelManager.instance.activeLevelData.missions.Find(x => x == cells[pos].mission));
                        }
                    }

                    if (cells[pos].RemoveAndDeleteObjectOnTop())
                    {
                        // return;
                    }
                }
            }
            FlatHelper.Vibrate(FlatHelper.VibrationType.Success);
        }

        // foreach (var block in breakableBlocks)
        // {
        //     block.Break();
        // }

        DeleteRows(rowToDelete, missionsToProgress);
        DeleteColoumns(colToDelete, missionsToProgress);

        foreach (var mission in missionsToProgress)
        {
            mission.missionShouldProgress = true;
            LevelManager.instance.SaveMissionProgress(mission);
        }

        if (LevelManager.instance.tm == null && getNextShapeCor == null && CanShapesFit())
            UIManager.instance.FailLevel();

        CheckMode();
        if (LevelManager.instance.tm == null)
            LevelManager.instance.SaveShapes();
    }

    public void CalculatePlayerCombo(int brokenCount)
    {
        Player.main.lastComboCount = Player.main.comboCount;
        Player.main.lastBreakCount = Player.main.breakCount;
        Player.main.lastGainedAmount = Player.main.goldAccuiredThisLevel;

        if (brokenCount != 0)
            Player.main.breakCount++;
        else
            Player.main.ResetCombo();

        if (brokenCount >= 2)
            Player.main.comboCount = (int)Mathf.Pow(3, brokenCount - 2);

        if (brokenCount > 0 && Player.main.breakCount != 0)
            Player.main.comboCount += Player.main.breakCount - 1;

        int amount = (brokenCount * 3) + Player.main.comboCount;
        Player.main.goldAccuiredThisLevel += amount;
        PlayerDataController.SaveData("goldAccuiredThisLevel", Player.main.goldAccuiredThisLevel);
        if (amount != 0)
            UIManager.instance.AnimateGoldGain(amount, Player.main.comboCount);
        Player.main.UpdatePlayerGold((brokenCount * 3) + Player.main.comboCount);
    }

    public bool CanShapesFit()
    {
        bool canAnyFit = true;
        foreach (var shape in currentShapes)
        {
            if (shape.GetComponent<Shape>().CanFit())
            {
                foreach (var part in shape.GetComponentsInChildren<SpriteRenderer>())
                {
                    part.color = new Color(1f, 1f, 1f, 1f);
                }
                canAnyFit = false;
            }
            else
            {
                foreach (var part in shape.GetComponentsInChildren<SpriteRenderer>())
                {
                    part.color = new Color(1f, 1f, 1f, 0.6f);
                }
            }
        }
        return canAnyFit;
    }

    private void DeleteColoumns(List<int> colToDelete, List<Mission> mtp)
    {
        // List<BlockerBlock> breakableBlocks = new List<BlockerBlock>();
        foreach (var colIndex in colToDelete)
        {
            for (int i = 0; i < 9; i++)
            {
                Tuple<int, int> pos = new Tuple<int, int>(colIndex, i);

                // GetBlockersOnTheSides(breakableBlocks, pos);

                if (cells[pos].mission != null)
                {
                    if (!mtp.Contains(cells[pos].mission))
                    {
                        //mtp.Add(LevelManager.instance.activeLevelData.missions[
                        //        LevelManager.instance.activeLevelData.missions.IndexOf(cells[pos].mission)
                        //        ]);
                        if (cells[pos].mission.Count != 0)
                            mtp.Add(LevelManager.instance.activeLevelData.missions.Find(x => x == cells[pos].mission));
                    }
                }
                if (cells[pos].RemoveAndDeleteObjectOnTop())
                {
                    // return;
                }
            }
            FlatHelper.Vibrate(FlatHelper.VibrationType.Success);
        }

        // foreach (var block in breakableBlocks)
        // {
        //     block.Break();
        // }
    }

    private void DeleteRows(List<int> rowToDelete, List<Mission> mtp)
    {
        //List<BlockerBlock> breakableBlocks = new List<BlockerBlock>();
        foreach (var rowIndex in rowToDelete)
        {
            for (int i = 0; i < 9; i++)
            {
                Tuple<int, int> pos = new Tuple<int, int>(i, rowIndex);

                //GetBlockersOnTheSides(breakableBlocks, pos);

                if (cells[pos].mission != null)
                {
                    if (!mtp.Contains(cells[pos].mission))
                    {
                        //mtp.Add(LevelManager.instance.activeLevelData.missions[
                        //        LevelManager.instance.activeLevelData.missions.IndexOf(cells[pos].mission)
                        //        ]);
                        if (cells[pos].mission.Count != 0)
                            mtp.Add(LevelManager.instance.activeLevelData.missions.Find(x => x == cells[pos].mission));
                    }
                }
                if (cells[pos].RemoveAndDeleteObjectOnTop())
                {
                    // return;
                }
            }

            FlatHelper.Vibrate(FlatHelper.VibrationType.Success);
        }

        // foreach (var block in breakableBlocks)
        // {
        //     block.Break();
        // }
    }

    private static void FindEveryBlockToDelete<T>(Dictionary<T, CellManager> placementCells, out List<int> rowToDelete, out List<int> colToDelete, out List<Tuple<int, int>> squareToDelete)
    {
        rowToDelete = new List<int>();
        colToDelete = new List<int>();
        squareToDelete = new List<Tuple<int, int>>();
        squareToDelete.Clear();
        squareToDelete.Capacity = 0;

        //Debug.Log("a" + placementCells.Count);
        foreach (var part in placementCells)
        {
            bool removeColmn = true;
            bool removeRow = true;
            bool removeSquare = true;
            int squareX = part.Value.index.x - (part.Value.index.x % 3);
            int squareY = part.Value.index.y - (part.Value.index.y % 3);
            //Debug.Log(squareX + squareY);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(squareX + i, squareY + j);
                    // if (removeSquare && cells[pos].blockerBlock != null)
                    // {
                    //     if (cells[pos].blockerBlock.blockerType == BlockerType.Unbreakable)
                    //         removeSquare = false;
                    // }
                    //Debug.Log(cells[pos].isFull.ToString() + "   " + part.Value.index.x + "," + part.Value.index.y);
                    if (removeSquare && ((!cells[pos].isFull || cells[pos].fruitOnTop == null)))
                    {
                        removeSquare = false;
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                // if (removeSquare && cells[new Tuple<int, int>(i, part.Value.index.y)].blockerBlock != null)
                // {
                //     if (cells[new Tuple<int, int>(i, part.Value.index.y)].blockerBlock.blockerType == BlockerType.Unbreakable)
                //         removeRow = false;
                // }
                // if (removeSquare && cells[new Tuple<int, int>(part.Value.index.x, i)].blockerBlock != null)
                // {
                //     if (cells[new Tuple<int, int>(part.Value.index.x, i)].blockerBlock.blockerType == BlockerType.Unbreakable)
                //         removeColmn = false;
                // }
                if (removeRow && ((!cells[new Tuple<int, int>(i, part.Value.index.y)].isFull || cells[new Tuple<int, int>(i, part.Value.index.y)].fruitOnTop == null)))
                {
                    removeRow = false;
                }

                if (removeColmn && ((!cells[new Tuple<int, int>(part.Value.index.x, i)].isFull || cells[new Tuple<int, int>(part.Value.index.x, i)].fruitOnTop == null)))
                {
                    removeColmn = false;
                }
            }

            if (removeSquare && !squareToDelete.Contains(new Tuple<int, int>(squareX, squareY)))
            {
                squareToDelete.Add(new Tuple<int, int>(squareX, squareY));
            }
            //Debug.Log(squareToDelete.Count.ToString() + removeSquare + "X: " + squareX + " " + "Y: " + squareY + "   " + part.Value.index.x + "," + part.Value.index.y);

            if (removeRow && !rowToDelete.Contains(part.Value.index.y))
                rowToDelete.Add(part.Value.index.y);

            if (removeColmn && !colToDelete.Contains(part.Value.index.x))
                colToDelete.Add(part.Value.index.x);

        }
    }

    private static void FindEveryBlockToHighlight(Dictionary<Tuple<int, int>, CellManager> placementCells, out List<int> rowToDelete, out List<int> colToDelete, out List<Tuple<int, int>> squareToDelete)
    {
        rowToDelete = new List<int>();
        colToDelete = new List<int>();
        squareToDelete = new List<Tuple<int, int>>();
        squareToDelete.Clear();
        squareToDelete.Capacity = 0;

        //Debug.Log("a" + placementCells.Count);
        foreach (var part in placementCells)
        {
            bool removeColmn = true;
            bool removeRow = true;
            bool removeSquare = true;
            int squareX = part.Value.index.x - (part.Value.index.x % 3);
            int squareY = part.Value.index.y - (part.Value.index.y % 3);
            //Debug.Log(squareX + squareY);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Tuple<int, int> pos = new Tuple<int, int>(squareX + i, squareY + j);
                    // if (removeSquare && cells[pos].blockerBlock != null)
                    // {
                    //     if (cells[pos].blockerBlock.blockerType == BlockerType.Unbreakable)
                    //         removeSquare = false;
                    // }
                    //Debug.Log(cells[pos].isFull.ToString() + "   " + part.Value.index.x + "," + part.Value.index.y);
                    if (removeSquare && ((!cells[pos].isFull || cells[pos].fruitOnTop == null) && !placementCells.ContainsKey(pos)))
                    {
                        removeSquare = false;
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                // if (removeSquare && cells[new Tuple<int, int>(i, part.Value.index.y)].blockerBlock != null)
                // {
                //     if (cells[new Tuple<int, int>(i, part.Value.index.y)].blockerBlock.blockerType == BlockerType.Unbreakable)
                //         removeRow = false;
                // }
                // if (removeSquare && cells[new Tuple<int, int>(part.Value.index.x, i)].blockerBlock != null)
                // {
                //     if (cells[new Tuple<int, int>(part.Value.index.x, i)].blockerBlock.blockerType == BlockerType.Unbreakable)
                //         removeColmn = false;
                // }

                if (removeRow && ((!cells[new Tuple<int, int>(i, part.Value.index.y)].isFull || cells[new Tuple<int, int>(i, part.Value.index.y)].fruitOnTop == null) && !placementCells.ContainsKey(new Tuple<int, int>(i, part.Value.index.y))))
                {
                    removeRow = false;
                }

                if (removeColmn && ((!cells[new Tuple<int, int>(part.Value.index.x, i)].isFull || cells[new Tuple<int, int>(part.Value.index.x, i)].fruitOnTop == null) && !placementCells.ContainsKey(new Tuple<int, int>(part.Value.index.x, i))))
                {
                    removeColmn = false;
                }
            }

            if (removeSquare && !squareToDelete.Contains(new Tuple<int, int>(squareX, squareY)))
            {
                squareToDelete.Add(new Tuple<int, int>(squareX, squareY));
            }
            //Debug.Log(squareToDelete.Count.ToString() + removeSquare + "X: " + squareX + " " + "Y: " + squareY + "   " + part.Value.index.x + "," + part.Value.index.y);

            if (removeRow && !rowToDelete.Contains(part.Value.index.y))
                rowToDelete.Add(part.Value.index.y);

            if (removeColmn && !colToDelete.Contains(part.Value.index.x))
                colToDelete.Add(part.Value.index.x);

        }
    }

    private void SnapShape(Dictionary<GameObject, CellManager> placementCells)
    {

        FlatHelper.Vibrate(FlatHelper.VibrationType.Light);
        lastPlacedShape = holdingObject.transform.parent.GetComponent<Shape>();
        //Geri almak için kaydedilen state
        foreach (var cell in cells)
        {
            if (cell.Value.isFull)
            {
                // MyTuples pos = new MyTuples(cell.Key.Item1, cell.Key.Item2);
                // if (PlayerDataController.data.uncompletedLevel.ContainsTuple(ref pos))
                // {
                //     PlayerDataController.data.uncompletedLevel.Remove(pos);

                // }
                lastEmptyCells.Remove(cell.Key);
            }
            else
            {
                if (!lastEmptyCells.ContainsKey(cell.Key))
                {
                    lastEmptyCells.Add(cell.Key, cell.Value);
                }
            }
        }
        lastMissionInfo.Clear();
        lastMissionInfo.Capacity = 0;
        foreach (var mission in LevelManager.instance.activeLevelData.missions)
        {
            lastMissionInfo.Add(mission.Count);
        }

        foreach (var placement in placementCells)
        {
            StartCoroutine(placement.Key.EaseSnap(placement.Value.transform.position, new Vector3(0.5f, 0.5f, 0.5f), 0.05f, false));
            placement.Key.GetComponent<Part>().isPlaced = true;
            placement.Key.GetComponent<Part>().cellBellow = placement.Value;
            placement.Value.isFull = true; // bu satıra gerek olmayabilir
            placement.Value.fruitOnTop = placement.Key; // parçanın cell üzerinde olduğunun atanması
            _moveCount++; // parçaların eklenmesi
            emptyCellSlots--;
            placement.Key.GetComponent<BoxCollider2D>().enabled = false;
            placement.Key.GetComponent<SpriteRenderer>().sortingOrder = 4;

            LevelManager.instance.SaveBlockStatusChange(placement.Value, true);
        }
        currentShapes.Remove(holdingObject.transform.parent.gameObject);

        if (LevelManager.instance.tm != null)
        {
            return;
        }


        Debug.Log(currentShapes.Count);
        if (currentShapes.Count == 0)
        {
            getNextShapeCor = StartCoroutine(GetNextShapes());
        }
        else
        {

            if (getNextShapeCor != null)
                StopCoroutine(getNextShapeCor);
            getNextShapeCor = null;
        }
    }

    public enum BoosterTypes
    {
        Row,
        Column,
        Bomb
    }

    public bool CheckGridCelssForPlacement(GameObject piece, ref Dictionary<GameObject, CellManager> checkCells)
    {
        for (int i = 0; i < piece.transform.childCount; i++)
        {
            Transform part = piece.transform.GetChild(i);
            RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(part.position, part.forward), 10);
            if (hit.collider != null)
            {
                CellManager cell = hit.transform.GetComponent<CellManager>();
                if (cell != null && !cell.isFull && !cell.blocked)
                    checkCells.Add(part.gameObject, cell);
            }
        }

        if (checkCells.Count == piece.transform.childCount)
        {
            return true;
        }

        return false;
    }

    public bool CheckForValidMove()
    {
        foreach (var shape in currentShapes)
        {
            if (shape.GetComponent<Shape>().CanFit())
                return true;
        }
        return false;
    }
    #endregion

    #region DifficultyMode Controller
    public List<string> ShapeTypes = new List<string> {
        "tek",
        "yatay2",
        "dik2",
        "yatay3",
        "dik3",
        "L",
        "tersL",
        "F",
        "tersF",
        "kare",
        "uzunL",
        "uzuntersF",
        "uzunF",
        "uzuntersL"
    };

    public List<string> currentShapeTypes;
    public enum ModeTypes
    {
        EASY,
        MEDIUM,
        HARD,
        HARDEST
    }

    public ModeTypes GameModeType;
    public SingleModeModel GameModeProperties;
    public ModeModel defaultModes;
    public int _moveCount;
    public static int _killCount;
    public List<GameObject> shapes;
    public Coroutine getNextShapeCor;

    public IEnumerator GetNextShapes()
    {
        currentShapes = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            int rand = UnityEngine.Random.Range(0, currentShapeTypes.Count);
            SpawnShape(rand, i);
            yield return new WaitForSeconds(0.2f);
        }
        if (CanShapesFit())
            UIManager.instance.FailLevel();

        LevelManager.instance.SaveShapes();
    }

    public void SpawnShape(int randIndex, int spawnIndex)
    {
        GameObject currentShape = (Instantiate(shapes[ShapeTypes.IndexOf(currentShapeTypes[randIndex % currentShapeTypes.Count])],
            spawnPositions[spawnIndex],
            Quaternion.identity, LevelManager.instance.activeLevelData.transform));
        currentShape.transform.localScale *= SCALELOSSOFPARTS;
        currentShapes.Add(currentShape);
        foreach (var part in currentShape.GetComponentsInChildren<SpriteRenderer>())
        {
            part.sprite = LevelManager.instance.activeLevelData.levelPartSprites[UnityEngine.Random.Range(0, LevelManager.instance.activeLevelData.levelPartSprites.Count)];
            part.sortingOrder = 5;
        }
        Shape sh = currentShape.GetComponent<Shape>();
        sh.id = randIndex % currentShapeTypes.Count;
        sh.spawnIndex = spawnIndex;
        sh.Init();
    }
    public void CheckMode()
    {
        switch (GameModeType)
        {
            case ModeTypes.EASY:
                if (_moveCount >= defaultModes.EASY.moveCount && (_killCount / _moveCount) * 100 >= defaultModes.EASY.success)
                {
                    //move to up level
                    GameModeProperties = defaultModes.EASY;
                    GameModeType = ModeTypes.MEDIUM;
                    _moveCount = 0;
                    _killCount = 0;
                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.MEDIUM.shapes);
                }
                else
                {
                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.EASY.shapes);
                }
                break;
            case ModeTypes.MEDIUM:
                if (_moveCount >= defaultModes.MEDIUM.moveCount && (_killCount / _moveCount) * 100 >= defaultModes.MEDIUM.success)
                {
                    //move to up level
                    GameModeProperties = defaultModes.HARD;
                    GameModeType = ModeTypes.HARD;
                    _moveCount = 0;
                    _killCount = 0;

                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.HARD.shapes);
                }
                else if (_moveCount >= defaultModes.MEDIUM.moveCount && (_killCount / _moveCount) * 100 <= defaultModes.MEDIUM.fail)
                {
                    //move to down level
                    GameModeProperties = defaultModes.EASY;
                    GameModeType = ModeTypes.EASY;
                    _moveCount = 0;
                    _killCount = 0;

                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.EASY.shapes);
                }
                else
                {
                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.MEDIUM.shapes);
                }
                break;
            case ModeTypes.HARD:
                if (_moveCount >= defaultModes.HARD.moveCount && (_killCount / _moveCount) * 100 >= defaultModes.HARD.success)
                {
                    //move to up level
                    GameModeProperties = defaultModes.HARDEST;
                    GameModeType = ModeTypes.HARDEST;
                    _moveCount = 0;
                    _killCount = 0;
                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.HARDEST.shapes);
                }
                else if (_moveCount >= defaultModes.HARD.moveCount && (_killCount / _moveCount) * 100 <= defaultModes.HARD.fail)
                {
                    //move to down level
                    GameModeProperties = defaultModes.MEDIUM;
                    GameModeType = ModeTypes.MEDIUM;
                    _moveCount = 0;
                    _killCount = 0;
                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.MEDIUM.shapes);
                }
                else
                {
                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.HARD.shapes);
                }
                break;
            case ModeTypes.HARDEST:
                if (_moveCount >= defaultModes.HARDEST.moveCount && (_killCount / _moveCount) * 100 <= defaultModes.HARDEST.fail)
                {
                    //move to down level
                    GameModeProperties = defaultModes.HARD;
                    GameModeType = ModeTypes.HARD;
                    _moveCount = 0;
                    _killCount = 0;
                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.HARD.shapes);
                }
                else
                {
                    currentShapeTypes.Clear();
                    currentShapeTypes.AddRange(defaultModes.HARDEST.shapes);
                }
                break;
        }

        PlayerDataController.SaveData("levelModIndex", (int)GameModeType);
        Debug.Log("Current Game Mode Type:" + GameModeType + "  kill count: " + _killCount);
    }
    #endregion

}