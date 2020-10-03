using System;
using UnityEngine;

public class Tek : Shape
{

    public override bool CanFit()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Tuple<int, int> pos = new System.Tuple<int, int>(i, j);
                if (GameManager.cells.ContainsKey(pos) && (!GameManager.cells[pos].isFull && !GameManager.cells[pos].blocked))
                {
                    return true;
                }
            }
        }
        return false;
    }

}
