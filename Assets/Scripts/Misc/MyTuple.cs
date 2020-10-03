using UnityEngine;
using System;
[Serializable]
public class MyTuples
{
    public int x;
    public int y;
    public bool isFull;

    public MyTuples(int X, int Y)
    {
        x = X;
        y = Y;
        isFull = false;
    }
}
