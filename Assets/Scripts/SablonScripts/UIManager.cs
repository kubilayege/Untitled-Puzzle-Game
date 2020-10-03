using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    public List<GameObject> goldSprites;
    public GameObject UICanvas;
    public GameObject TrashUI;
    GameObject[] AddedGoldSprites;
    Coroutine goldCoroutine;
    Coroutine randomSelectCoroutine;
    Coroutine continueProgCoroutine;

    public Sprite openThemeSprite;
    public Sprite closeThemeSprite;
    public Sprite selectedThemeSprite;
    public GameObject animatedSelectedTheme;

    public GameObject themes1;
    public GameObject themes2;
    public List<Sprite> Theme1Sprites;
    public List<Sprite> Theme2Sprites;

    Image[] theme1Images = new Image[0];
    Image[] theme2Images = new Image[0];

    public Text RandomButtonTheme1Text;
    public Text RandomButtonTheme2Text;
    public Text WatchButtonTheme1Text;
    public Text WatchButtonTheme2Text;

    public GameObject Theme1Mark;
    public GameObject Theme2Mark;

    public GameObject[] MenuScreens;
    public GameObject GameplayScreen;
    public GameObject GamePlayUI;

    public Text ContinueTitle;
    public Image ContinueProgressbar;
    public Text ContinueCountdownText;
    public GameObject NoThanksButton;

    public GameObject VibrateOpenIcon;
    public GameObject VibrateCloseIcon;
    public GameObject VibrateOpenRadioIcon;
    public GameObject VibrateCloseRadioIcon;

    Image lastSelectedTheme1;
    Image lastSelectedTheme2;

    public Image UIBackground;
    public GameObject Missions;
    public Text[] missionCounts;
    public GameObject missionInfoUI;

    public Text GoldText;
    public TMPro.TextMeshProUGUI comboText;
    public TMPro.TextMeshProUGUI goldGainText;
    public enum Screens
    {
        MainScreen,
        Settings,
        RainbowUI,
        GameplayScreen,
        Failscreen,
        TrashUI,
        LevelInfoUI,
        PauseUI,
        SuccessUI,
        ShopUI,
        GoodJobUI,
        GamePlayUI
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void InitUI()
    {

        //AddGolds();
        //UpdateGoldText();
        //RefreshTheTheme(0);
        //RefreshTheTheme(1);
        //MenuScreens[(int)Screens.Theme1].transform.Find("Title").gameObject.GetComponent<Text>().text = PlayerDataController.data.theme1Title;
        //MenuScreens[(int)Screens.Theme2].transform.Find("Title").gameObject.GetComponent<Text>().text = PlayerDataController.data.theme2Title;
        //animatedSelectedTheme.SetActive(false);
    }

    public void Undo()
    {
        if (Player.main.Gold < 100)
        {
            return;
        }
        GameManager.instance.UndoGrid();
    }

    public void Reload()
    {
        GamePlayUI.SetActive(true);
        CloseScreen((int)Screens.Failscreen);
        PlayerDataController.SaveData("isLevelCompleted", true);
        LevelManager.instance.LoadLevel(true);
    }
    public void FinishGoldAnimation()
    {
        goldGainText.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
        goldGainText.rectTransform.anchoredPosition3D = Vector3.zero;

    }
    public void EndGoldComboAnimate()
    {
        StartCoroutine(goldGainText.EaseScale(new Vector3(0.01f, 0.01f, 0.01f), 0.4f, FinishGoldAnimation));
        StartCoroutine(goldGainText.EaseMove(GoldText.transform.position, 0.4f, () => { }));

        iTween.PunchScale(GoldText.gameObject, new Vector3(0.27f, 0.27f, 0.27f), 0.6f);
    }
    public void EndGoldAnimate()
    {
        StartCoroutine(goldGainText.EaseScale(new Vector3(0.01f, 0.01f, 0.01f), 0.4f, FinishGoldAnimation));
        StartCoroutine(goldGainText.EaseMove(GoldText.transform.position, 0.4f, () => { }));

        iTween.PunchScale(GoldText.gameObject, new Vector3(0.27f, 0.27f, 0.27f), 0.6f);
    }
    public void AnimateGoldGain(int totalAmount, int comboCount)
    {
        goldGainText.text = "+" + totalAmount + " gold";
        if (comboCount >= 1)
        {
            Debug.Log(comboCount);
            goldGainText.gameObject.SetActive(true);
            comboText.gameObject.SetActive(true);
            StartCoroutine(goldGainText.EaseScale(new Vector3(4f, 4f, 4f), 0.6f, EndGoldComboAnimate));
        }
        else
        {
            Debug.Log(comboCount);
            goldGainText.gameObject.SetActive(true);
            goldGainText.transform.localScale = Vector3.one * 3f;
            StartCoroutine(goldGainText.EaseScale(new Vector3(4f, 4f, 4f), 0.6f, EndGoldAnimate));
        }
    }
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    public List<GameObject> buttonSlots;
    public void Trash()
    {
        if (Player.main.Gold < 100)
        {

            TrashUI.gameObject.SetActive(false);
            return;
        }


        GameManager.playerState = Player.States.UI;
        int i = 0;
        foreach (var button in buttonSlots)
        {
            button.SetActive(false);
            // Vector3 currentSpawnSlot = GameManager.instance.spawnPositions[i];
            // button.transform.position = new Vector3(currentSpawnSlot.x, currentSpawnSlot.y + 1f, button.transform.position.z);
            // i++;
        }
        foreach (var shape in GameManager.instance.currentShapes)
        {
            buttonSlots[shape.GetComponent<Shape>().spawnIndex].SetActive(true);
            Debug.Log(shape.GetComponent<Shape>().spawnIndex);
        }
    }


    public void SelectShapeToRecycle(int val)
    {
        Debug.Log(val);
        CloseScreen(5);
        foreach (var shape in GameManager.instance.currentShapes)
        {
            if (val == shape.GetComponent<Shape>().spawnIndex)
            {
                int index = shape.GetComponent<Shape>().id + UnityEngine.Random.Range(1, GameManager.instance.currentShapeTypes.Count - 1);
                Debug.Log("ID : " + shape.GetComponent<Shape>().id + " index : " + index + " indexModded : " + index % GameManager.instance.currentShapeTypes.Count);
                GameManager.instance.SpawnShape(shape.GetComponent<Shape>().id + UnityEngine.Random.Range(1, GameManager.instance.currentShapeTypes.Count - 1), val);
                GameManager.instance.currentShapes.Remove(shape.gameObject);
                Destroy(shape.gameObject);
                Player.main.UpdatePlayerGold(-100);
                if (GameManager.instance.CanShapesFit())
                    UIManager.instance.FailLevel();
                return;
            }
        }
    }

    // public void UICageActivate(int i)
    // {
    //     missionButtons[i].gameObject.SetActive(false);
    //     missionCounts[i].text = "";
    //     LevelManager.instance.activeLevelData.missions[i].BreakCage();
    // }
    public void RefreshTheTheme(int whichTheme)
    {
        if (whichTheme == 0 && theme1Images.Length == 0)
        {
            theme1Images = new Image[Theme1Sprites.Count];
            theme1Images = GetImages(themes1.GetComponentsInChildren<Image>(true));
        }
        else if (whichTheme == 1 && theme2Images.Length == 0)
        {
            theme2Images = new Image[Theme2Sprites.Count];
            theme2Images = GetImages(themes2.GetComponentsInChildren<Image>(true));
        }

        for (int i = 0; i < theme1Images.Length; i++)
        {
            if (whichTheme == 0)
            {
                if (PlayerDataController.data.theme1[i].state)
                {
                    theme1Images[i].transform.parent.GetComponent<Button>().enabled = true;
                    theme1Images[i].gameObject.SetActive(true);

                    theme1Images[i].sprite = Theme1Sprites[i];
                    theme1Images[i].transform.parent.GetComponent<Image>().sprite = (PlayerDataController.data.selectedTheme1 == i) ? selectedThemeSprite : openThemeSprite;
                    if (PlayerDataController.data.selectedTheme1 == i)
                        lastSelectedTheme1 = theme1Images[i].transform.parent.GetComponent<Image>();
                }
                else
                {
                    theme1Images[i].transform.parent.GetComponent<Image>().sprite = closeThemeSprite;
                    theme1Images[i].transform.parent.GetComponent<Button>().enabled = false;
                    theme1Images[i].gameObject.SetActive(false);
                }
            }
            else
            {
                if (PlayerDataController.data.theme2[i].state)
                {
                    theme2Images[i].transform.parent.GetComponent<Button>().enabled = true;
                    theme2Images[i].gameObject.SetActive(true);

                    theme2Images[i].sprite = Theme2Sprites[i];
                    theme2Images[i].transform.parent.GetComponent<Image>().sprite = (PlayerDataController.data.selectedTheme2 == i) ? selectedThemeSprite : openThemeSprite;
                    if (PlayerDataController.data.selectedTheme2 == i)
                        lastSelectedTheme2 = theme2Images[i].transform.parent.GetComponent<Image>();

                }
                else
                {
                    theme2Images[i].transform.parent.GetComponent<Image>().sprite = closeThemeSprite;
                    theme2Images[i].transform.parent.GetComponent<Button>().enabled = false;
                    theme2Images[i].gameObject.SetActive(false);
                }
            }

        }

    }
    public Image[] GetImages(Image[] images)
    {
        Image[] newImage;
        List<Image> newImageList = new List<Image>();
        int count = 0;

        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].tag == "ItemImages")
            {
                count++;
                newImageList.Add(images[i]);
            }
        }

        newImage = new Image[count];

        for (int i = 0; i < newImageList.Count; i++)
        {
            newImage[i] = newImageList[i];
        }

        return newImage;
    }
    public void onClickWatchAd(string whichAdToWatch)
    {
        AdManager.instance.ShowRewarded(whichAdToWatch, onAdState, onAdComplete);
    }
    public void onClickContinueAd()
    {
        StopCoroutine(continueProgCoroutine);
        CloseScreen((int)Screens.Failscreen);

        AdManager.instance.ShowRewarded("continue", onAdState, onAdComplete);
    }
    public void onClickNoThanks()
    {

        if (continueProgCoroutine != null)
            StopCoroutine(continueProgCoroutine);

        CloseScreen((int)Screens.Failscreen);
        LevelManager.instance.LoadLevel(false);
    }
    public void onAdState(string whichAd, AdManager.AD_STATES state)
    {
        if (whichAd == "continue")
        {
            if (state == AdManager.AD_STATES.READY)
                Debug.Log(whichAd + " Reklamı Hazır ve izleniyor!");
            else
            {
                Debug.Log(whichAd + " Reklamı Hazir Degil!");
                onClickNoThanks();
            }
        }
        else if (whichAd == "2x")
        {
            if (state == AdManager.AD_STATES.READY)
                Debug.Log(whichAd + " Reklamı Hazır ve izleniyor!");
            else
                Debug.Log(whichAd + " Reklamı Hazir Degil!");
        }
        else
        {
            if (state == AdManager.AD_STATES.READY)
                Debug.Log("Reklam Hazır ve izleniyor!");
            else
                Debug.Log("Reklam Hazır Degil!");
        }
    }
    public void onAdComplete(string whichAd, AdManager.AD_STATES state)
    {
        if (whichAd == "continue")
        {
            if (state == AdManager.AD_STATES.COMPLETE)
            {
                GameManager.instance.ChangeGameStates(GameManager.GameStates.PLAY);
            }
            else if (state == AdManager.AD_STATES.NOT_COMPLETE)
            {
                GameManager.instance.ChangeGameStates(GameManager.GameStates.WAIT_FOR_PLAY);
            }
        }
        else if (whichAd == "2x")
        {
            if (state == AdManager.AD_STATES.COMPLETE)
            {
                Player.main.goldAccuiredThisLevel *= 2;

                Player.main.UpdatePlayerGold(Player.main.goldAccuiredThisLevel);
            }

            CloseScreen((int)Screens.SuccessUI);
        }
        else if (whichAd == "rainbow")
        {
            if (state == AdManager.AD_STATES.COMPLETE)
            {
                RainbowActivate();
            }
        }
    }
    public void CloseScreen(int screenIndex)
    {
        MenuScreens[screenIndex].SetActive(false);
        //Asagida bu ekrana ait ozel kodlari ekleyebilirsin
        switch ((Screens)screenIndex)
        {
            case Screens.MainScreen:
                break;
            case Screens.Settings:

                GamePlayUI.SetActive(false);
                break;
            case Screens.GameplayScreen:
                GamePlayUI.SetActive(false);
                GameManager.playerState = Player.States.Empty;
                break;
            case Screens.RainbowUI:
                GameManager.playerState = Player.States.Empty;
                break;
            case Screens.TrashUI:

                GameManager.playerState = Player.States.Empty;
                break;
            case Screens.Failscreen:
                GameManager.playerState = Player.States.Empty;
                break;
            case Screens.LevelInfoUI:
                GameManager.playerState = Player.States.Empty;
                break;
            case Screens.PauseUI:

                GameManager.playerState = Player.States.Empty;
                break;
            case Screens.SuccessUI:
                LevelManager.instance.PassNewLevel();
                GameManager.playerState = Player.States.Empty;
                break;
            case Screens.ShopUI:
                OpenScreen((int)Screens.PauseUI);
                break;
            case Screens.GamePlayUI:

                GameManager.playerState = Player.States.UI;
                break;
            default:
                break;
        }
    }
    public void OpenScreen(int screenIndex)
    {
        MenuScreens[screenIndex].SetActive(true);

        //Asagida bu ekrana ait ozel kodlari ekleyebilirsin
        switch ((Screens)screenIndex)
        {
            case Screens.MainScreen:
                break;
            case Screens.GameplayScreen:
                GamePlayUI.SetActive(true);
                missionInfoUI.SetActive(true);
                GameManager.playerState = Player.States.UI;
                break;
            case Screens.RainbowUI:

                if (Player.main.Gold < 400)
                    rainbowGoldButton.GetComponent<Button>().interactable = false;
                else
                    rainbowGoldButton.GetComponent<Button>().interactable = true;
                GameManager.playerState = Player.States.UI;
                break;
            case Screens.TrashUI:
                Trash();
                break;
            case Screens.Failscreen:

                GameManager.playerState = Player.States.UI;
                break;
            case Screens.LevelInfoUI:
                LevelInfoUI.instance.DisplayLevelInfo(PlayerDataController.data.levelNum, LevelManager.instance.activeLevelData.missions);
                GameManager.playerState = Player.States.UI;
                break;
            case Screens.PauseUI:
                LoadPlayerSettings();
                GameManager.playerState = Player.States.UI;
                break;
            case Screens.SuccessUI:
                LevelSuccessUI.main.UpdateText();
                GameManager.playerState = Player.States.UI;
                break;
            case Screens.ShopUI:
                CloseScreen((int)Screens.PauseUI);
                break;
            case Screens.GoodJobUI:

                GameManager.playerState = Player.States.UI;
                break;
            default:
                break;
        }
    }
    public void LoadPlayerSettings()
    {
        hapticSetting = PlayerDataController.data.isVibrationOpen;
        soundSetting = PlayerDataController.data.isSoundOpen;

        soundSettingDisplay.sprite = soundSprites[soundSetting ? 1 : 0];

        hapticSettingDisplay.sprite = hapticSprites[hapticSetting ? 1 : 0];
    }
    public Sprite[] soundSprites;
    public Image soundSettingDisplay;
    public bool soundSetting;
    public void ToggleSound()
    {
        soundSetting = !soundSetting;
        PlayerDataController.SaveData("isSoundOpen", soundSetting);
        PlayerDataController.SaveData("isRefreshDataEveryLaunch", !soundSetting);
        soundSettingDisplay.sprite = soundSprites[soundSetting ? 1 : 0];
        Debug.Log("Sound Toggled");
    }

    public Sprite[] hapticSprites;
    public Image hapticSettingDisplay;
    public bool hapticSetting;
    public void ToggleHaptic()
    {
        hapticSetting = !hapticSetting;
        PlayerDataController.SaveData("isVibrationOpen", hapticSetting);

        hapticSettingDisplay.sprite = hapticSprites[hapticSetting ? 1 : 0];
        Debug.Log("Haptic Toggled");
    }

    public void NightMode()
    {
        Debug.Log("nightMode");
    }

    public void IAP()
    {
        Debug.Log("In App Purchases");
    }

    public void GameCenter()
    {
        Debug.Log("GameCenter");
    }

    public void StartContinueCountdown()
    {
        ContinueTitle.text = "LEVEL " + PlayerDataController.data.levelNum + "\nCONTINUE?";

        NoThanksButton.SetActive(false);

        if (continueProgCoroutine != null)
            StopCoroutine(continueProgCoroutine);

        continueProgCoroutine = StartCoroutine(Timer.TimeSteps(

            PlayerDataController.data.continueCountdown,

            Time.deltaTime,

            () =>
            {
                SetProgress(ContinueProgressbar, 0);
            },

            (elapsed) =>
            {

                if (elapsed > PlayerDataController.data.noThanksAppearAfter && !NoThanksButton.activeInHierarchy)
                    NoThanksButton.SetActive(true);

                ContinueCountdownText.text = "" + Math.Round((PlayerDataController.data.continueCountdown - elapsed));
                SetProgress(ContinueProgressbar, (PlayerDataController.data.continueCountdown - elapsed) * .1f);

            },

            () =>
            {
                ContinueCountdownText.text = "";
                SetProgress(ContinueProgressbar, 1);
                onClickNoThanks();
            }

        ));
    }
    public void SetProgress(Image go, float amount)
    {
        go.fillAmount = amount;
    }
    public void CheckContinue()
    {
        GameManager.instance.ChangeGameStates(GameManager.GameStates.FAIL_FINISH);
    }
    public void ToggleVibrationState()
    {
        PlayerDataController.SaveData("isVibrationOpen", !PlayerDataController.data.isVibrationOpen);

        VibrateOpenIcon.SetActive(false);
        VibrateCloseIcon.SetActive(false);
        VibrateOpenRadioIcon.SetActive(false);
        VibrateCloseRadioIcon.SetActive(false);

        if (PlayerDataController.data.isVibrationOpen)
        {
            VibrateOpenIcon.SetActive(true);
            VibrateOpenRadioIcon.SetActive(true);
        }
        else
        {
            VibrateCloseIcon.SetActive(true);
            VibrateCloseRadioIcon.SetActive(true);
        }
    }
    public void ChangeTheme1(int itemIndex)
    {
        if (itemIndex == PlayerDataController.data.selectedTheme1)
            return;

        PlayerDataController.SaveData("selectedTheme1", itemIndex);
        lastSelectedTheme1.sprite = openThemeSprite;
        for (int i = 0; i < theme1Images.Length; i++)
        {
            if (theme1Images[i].transform.parent.name == "Item" + (itemIndex + 1))
            {
                theme1Images[i].gameObject.SetActive(true);
                theme1Images[i].sprite = Theme1Sprites[itemIndex];
                theme1Images[i].transform.parent.GetComponent<Button>().enabled = true;
                theme1Images[i].transform.parent.GetComponent<Image>().sprite = selectedThemeSprite;
                lastSelectedTheme1 = theme1Images[i].transform.parent.GetComponent<Image>();
                break;
            }
        }
    }
    public void ChangeTheme2(int itemIndex)
    {
        if (itemIndex == PlayerDataController.data.selectedTheme2)
            return;
        PlayerDataController.SaveData("selectedTheme2", itemIndex);
        lastSelectedTheme2.sprite = openThemeSprite;
        for (int i = 0; i < theme2Images.Length; i++)
        {
            if (theme2Images[i].transform.parent.name == "Item" + (itemIndex + 1))
            {
                theme2Images[i].gameObject.SetActive(true);
                theme2Images[i].sprite = Theme2Sprites[itemIndex];
                theme2Images[i].transform.parent.GetComponent<Button>().enabled = true;
                theme2Images[i].transform.parent.GetComponent<Image>().sprite = selectedThemeSprite;
                lastSelectedTheme2 = theme2Images[i].transform.parent.GetComponent<Image>();
                break;
            }
        }
    }
    public void OpenRandomTheme(int themeIndex)
    {
        if ((themeIndex == 0 && PlayerDataController.data.gold < PlayerDataController.data.inventory1RandomGoldPrice) ||
            (themeIndex == 1 && PlayerDataController.data.gold < PlayerDataController.data.inventory2RandomGoldPrice))
            return;

        List<ThemesDataModel> unopenedThemes;

        if (themeIndex == 0)
            unopenedThemes = PlayerDataController.data.theme1.FindAll(x => x.state == false);
        else
            unopenedThemes = PlayerDataController.data.theme2.FindAll(x => x.state == false);

        if (unopenedThemes.Count == 0)
            return;

        if (randomSelectCoroutine != null)
            StopCoroutine(randomSelectCoroutine);

        int[] travelList = new int[4];

        if (unopenedThemes.Count < 4)
            travelList = new int[unopenedThemes.Count];

        FlatHelper.ShuffleList<ThemesDataModel>(unopenedThemes);

        for (int i = 0; i < travelList.Length; i++)
        {
            travelList[i] = unopenedThemes[i].id;
        }

        randomSelectCoroutine = StartCoroutine(RandomSelectAnimation(travelList.Length, travelList, themeIndex));

    }
    IEnumerator RandomSelectAnimation(int cycleCount, int[] travelList, int whichTheme)
    {
        var elapsed = 0;
        var xInc = 234;
        var yInc = -233.5f;

        var defaultX = -354.0f;
        var defaultY = 443.0f;

        animatedSelectedTheme.SetActive(true);

        if (whichTheme == 0)
            lastSelectedTheme1.sprite = openThemeSprite;
        else
            lastSelectedTheme2.sprite = openThemeSprite;

        while (elapsed < cycleCount)
        {
            animatedSelectedTheme.GetComponent<RectTransform>().position = new Vector2(
                Screen.width / 2 + defaultX + xInc * (travelList[elapsed] % 4),
                Screen.height / 2 + defaultY + yInc * (int)(travelList[elapsed] / 4)
            );
            yield return new WaitForSeconds(.15f);

            elapsed++;
        }

        animatedSelectedTheme.SetActive(false);

        if (whichTheme == 0)
        {
            PlayerDataController.ChangeValueInList("theme1", "item" + (travelList[travelList.Length - 1] + 1), true);
            PlayerDataController.SaveData("gold", PlayerDataController.data.gold - PlayerDataController.data.inventory1RandomGoldPrice);
            ChangeTheme1(travelList[travelList.Length - 1]);
        }
        else
        {
            PlayerDataController.ChangeValueInList("theme2", "item" + (travelList[travelList.Length - 1] + 1), true);
            PlayerDataController.SaveData("gold", PlayerDataController.data.gold - PlayerDataController.data.inventory2RandomGoldPrice);
            ChangeTheme2(travelList[travelList.Length - 1]);
        }

        UpdateGoldText();
        StopCoroutine(randomSelectCoroutine);
    }
    public void UpdateUI()
    {
        // AnimateGolds();
        UpdateGoldText();
    }
    public void UpdateGoldText()
    {

        string str = "";
        string str1 = "";
        string str2 = "";
        string str3 = "";
        long gold = Player.main.Gold;

        if (gold < 1000)
        {
            str = "" + gold;
        }
        else if (gold >= 1000 && gold < 10000)
        {
            str1 = "." + Math.Floor(((float)gold % 1000) / 100);
            str1 += "" + Math.Floor((((float)gold % 1000) % 100) / 10);

            if (str1 == ".00")
                str1 = "";

            str = "" + Math.Floor((float)gold / 1000) + str1 + "K";
        }
        else if (gold >= 10000 && gold < 100000)
        {
            str1 = "." + Math.Floor(((float)gold % 1000) / 100);

            if (str1 == ".0")
                str1 = "";

            str = "" + Math.Floor((float)gold / 1000) + str1 + "K";
        }
        else if (gold >= 100000 && gold < 1000000)
        {
            str = "" + Math.Floor((float)gold / 1000) + str1 + "K";
        }
        else if (gold >= 1000000 && gold < 10000000)
        {

            str1 = "." + Math.Floor(((float)gold % 1000000) / 100000);
            str1 += "" + Math.Floor((((float)gold % 1000000) % 100000) / 10000);

            if (str1 == ".00")
                str1 = "";

            str = "" + Math.Floor((float)gold / 1000000) + str1 + "M";
        }
        else if (gold >= 10000000 && gold < 100000000)
        {

            str1 = "." + Math.Floor(((float)gold % 1000000) / 100000);

            if (str1 == ".0")
                str1 = "";

            str = "" + Math.Floor((float)gold / 1000000) + str1 + "M";
        }
        else if (gold >= 100000000 && gold < 1000000000)
        {

            str = "" + Math.Floor((float)gold / 1000000) + "M";
        }
        else
        {
            str = "" + gold;
        }

        GoldText.text = str;
        // RandomButtonTheme1Text.text = "" + PlayerDataController.data.inventory1RandomGoldPrice;
        // RandomButtonTheme2Text.text = "" + PlayerDataController.data.inventory2RandomGoldPrice;
        // WatchButtonTheme1Text.text = "+" + PlayerDataController.data.GetVideoReward("gold");
        // WatchButtonTheme2Text.text = "+" + PlayerDataController.data.GetVideoReward("gold");

        // if (PlayerDataController.data.theme1.FindAll(x => x.state == false).Count > 0 && PlayerDataController.data.gold >= PlayerDataController.data.inventory1RandomGoldPrice)
        // {
        //     Theme1Mark.SetActive(true);
        // }
        // else
        // {
        //     Theme1Mark.SetActive(false);
        // }
        // if (PlayerDataController.data.theme2.FindAll(x => x.state == false).Count > 0 && PlayerDataController.data.gold >= PlayerDataController.data.inventory2RandomGoldPrice)
        // {
        //     Theme2Mark.SetActive(true);
        // }
        // else
        // {
        //     Theme2Mark.SetActive(false);
        // }

    }



    public GameObject rainbowEffectButton;
    public GameObject rainbowEffectUI;
    public GameObject rainbowGoldButton;
    public List<GameObject> missionCheckMarks;

    public void ChangeRainbowButtonState(bool state)
    {
        rainbowEffectButton.SetActive(state);
    }

    public void RainbowEffect()
    {
        GameManager.instance.ActivateRainbowEffect();
        CloseScreen(2);
    }

    public void RainbowActivate()
    {
        GameManager.instance.ActivateRainbowEffect();
        CloseScreen(2);
    }

    public void Skip()
    {
        GamePlayUI.SetActive(true);
        CloseScreen((int)Screens.Failscreen);
        LevelManager.instance.PassNewLevel();
    }

    public void FailLevel()
    {
        GamePlayUI.SetActive(false);
        OpenScreen((int)Screens.Failscreen);

    }

    void AddGolds()
    {
        GameObject go;

        AddedGoldSprites = new GameObject[goldSprites.Count];

        for (int i = 0; i < goldSprites.Count; i++)
        {
            go = Instantiate(goldSprites[i], UICanvas.transform);
            go.SetActive(false);
            AddedGoldSprites[i] = go;
        }
    }
    void AnimateGolds()
    {
        float minX = 0;
        float maxX = 400;

        float minY = 0;
        float maxY = 200;

        float centerX = 261;
        float centerY = -628;

        float randX;
        float randY;

        float totalDuration = 0;

        for (int i = 0; i < AddedGoldSprites.Length; i++)
        {
            randX = UnityEngine.Random.Range(minX, maxX);
            if (randX > (maxX - minX) / 2)
            {
                randX /= 2;
            }
            else
            {
                randX *= -1;
            }
            randX += centerX;

            randY = UnityEngine.Random.Range(minY, maxY);
            if (randY > (maxY - minY) / 2)
            {
                randY /= 2;
            }
            else
            {
                randY *= -1;
            }
            randY += centerY;

            AddedGoldSprites[i].SetActive(true);
            AddedGoldSprites[i].transform.position = new Vector3(randX + Screen.width / 2, randY + Screen.height / 2);

            // iTween.MoveTo(AddedGoldSprites[i], iTween.Hash("x", Screen.width - 120, "y", Screen.height - 120, "easetype", "easeInOutBack", "time", .5, "delay", .025f * i));

            totalDuration += .01f * i;
        }
        if (goldCoroutine != null)
            this.StopCoroutine(goldCoroutine);

        this.StartCoroutine(HideGolds(totalDuration - 1f));
    }
    IEnumerator HideGolds(float duration)
    {
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < AddedGoldSprites.Length; i++)
        {
            AddedGoldSprites[i].SetActive(false);
        }
    }
}