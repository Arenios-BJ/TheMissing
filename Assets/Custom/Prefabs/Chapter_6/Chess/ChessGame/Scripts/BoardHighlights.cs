using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour {

    public static BoardHighlights Instance { set; get; }

    public GameObject highlightPrefab;
    private List<GameObject> highlights;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>(); //전역에다가 선언해도 문제는 없다. 그냥 개발자의 마음
    }

    private GameObject GetHighlightObject()
    {
        //g 는 highlights List 안의 요소를 의미하며 (변수이기 때문에 g 라는 이름이 아니여도 동작합니다)
        //우측의 => 는 조건을 의미합니다.
        //그래서 highlights List 안에서 active 가 false 인 GameObject 를 하나 찾게 됩니다.
        GameObject go = highlights.Find(g => !g.activeSelf); //람다식 과 델리게이트를 알아보자. (Predicate 델리게이트라는 기능)

        if (go == null) //go가 null값이라면
        {
            go = Instantiate(highlightPrefab); //highlightPrefab을 생성한다
            highlights.Add(go); //Prefab을 집어넣은 go변수를 highlights 리스트에 추가한다
        }

        return go; //go를 반환한다.
    }

    //움직일 수 있는 장소에 하이라이트(prefab)을 활성화 시켜줄것이다.
    public void HighlightAllowedMoves(bool[,] moves)
    {
        for(int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves [i, j])
                {
                    GameObject go = GetHighlightObject(); //GetHighlightObject함수에서 생성한 highlightPrefab을 go변수안에 넣는다.
                    go.SetActive(true); //하이라이트를 활성화 시킨다.
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f); //하이라이트의 위치를 잡아주기위함.
                }
            }
        }
    }

    public void Hidehighlights() //하이라이트를 숨기는 함수
    {
        foreach (GameObject go in highlights) //highlights 리스트안에 있는 모든 GameObject들을 임시로 go라는 변수명을 준다.
            go.SetActive(false); //모든 go GameObject를 비활성화시킨다.
    }
}
