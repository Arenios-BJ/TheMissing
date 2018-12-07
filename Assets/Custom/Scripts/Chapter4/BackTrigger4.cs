using UnityEngine;

// 플레이어가 다른 곳으로 이동하지 못하게 막음
// 사용한 방법 : OnCollisionEnter / Find / SetActive

public class BackTrigger4 : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "이쪽은 위험해";
    }
}
