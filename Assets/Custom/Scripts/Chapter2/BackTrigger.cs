using UnityEngine;

// 플레이어가 정해진 길이 아닌, 다른 길로 갔을 때 막기 위함
// 사용한 방법 : OnCollisionEnter / Find / SetActive

public class BackTrigger : MonoBehaviour {
    public Transform afterPoint;

    void OnCollisionEnter(Collision col)
    {
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "북쪽으로 계속 이동해야 해.";
    }

    public void setPosition()
    {
        transform.position = afterPoint.position;
        transform.rotation = afterPoint.rotation;
    }
}
