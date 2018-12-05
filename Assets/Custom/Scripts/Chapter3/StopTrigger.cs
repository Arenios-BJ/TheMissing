using UnityEngine;

public class StopTrigger : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "어서 숲을 나가야해. 다리로 가자!";
    }
}
