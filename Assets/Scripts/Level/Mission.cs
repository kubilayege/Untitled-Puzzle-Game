using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Mission
{
    public Animals animal;
    public float animalRejoiceAnimationLenght;
    public GameObject animalTexture;
    public GameObject animalModel;
    public GameObject animalCage;

    public Text progressBar;
    public GameObject checkMark;
    public Coroutine corRejoice;

    public int Count;

    public bool missionCompleted;
    public bool missionShouldProgress;
    public bool ProgressMission(CellManager cm)
    {
        // Count--;
        progressBar.text = Count.ToString();
        if (missionShouldProgress)
        {
            iTween.PunchScale(progressBar.gameObject, new Vector3(1.1f, 1.1f, 1.1f), 1f);
            missionShouldProgress = false;
        }
        if (Count <= 0)
        {
            checkMark.gameObject.SetActive(true);
            progressBar.transform.parent.gameObject.SetActive(false);
            Count = 0;
            progressBar.text = "";
            if (!missionCompleted)
            {
                BreakCage();
                AnimateAnimal();
            }
            missionCompleted = true;
            return LevelManager.instance.activeLevelData.MissionCompleted();
        }

        return false;
    }



    public void AnimateAnimal()
    {
        if (Count <= 0)
        {
            if (corRejoice == null)
                corRejoice = LevelManager.instance.activeLevelData.Rejoice(this, 3);
        }
        else
        {
            if (corRejoice == null)
                corRejoice = LevelManager.instance.activeLevelData.Rejoice(this, 1);
        }
    }

    public Mission(Animals _animal, Text _progress, GameObject _checkMark, int length)
    {
        animal = _animal;
        progressBar = _progress;
        checkMark = _checkMark;
        // progressBar.transform.parent.gameObject.SetActive(true);
        progressBar.text = length.ToString();
        Count = length;
    }

    public void SetAnimalModel(GameObject _animalModel, GameObject _animalCage)
    {
        animalModel = _animalModel;
        foreach (var clip in animalModel.GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if (clip.name == "Jump")
            {
                animalRejoiceAnimationLenght = clip.length;

            }
        }

        animalCage = _animalCage;
        animalCage.transform.localScale = animal.cageScale;
        animalCage.transform.parent = animalModel.transform;
    }

    public void BreakCage()
    {
        animalCage.transform.parent = LevelManager.instance.activeLevel.transform;
        animalCage.transform.GetChild(0).gameObject.SetActive(false);
        GameObject breakables = animalCage.transform.GetChild(1).gameObject;
        breakables.SetActive(true);
        foreach (var part in breakables.GetComponentsInChildren<Rigidbody>())
        {
            part.AddExplosionForce(2400f, part.transform.position, 1f);
        }
        MonoBehaviour.Destroy(animalCage, 3f);
    }

    public void RewindMissionCount(int rewindTo)
    {
        if (Count == 0 && rewindTo != 0)
        {
            animalCage = MonoBehaviour.Instantiate(GridConstructer.instance.cagePrefab, animalModel.transform.position,
                                 animalModel.transform.rotation, LevelManager.instance.activeLevel.transform);
            animalCage.transform.localScale = animal.cageScale;
            animalCage.transform.parent = animalModel.transform;
            checkMark.gameObject.SetActive(false);
            progressBar.transform.parent.gameObject.SetActive(true);
            missionCompleted = false;
        }

        if (Count != rewindTo)
        {
            Count = rewindTo;
            progressBar.text = Count.ToString();
            LevelManager.instance.SaveMissionProgress(this);
            if (rewindTo == 0)
            {
                if (animalCage != null)
                    BreakCage();
                checkMark.gameObject.SetActive(true);
                progressBar.transform.parent.gameObject.SetActive(false);
                missionCompleted = true;
                progressBar.text = "";
            }
        }

    }
}