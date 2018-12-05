using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i, j;

        //위쪽 
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 || i < 8)
                {
                    c = Board_Manager.Instance.Chessmans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isWhite != c.isWhite)
                        r[i, j] = true;
                }
                i++;
            }
        }

        //아래 쪽
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 || i < 8)
                {
                    c = Board_Manager.Instance.Chessmans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isWhite != c.isWhite)
                        r[i, j] = true;
                }
                i++;
            }
        }

        //왼쪽
        if (CurrentX != 0)
        {
            c = Board_Manager.Instance.Chessmans[CurrentX - 1, CurrentY];
            if (c == null)
                r [CurrentX - 1, CurrentY] = true;
            else if (isWhite != c.isWhite)
                r [CurrentX - 1, CurrentY] = true;
        }

        //오른쪽
        if (CurrentX != 7)
        {
            c = Board_Manager.Instance.Chessmans[CurrentX + 1, CurrentY];
            if (c == null)
                r[CurrentX + 1, CurrentY] = true;
            else if (isWhite != c.isWhite)
                r[CurrentX + 1, CurrentY] = true;
        }

        return r;
    }

}
