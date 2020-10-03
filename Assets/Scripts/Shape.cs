using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public int id;
    public int spawnIndex;
    private Vector3 spawnPos;
    private Vector3 tempPos;
    public string shapeType;
    public Vector3 SpawnPos
    {
        get => spawnPos;
        set => spawnPos = value;
    }
    void Interpolate(float _elapsed)
    {
        transform.position = Vector3.Lerp(transform.position, tempPos + new Vector3(0, 1, 0), _elapsed);
    }
    public void Init()
    {
        StartCoroutine(Timer.TimeSteps(0.2f, 0.01f,
            () =>
            {
                tempPos = this.transform.position;
            },
            Interpolate,
            () =>
            {
                SpawnPos = tempPos + new Vector3(0, 1, 0);
            }));
    }

    public abstract bool CanFit();
}