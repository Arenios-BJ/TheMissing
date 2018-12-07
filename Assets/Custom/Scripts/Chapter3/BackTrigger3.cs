using UnityEngine;

// 플레이어가 정해진 길이 아닌, 다른 길로 가려고할 때 막기 위함
// 사용한 방법 : OnCollisionEnter / SetActive / Find

public class BackTrigger3 : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "왔던 길이야. 얼른 이곳을 벗어나야돼.";

    }
}
