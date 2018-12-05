using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForTimelineScript : MonoBehaviour
{
    public TextAsset[] textAssets;
    public Text TextChange;
    public string StringChange;
    public string[] String_Array;
    public List<string> text_list = new List<string>();

    public int text_number = 0; // 리스트에 들어가있는 대사들을 순차적으로 부르기위한 int형 변수

    private void OnEnable()
    {
        // 현재 출력할 스크립트 리스트의 갯수가 '0'이면 바로 return. 하위 코드는 실행하지 않는다.
        if (text_list.Count == 0)
        {
            TextChange.text = "";
            return;
        }

        TextChange.text = text_list[text_number]; //텍스트를 text_list에 있는 index에 맞게 출력하기위함이다.
        text_number++; //한번 출력되면 index를 + 1씩 올린다

        // 출력 index와 스크립트 리스트의 갯수가 같다면(스크립트 변경 후 index를 증가시키기 때문에 가능)
        if (text_number == text_list.Count)
        {
            // 리스트를 비우고 index를 0으로 초기화.
            text_list.Clear();
            text_number = 0;
        }
    }

    public void SetScript(int idx)
    {
        StringChange = textAssets[idx].text; //스크립트 내용을 가지고 있는 txt 파일을 string형인 stringchange에 넣는다

        //Split이란 각 문장을 구분하기위한 기능이다 괄호안에 '#'이 들어갔으니 줄띄우기로 구분하는거고 !를 넣으면 !로 구분한다.
        String_Array = StringChange.Split('#');

        //string_array배열안에 있는 string형들을 임시로 a라는 변수명으로 준다
        foreach (string a in String_Array)
        {
            text_list.Add(a); //a들을 string형 리스트인 text_list에 넣는다.
        }
        
        // 출력 index 초기화.
        text_number = 0;
        gameObject.SetActive(false);
    }
}
