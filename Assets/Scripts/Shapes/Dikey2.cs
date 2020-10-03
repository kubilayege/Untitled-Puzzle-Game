using System;
using UnityEngine;

public class Dikey2 : Shape
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
        if (j == 8)
            return false;
        for (int k = 0; k < 2; k++)
        {
            Tuple<int, int> tempPos = new Tuple<int, int>(i, j + k);
            if (GameManager.cells.ContainsKey(tempPos) && (GameManager.cells[tempPos].isFull || GameManager.cells[tempPos].blocked))
            {
                return false;
            }
        }
        return true;
    }

}
