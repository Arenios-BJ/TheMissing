using UnityEngine;

// 챕터2로 넘어갔을 때, 다시 챕터1로 돌아가지 못하도록 만든 스크립트

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
