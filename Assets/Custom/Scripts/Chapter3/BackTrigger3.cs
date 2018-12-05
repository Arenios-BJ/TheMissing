using UnityEngine;

public class BackTrigger3 : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "왔던 길이야. 얼른 이곳을 벗어나야돼.";

    }
}
