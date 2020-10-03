using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class PlayerDataController
{
    public static PlayerDataModel data;
    static string jsonData;
    static VideoAdsDataModel jsonAdsData1;
    static ThemesDataModel jsonAdsData2;

    static MyTuples jsonGridData;
    static int animalProgress;
    static bool isInStart;
    public static T GetData<T>(string key)
    {
        if (HasKey(key) && typeof(T) == typeof(float))
        {
            return (T)(object)PlayerPrefs.GetFloat(key);
        }
        else if (HasKey(key) && typeof(T) == typeof(int))
        {
            return (T)(object)PlayerPrefs.GetInt(key);
        }
        else if (HasKey(key) && typeof(T) == typeof(string))
        {
            return (T)(object)PlayerPrefs.GetString(key);
        }
        else if (HasKey(key) && typeof(T) == typeof(bool))
        {
            return (T)(object)FlatHelper.ToBool(PlayerPrefs.GetInt(key));
        }
        else
        {
            return default(T);
        }
    }
    public static bool SetDefaultDataForFirstTime(string jsonFile)
    {
        isInStart = false;
        Timer.Reset();
        data = FlatHelper.FromJson<PlayerDataModel>(jsonFile);

        if (!HasKey("isRefreshDataEveryLaunch") || GetData<int>("isRefreshDataEveryLaunch") == 1)
        {
            Clear();
            foreach (var property in typeof(PlayerDataModel).GetFields())
            {
                if (property.GetValue(data).GetType() == typeof(List<VideoAdsDataModel>))
                {
                    jsonData = FlatHelper.ToJson<VideoAdsDataModel>(property.GetValue(data));
                    SaveData(property.Name, jsonData);
                }
                else if (property.GetValue(data).GetType() == typeof(List<ThemesDataModel>))
                {
                    jsonData = FlatHelper.ToJson<ThemesDataModel>(property.GetValue(data));
                    SaveData(property.Name, jsonData);
                }
                else if (property.GetValue(data).GetType() == typeof(List<MyTuples>))
                {
                    jsonData = FlatHelper.ToJson<MyTuples>(property.GetValue(data));
                    SaveData(property.Name, jsonData);
                }
                else
                {
                    SaveData(property.Name, property.GetValue(data));
                }
            }

            isInStart = true;
            return true;
        }
        else
        {
            RefreshModel();
            isInStart = true;
            return false;
        }
    }

    public static void ResetLevelData()
    {
        Debug.Log("Level Data Is Resetted");
        foreach (var cellData in data.uncompletedLevel)
        {
            cellData.isFull = false;
        }

        Player.main.goldAccuiredThisLevel = 0;

        SaveData("uncompletedLevel", FlatHelper.ToJson<MyTuples>(data.uncompletedLevel));
        SaveData("isLevelCompleted", false);
        SaveData("animalProgress", FlatHelper.ToJson<int>(LevelManager.instance.activeLevelData.missionLenghts));
        SaveData("levelModIndex", (int)LevelManager.instance.activeLevelData.startingMode);
        SaveData("levelPartTheme", (int)LevelManager.instance.activeLevelData.partTheme);
        SaveData("goldAccuiredThisLevel", Player.main.goldAccuiredThisLevel);
    }

    static void RefreshModel()
    {
        foreach (var property in typeof(PlayerDataModel).GetFields())
        {
            if (property.GetValue(data).GetType() == typeof(List<VideoAdsDataModel>))
            {
                data.videoAds.Clear();
                string[] strArr = GetData<string>(property.Name).Split('|');
                for (int i = 0; i < strArr.Length; i++)
                {
                    jsonAdsData1 = FlatHelper.FromJson<VideoAdsDataModel>(strArr[i]);
                    data.videoAds.Add(jsonAdsData1);
                }
            }
            else if (property.GetValue(data).GetType() == typeof(List<ThemesDataModel>))
            {
                if (property.Name == "theme1")
                    data.theme1.Clear();
                else
                    data.theme2.Clear();

                string[] strArr = GetData<string>(property.Name).Split('|');
                for (int i = 0; i < strArr.Length; i++)
                {
                    jsonAdsData2 = FlatHelper.FromJson<ThemesDataModel>(strArr[i]);
                    if (property.Name == "theme1")
                        data.theme1.Add(jsonAdsData2);
                    else
                        data.theme2.Add(jsonAdsData2);
                }
            }
            else if (property.GetValue(data).GetType() == typeof(List<MyTuples>))
            {
                if (property.Name == "uncompletedLevel")
                {
                    data.uncompletedLevel.Clear();
                    string[] strArr = GetData<string>(property.Name).Split('|');
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        jsonGridData = FlatHelper.FromJson<MyTuples>(strArr[i]);
                        data.uncompletedLevel.Add(jsonGridData);
                        // Debug.Log(jsonGridData.x + "," + jsonGridData.y + " > " + jsonGridData.isFull);
                    }
                }
                else
                {
                    data.savedShapes.Clear();
                    string[] strArr = GetData<string>(property.Name).Split('|');
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        jsonGridData = FlatHelper.FromJson<MyTuples>(strArr[i]);
                        data.savedShapes.Add(jsonGridData);
                        // Debug.Log(jsonGridData.x + "," + jsonGridData.y + " > " + jsonGridData.isFull);
                    }
                }
            }
            else
            {
                var type = property.GetValue(data).GetType();
                if (type == typeof(float))
                    property.SetValue(data, GetData<float>(property.Name));
                else if (type == typeof(int))
                    property.SetValue(data, GetData<int>(property.Name));
                else if (type == typeof(string))
                    property.SetValue(data, GetData<string>(property.Name));
                else if (type == typeof(bool))
                    property.SetValue(data, GetData<bool>(property.Name));
            }
        }
    }
    public static void SaveData(string key, object value)
    {
        if (value.GetType() == typeof(int))
        {
            PlayerPrefs.SetInt(key, (int)value);
        }
        else if (value.GetType() == typeof(bool))
        {
            PlayerPrefs.SetInt(key, FlatHelper.ToInt(value));
        }
        else if (value.GetType() == typeof(string))
        {
            PlayerPrefs.SetString(key, (string)value);
        }
        else if (value.GetType() == typeof(float))
        {
            PlayerPrefs.SetFloat(key, (float)value);
        }


        // PlayerPrefs.Save();
        if (isInStart)
            RefreshModel();
    }
    public static void SaveData(string key, object value, bool isIncrement)
    {
        if (value.GetType() == typeof(int))
        {
            if (isIncrement)
                PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key) + (int)value);
            else
                PlayerPrefs.SetInt(key, (int)value);
        }
        else if (value.GetType() == typeof(bool))
        {
            PlayerPrefs.SetInt(key, FlatHelper.ToInt(value));
        }
        else if (value.GetType() == typeof(string))
        {
            PlayerPrefs.SetString(key, (string)value);
        }
        else if (value.GetType() == typeof(float))
        {
            if (isIncrement)
                PlayerPrefs.SetFloat(key, PlayerPrefs.GetFloat(key) + (float)value);
            else
                PlayerPrefs.SetFloat(key, (float)value);
        }
        PlayerPrefs.Save();
        if (isInStart)
            RefreshModel();
    }
    public static void ChangeValueInList(string listName, string key, object value)
    {
        VideoAdsDataModel dataModel1;
        ThemesDataModel dataModel2;
        int count;

        if (listName == "videoAds")
        {
            for (int i = 0; i < data.videoAds.Count; i++)
            {

                dataModel1 = data.videoAds[i];
                if (dataModel1.name == key)
                {
                    dataModel1.reward = (int)value;
                }
            }
        }
        else if (listName == "theme1" || listName == "theme2")
        {
            count = listName == "theme1" ? data.theme1.Count : data.theme2.Count;
            for (int i = 0; i < count; i++)
            {
                dataModel2 = listName == "theme1" ? data.theme1[i] : data.theme2[i];
                if (dataModel2.name == key)
                {
                    dataModel2.state = (bool)value;
                }
            }
        }

        if (listName == "videoAds")
            jsonData = FlatHelper.ToJson<VideoAdsDataModel>(data.videoAds);
        else if (listName == "theme1" || listName == "theme2")
            jsonData = FlatHelper.ToJson<ThemesDataModel>((listName == "theme1" ? data.theme1 : data.theme2));
        SaveData(listName, jsonData);

    }
    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
    }
    public static void Clear(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
}