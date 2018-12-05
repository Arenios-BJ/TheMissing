using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ch6_Boss_Player_Talk : MonoBehaviour {

    public TextAsset TextZone;
    public GameObject Boss_Canvas;
    public Text TextChange;
    public string StringChange;
    string[] String_Array;
    public List<string> text_list = new List<string>();

    public int text_number = 0; // 리스트에 들어가있는 대사들을 순차적으로 부르기위한 int형 변수

    public float timer; // 대사를 canvas가 켜진후 몇초뒤에 보여줄건가를 정할 시간변수 

    void Start () {

        StringChange = TextZone.text; //스크립트내용을 가지고있는 pigzone.text를 string형인 stringchange에 넣는다

        //Split이란 각 문장을 구분하기위한 기능이다 괄호안에 '\n'이 들어갔으니 줄띄우기로 구분하는거고 !를 넣으면 !로 구분한다.
        String_Array = StringChange.Split('\n');

        //string_array배열안에 있는 string형들을 임시로 a라는 변수명으로 준다
        foreach (string a in String_Array)
        {
            text_list.Add(a); //a들을 string형 리스트인 text_list에 넣는다.
        }

        Boss_Canvas.SetActive(true); //canvas를 처음에 꺼놔야 시작하자마자 대사가 나오는걸 막을 수 있다.


    }

    private void OnEnable()
    {
        if (text_list.Count == 0)
        {
            return;
        }
        else
        {
            TextChange.text = text_list[text_number]; //텍스트를 text_list에 있는 index에 맞게 출력하기위함이다.
            text_number++; //한번 출력되면 index를 + 1씩 올린다
        }
    }

    void Update () {

        //canvas가 켜져있다면
        if (Boss_Canvas.activeSelf == true)
        {
            timer += Time.deltaTime;
            if (timer >= 2.0f)
            {

                timer = 0.0f; //초기화
            }
        }
        if (TextChange.text.StartsWith("#")) // #으로 시작하는 문장을 찾는다 //#으로 대화의 단락을 정하는것이다
        {
            TextChange.text = text_list[text_number]; //이걸 넣지않으면 다음번 대화에서 #이 그대로 출력이 된다.
            Boss_Canvas.SetActive(false); //canvas를 끈다
            
        }

    }
}
