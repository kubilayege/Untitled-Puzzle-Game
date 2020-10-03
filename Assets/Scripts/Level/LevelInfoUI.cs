using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoUI : MonoBehaviour
{
    public Text LevelNumberDisplay;
    public GameObject contentParent;
    public GameObject missionInfoPrefab;
    public List<MissionInfo> missionInfos;
    public static LevelInfoUI instance;
    private void Awake()
    {
        instance = this;
    }

    public void DisplayLevelInfo(int levelNumber, List<Mission> missions)
    {
        if (missionInfos.Count != 0)
        {
            foreach (var missionInfo in missionInfos)
            {
                Destroy(missionInfo.gameObject);
            }
        }

        missionInfos = new List<MissionInfo>();
        Debug.Log("mission info count " + missionInfos.Count);

        LevelNumberDisplay.text = "Level " + levelNumber;

        foreach (var mission in missions)
        {
            missionInfos.Add(Instantiate(missionInfoPrefab, contentParent.transform).GetComponent<MissionInfo>());
            missionInfos[missionInfos.Count - 1].Init(mission.animal.animalTexture, mission.Count);
        }
    }
}
