using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ch6_Script : MonoBehaviour {

    public GameObject Ch6Script;
    public Text ChangeText;
    public int Textcount = 0;
    public string talk;
    string[] T_talk;
    public List<string> ListTalk = new List<string>();
    public TextAsset Story_text;
    private float time = 0f;

    void Start()
    {
        Ch6Script.SetActive(false);

        talk = Story_text.text;

        T_talk = talk.Split('\n');
        foreach (string a in T_talk)
        {
            ListTalk.Add(a);
        }

    }

    void Update()
    {
        if (Ch6Script.activeSelf == true)
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
            Ch6Script.SetActive(false);
            ChangeText.text = ListTalk[Textcount];
        }
    }
}