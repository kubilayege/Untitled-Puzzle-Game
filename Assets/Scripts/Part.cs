using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public bool isPlaced;
    public bool hasBuble;
    public GameObject buble;
    public CellManager cellBellow;
    public void DestroyPart()
    {
        StartCoroutine(Timer.TimeSteps(0.2f, 0.01f, onStartEvent, onUpdateEvent, onCompleteEvent));
    }

    public void onStartEvent()
    {



        if (cellBellow.mission != null)
            cellBellow.mission.Count -= cellBellow.mission.Count <= 0 ? 0 : 1;

        // if (cellBellow.mission != null)
        //     LevelManager.instance.SaveMissionProgress(cellBellow.mission);
    }

    public void onUpdateEvent(float elapsedValue)
    {
        this.GetComponent<SpriteRenderer>().color = Color.Lerp(this.GetComponent<SpriteRenderer>().color, Color.black, elapsedValue);
        this.transform.localScale *= 0.9f;
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y * 1.01f, this.transform.localScale.z);
        if (cellBellow.mission != null && !cellBellow.mission.missionCompleted)
        {
            if (this.transform.position.y < 10)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, cellBellow.mission.animalModel.transform.position.y, cellBellow.mission.animalModel.transform.position.z), elapsedValue);
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, cellBellow.mission.animalModel.transform.position, elapsedValue);
            }
        }
    }

    public void onCompleteEvent()
    {
        if (cellBellow.mission != null)
            cellBellow.mission.ProgressMission(cellBellow);
        Destroy(this.gameObject);
    }

    public bool Explode()
    {
        DestroyPart();
        return true;
    }
}