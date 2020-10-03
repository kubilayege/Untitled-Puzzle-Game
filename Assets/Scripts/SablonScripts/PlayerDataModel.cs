using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerDataModel : IPlayerDataView
{
    public bool isRefreshDataEveryLaunch;

    public int levelNum;
    public int bonusLevelStepCount;
    public bool isBonusLevel;

    public bool isLevelCompleted;
    public bool isVibrationOpen;
    public bool isSoundOpen;
    public int inventory1RandomGoldPrice;
    public int inventory2RandomGoldPrice;
    public int continueCountdown;
    public float noThanksAppearAfter;
    public float adsTime;
    public int gold;
    public int goldAccuiredThisLevel;
    public List<MyTuples> uncompletedLevel;
    public List<MyTuples> savedShapes;
    public List<VideoAdsDataModel> videoAds;
    public int selectedTheme1;
    public int selectedTheme2;
    public string theme1Title;
    public string theme2Title;
    public List<ThemesDataModel> theme1;
    public List<ThemesDataModel> theme2;

    public int levelModIndex;
    public int levelPartTheme;
    public bool isRewardedVideoWatched;
    public string whichVideoFor;
    public float durationForContinuePanelShow;

    public bool isTest;
    public float intersitialDurationForTest;
    public float rewardedDurationForTest;

    public int GetVideoReward(string key)
    {
        if (videoAds.Exists(x => x.name == key))
        {
            return videoAds.Find((x) => x.name == key).reward;
        }
        else
        {
            return -1;
        }
    }
    public int GetThemePrice(int whichTheme, string key)
    {

        if (whichTheme == 0 && theme1.Exists(x => x.name == key))
        {
            return theme1.Find((x) => x.name == key).price;
        }
        else if (whichTheme == 1 && theme2.Exists(x => x.name == key))
        {
            return theme2.Find((x) => x.name == key).price;
        }
        else
        {
            return -1;
        }
    }
    public bool GetThemeState(int whichTheme, string key)
    {
        if (whichTheme == 0 && theme1.Exists(x => x.name == key))
        {
            return theme1.Find((x) => x.name == key).state;
        }
        else if (whichTheme == 1 && theme2.Exists(x => x.name == key))
        {
            return theme2.Find((x) => x.name == key).state;
        }
        else
        {
            return false;
        }
    }
}