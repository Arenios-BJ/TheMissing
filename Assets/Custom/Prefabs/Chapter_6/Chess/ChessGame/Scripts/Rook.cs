using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        Chessman c;
        int i;

        //오른쪽
        i = CurrentX;
        while(true)
        {
            i++;
            if (i >= 8)
                break;

            c = Board_Manager.Instance.Chessmans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;

                break;
            }
        }

        //왼쪽
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = Board_Manager.Instance.Chessmans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;

                break;
            }
        }

        //위
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8)
                break;

            c = Board_Manager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;

                break;
            }
        }

        //아래
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = Board_Manager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;

                break;
            }
        }
        return r;
    }
}
