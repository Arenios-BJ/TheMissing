using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chessman {

    //
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8]; //bool형의 2중배열을 선언하고  8x8배열을 동적 생성한다.
        Chessman c, c2; //Chessman형 변수 2개를 선언한다.

        // White team move
        if (isWhite) //백돌일때
        {
            //대각선 왼쪽
            if(CurrentX != 0 && CurrentY != 7)
            {
                c = Board_Manager.Instance.Chessmans[CurrentX - 1, CurrentY + 1]; //c변수에 Chessmans배열의 인자값을 넣어준다
                if (c != null && !c.isWhite) //c변수가 null값이 아니라면 그리고 c변수의 isWhite(bool)이 반대라면
                    r[CurrentX - 1, CurrentY + 1] = true; //bool배열을 true로 바꿔준다.
            }

            //대각선 오른쪽
            if (CurrentX != 7 && CurrentY != 7)
            {
                c = Board_Manager.Instance.Chessmans[CurrentX + 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                    r[CurrentX + 1, CurrentY + 1] = true;
            }

            //앞으로 전진
            if (CurrentY != 7)
            {
                c = Board_Manager.Instance.Chessmans[CurrentX, CurrentY + 1];
                if (c == null)
                    r[CurrentX, CurrentY + 1] = true;
            }

            //처음으로 움직일때 앞으로 전진(2칸갈 수 있음)
            if (CurrentY == 1)
            {
                c = Board_Manager.Instance.Chessmans[CurrentX, CurrentY + 1];
                c2 = Board_Manager.Instance.Chessmans[CurrentX, CurrentY + 2];
                if (c == null & c2 == null)
                    r[CurrentX, CurrentY + 2] = true;
            }

        }
        else
        {
            //대각선 왼쪽
            if (CurrentX != 0 && CurrentY != 0)
            {
                c = Board_Manager.Instance.Chessmans[CurrentX - 1, CurrentY - 1];
                if (c != null && c.isWhite)
                    r[CurrentX - 1, CurrentY - 1] = true;
            }

            //대각선 오른쪽
            if (CurrentX != 7 && CurrentY != 0)
            {
                c = Board_Manager.Instance.Chessmans[CurrentX + 1, CurrentY - 1];
                if (c != null && c.isWhite)
                    r[CurrentX + 1, CurrentY - 1] = true;
            }

            //앞으로 전진
            if (CurrentY != 0)
            {
                c = Board_Manager.Instance.Chessmans[CurrentX, CurrentY - 1];
                if (c == null)
                    r[CurrentX, CurrentY - 1] = true;
            }

            //처음으로 움직일때 앞으로 전진(2칸갈 수 있음)
            if (CurrentY == 6)
            {
                c = Board_Manager.Instance.Chessmans[CurrentX, CurrentY - 1];
                c2 = Board_Manager.Instance.Chessmans[CurrentX, CurrentY - 2];
                if (c == null & c2 == null)
                    r[CurrentX, CurrentY - 2] = true;
            }
        }

        return r;
    }

}
