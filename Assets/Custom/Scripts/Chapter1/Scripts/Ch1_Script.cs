using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 첫 번째 오두막과 관련된 대사(스토리)를 위한 코드
// 사용한 방법 : Text / string[] / List<string> / TextAsset / SetActive / Split / foreach / activeSelf / StartsWith

public class Ch1_Script : MonoBehaviour {

    public GameObject Ch1Script;
    public Text ChangeText;
    public int Textcount = 0;
    public string talk;
    string[] T_talk;
    public List<string> ListTalk = new List<string>();
    public TextAsset Story_text;
    private float time = 0f;

    void Start()
    {

        talk = Story_text.text;

        T_talk = talk.Split('\n');

        // T_talk에 담긴 내용들을 검색
        foreach (string a in T_talk)
        {
            // 리스트에 담는다.
            ListTalk.Add(a);
        }

    }

    void Update()
    {
        // 스토리가 활성화 되어 있다면 2초마다 넘어가거나, 또는 끄기 위함
        if (Ch1Script.activeSelf == true)
        {
            time += Time.deltaTime;

            if (time >= 2f)
            {
                ChangeText.text = ListTalk[Textcount];
                Textcount++;

                time = 0f;
            }
        }

        // #을 만나면 스토리 캔버스는 보이지 않는 상태가 된다.
        if (ChangeText.text.StartsWith("#"))
        {
            Ch1Script.SetActive(false);
            ChangeText.text = ListTalk[Textcount];
        }
    }
}
