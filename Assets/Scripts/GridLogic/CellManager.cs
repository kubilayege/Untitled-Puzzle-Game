using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public struct Pos
    {
        public int x;
        public int y;
    }

    public Pos index;
    public bool isFull;
    public bool blocked;
    public GameObject fruitOnTop;
    public Mission mission;
    public bool RemoveAndDeleteObjectOnTop()
    {
        if (fruitOnTop != null && !blocked)
        {
            GameManager.emptyCellSlots++;
            if (fruitOnTop.GetComponent<Part>().Explode())
            {
                isFull = false;
                LevelManager.instance.SaveBlockStatusChange(this, false);
                fruitOnTop = null;
            }
            GameManager._killCount += 1;

            if (mission != null && mission.missionCompleted)
            {
                // if (mission.ProgressMission(this))
                // {
                //Level is completed
                return true;
                // }
            }
        }
        return false;
    }

}