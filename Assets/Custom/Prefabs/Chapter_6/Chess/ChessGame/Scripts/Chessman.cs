using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chessman : MonoBehaviour {

    public int CurrentX { set; get; } //현재 위치를 나타내는 변수 Property방식으로 관리한다.
    public int CurrentY { set; get; }
    public bool isWhite; //해당말이 흑돌인지 백돌인지를 나타내는 bool형 변수


    //현재좌표를 나타내는 CurrentX와 CurrentY에 값을 넣는 함수.
    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[8, 8];
    }
}
