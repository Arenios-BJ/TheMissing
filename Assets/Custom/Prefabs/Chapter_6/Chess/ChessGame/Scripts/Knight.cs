using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman {

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        
        //위 대각선 왼쪽
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);

        //위 대각선 오른쪽
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);

        //오른쪽 대각선 위
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);

        //오른쪽 대각선 아래
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);

        //아래 대각선 왼쪽
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);

        //아래 대각선 오른쪽
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);

        //왼쪽 대각선 위
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);

        //오른쪽 대각선 아래
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);

        return r;
    }

    public void KnightMove(int x, int y, ref bool[,] r)
    {
        Chessman c;
        if(x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            c = Board_Manager.Instance.Chessmans[x, y];
            if (c == null)
                r[x, y] = true;
            else if (isWhite != c.isWhite)
                r[x, y] = true;
        }
    }
}
