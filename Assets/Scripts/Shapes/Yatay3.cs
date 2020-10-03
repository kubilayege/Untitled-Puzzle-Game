using System;
using UnityEngine;

public class Yatay3 : Shape
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
        return false;
    }

    private static bool CheckForPiece(int i, int j)
    {
        if (i >= 7)
            return false;

        for (int k = 0; k < 3; k++)
        {
            Tuple<int, int> tempPos = new Tuple<int, int>(i + k, j);
            if (GameManager.cells.ContainsKey(tempPos) && (GameManager.cells[tempPos].isFull || GameManager.cells[tempPos].blocked))
            {
                return false;
            }
        }
        return true;
    }
}