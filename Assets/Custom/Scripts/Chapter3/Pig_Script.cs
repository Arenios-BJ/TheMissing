using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pig_Script : MonoBehaviour {

    public TextAsset pigzone; //메모장(스크립트내용)을 넣기위한 변수
    public GameObject Pig_Canvas; //스크립트를 껏다 켰다 하기위한 변수
    public Text textname; //다음 스크립트(다음대사)로 넘기기위한 변수
    public string stringchange; //pigzone에 있는(외부에서 가져온 text파일) 내용을 string형으로 바꾸기위한 변수
    string[] string_array; //string형으로 바꾼 내용을 배열에 넣기위해 선언
    public List<string> text_list = new List<string>(); //string형으로 바뀐 내용이 들어가있는 배열을 string형 리스트에 넣는다.

    public int text_number = 0; // 리스트에 들어가있는 대사들을 순차적으로 부르기위한 int형 변수

    public float timer; // 대사를 canvas가 켜진후 몇초뒤에 보여줄건가를 정할 시간변수 

    private bool Pig_Script_Zone_Check; //대사를 한번만 출력하게 하기위한 bool형변수

    public GameObject StopJone;

    void Start () {
        stringchange = pigzone.text; //스크립트내용을 가지고있는 pigzone.text를 string형인 stringchange에 넣는다

        //Split이란 각 문장을 구분하기위한 기능이다 괄호안에 '\n'이 들어갔으니 줄띄우기로 구분하는거고 !를 넣으면 !로 구분한다.
        string_array = stringchange.Split('\n');

        //string_array배열안에 있는 string형들을 임시로 a라는 변수명으로 준다
        foreach (string a in string_array)
        {
            text_list.Add(a); //a들을 string형 리스트인 text_list에 넣는다.
        }

        Pig_Canvas.SetActive(false); //canvas를 처음에 꺼놔야 시작하자마자 대사가 나오는걸 막을 수 있다.

        Pig_Script_Zone_Check = true; //한번만 출력하기위해 true로 미리 선언한다.
    }


    void Update () {

        //canvas가 켜져있다면
        if(Pig_Canvas.activeSelf == true)
        {
            timer += Time.deltaTime;
            if (timer >= 1.6f)
            {
                textname.text = text_list[text_number]; //텍스트를 text_list에 있는 index에 맞게 출력하기위함이다.
                text_number++; //한번 출력되면 index를 + 1씩 올린다

                timer = 0.0f; //초기화
            }
        }
		if(textname.text.StartsWith("#")) // #으로 시작하는 문장을 찾는다 //#으로 대화의 단락을 정하는것이다
        {
            textname.text = text_list[text_number]; //이걸 넣지않으면 다음번 대화에서 #이 그대로 출력이 된다.
            Pig_Canvas.SetActive(false); //canvas를 끈다


            if (StopJone)
                Destroy(StopJone);

        }
    }

    //플레이어와 ScripZone이 부딪혔을때 대사를 던지게 하기위한 함수
    private void OnTriggerEnter(Collider other)
    {
        if (Pig_Script_Zone_Check == true) //단 한번 실행시키기 위함. start함수에서 true로 줬다.
        {
            if (other.gameObject.name == "Player") //부딪힌 대상(가해자)가 "Player"라면
            {
                Pig_Canvas.SetActive(true); //canvas를 켠다 (대사를 보여줘야하니까)
                Pig_Script_Zone_Check = false; //false를 시켜서 다시 못들어오게한다.
            }
        }
    }
}
