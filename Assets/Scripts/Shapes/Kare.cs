using System;
using UnityEngine;

public class Kare : Shape
{
    public override bool CanFit()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Tuple<int, int> pos = new Tuple<int, int>(i, j);
                if (!GameManager.cells[pos].isFull && !GameManager.cells[pos].blocked)
                {
                    //Debug.Log("CanFit"  + " " + i + j);
                    if (CheckForPiece(i, j))
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        //Debug.Log("CanFit"+"false");
        return false;
    }

    private static bool CheckForPiece(int i, int j)
    {
        if (i == 8 || j == 8)
            return false;


        //Debug.Log("Checking" + i + " " + j);
        for (int k = 0; k < 2; k++)
        {
            for (int m = 0; m < 2; m++)
            {
                Tuple<int, int> tempPos = new Tuple<int, int>(k + i, m + j);
                if (GameManager.cells.ContainsKey(tempPos) && (GameManager.cells[tempPos].isFull || GameManager.cells[tempPos].blocked))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
