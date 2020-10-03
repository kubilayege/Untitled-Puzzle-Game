using System;
using UnityEngine;

public abstract class Booster : MonoBehaviour
{
    public abstract void Place(CellManager.Pos pos);
}