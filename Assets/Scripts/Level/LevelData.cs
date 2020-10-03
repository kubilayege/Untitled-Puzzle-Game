using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public int levelNumber;
    public GameManager.ModeTypes startingMode;
    public PartTheme partTheme;
    public List<Animals> animals;
    public List<int> animalIndex;
    public List<Sprite> levelPartSprites;

    // public GameObject[] blockers1;
    // public GameObject[] blockers2;
    // public GameObject[] blockers3;
    // public GameObject[] otherBlockers;
    // public List<GameObject[]> mblokers;
    [SerializeField]
    public List<MyTuples> blockerPos;
    public List<GameObject> animalObjects;
    public int[] missionLenghts = new int[3];
    public List<Mission> missions;
    Coroutine levelCompleteCor;
    public bool MissionCompleted()
    {

        bool levelCompleted = true;
        foreach (var m in missions)
        {
            if (!m.missionCompleted)
                levelCompleted = false;

        }

        if (levelCompleted && levelCompleteCor == null)
        {
            levelCompleteCor = StartCoroutine(DelayLevelPass(3.5f));
            return true;
        }
        else if (levelCompleted)
        {
            return true;
        }

        return false;

    }
    public IEnumerator DelayLevelPass(float time)
    {
        yield return new WaitForSeconds(time / 2);
        if (LevelManager.instance.tm != null)
            UIManager.instance.OpenScreen((int)UIManager.Screens.GoodJobUI);
        else
            UIManager.instance.OpenScreen((int)UIManager.Screens.SuccessUI);
        yield return new WaitForSeconds(time / 2);
        levelCompleteCor = null;
        GameManager.instance.ChangeGameStates(GameManager.GameStates.SUCCESS_FINISH);
    }
    public IEnumerator AnimateAnimal(Mission mission, int times)
    {
        while (times > 0)
        {
            mission.animalModel.GetComponent<Animator>().Play("Jump");
            Vector3 pos = mission.animalModel.transform.position;
            yield return new WaitForSeconds(mission.animalRejoiceAnimationLenght);
            times--;
        }
        mission.corRejoice = null;
    }
    void StartRejoice(Mission mission)
    {
        Vector3 pos = mission.animalModel.transform.position;
        mission.animalModel.GetComponent<Animator>().Play("Jump");
    }
    public Coroutine Rejoice(Mission mission, int times)
    {
        return StartCoroutine(AnimateAnimal(mission, times));
    }

    public void LevelComplete()
    {
        foreach (Mission mission in missions)
        {
            Destroy(mission.animalTexture);
            Destroy(mission.animalModel);
        }
    }
}