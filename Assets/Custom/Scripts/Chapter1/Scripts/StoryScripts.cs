using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryScripts : MonoBehaviour {

    public GameObject Story2;

    public Text ChangeText2;

    public int Textcount = 0;

    public string talk;

    string[] T_talk;

    public List<string> ListTalk = new List<string>();

    public TextAsset Story_text;

    private float time2 = 0f;

    void Start () {

        talk = Story_text.text;

        T_talk = talk.Split('\n');
        foreach (string a in T_talk)
        {
            ListTalk.Add(a);
        }

    }

    void Update () {

        if (Story2.activeSelf == true)
        {
            time2 += Time.deltaTime;

            if (time2 >= 2f)
            {
                ChangeText2.text = ListTalk[Textcount];
                Textcount++;

                time2 = 0f;
            }
        }

        if (ChangeText2.text.StartsWith("#"))
        {
            Story2.SetActive(false);
            ChangeText2.text = ListTalk[Textcount];
        }
    }
}
