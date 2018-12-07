using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 두 번째 오두막의 스토리를 위한 스크립트
// 사용한 방법 : Text / string[] / List<string> / TextAsset / SetActive / Split / foreach / activeSelf / StartsWith

public class Ch2_Script : MonoBehaviour {

    public GameObject Ch2Script;
    public Text ChangeText;
    public int Textcount = 0;
    public string talk;
    string[] T_talk;
    public List<string> ListTalk = new List<string>();
    public TextAsset Story_text;
    private float time = 0f;

    void Start()
    {
        Ch2Script.SetActive(false);

        talk = Story_text.text;

        T_talk = talk.Split('\n');
        foreach (string a in T_talk)
        {
            ListTalk.Add(a);
        }

    }

    void Update()
    {
        if (Ch2Script.activeSelf == true)
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
            Ch2Script.SetActive(false);
            ChangeText.text = ListTalk[Textcount];
        }
    }
}

