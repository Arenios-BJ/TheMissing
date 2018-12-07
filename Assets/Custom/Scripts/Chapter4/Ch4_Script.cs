using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 보스의 집과 관련된 대사(스토리)를 위한 코드
// 사용한 방법 : Text / string[] / List<string> / TextAsset / SetActive / Split / foreach / activeSelf / StartsWith

public class Ch4_Script : MonoBehaviour {

    public GameObject Ch4Script;
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
        foreach (string a in T_talk)
        {
            ListTalk.Add(a);
        }

        Ch4Script.SetActive(false);

    }

    void Update()
    {
        if (Ch4Script.activeSelf == true)
        {
            time += Time.deltaTime;

            if (time >= 2f)
            {
                ChangeText.text = ListTalk[Textcount];
                Textcount++;

                time = 0f;
            }
        }

        if (ChangeText.text.StartsWith("#"))
        {
            Ch4Script.SetActive(false);
            ChangeText.text = ListTalk[Textcount];
        }
    }
}
