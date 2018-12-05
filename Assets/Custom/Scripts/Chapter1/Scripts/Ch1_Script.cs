using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        foreach (string a in T_talk)
        {
            ListTalk.Add(a);
        }

    }

    void Update()
    {
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

        if (ChangeText.text.StartsWith("#"))
        {
            Ch1Script.SetActive(false);
            ChangeText.text = ListTalk[Textcount];
        }
    }
}
