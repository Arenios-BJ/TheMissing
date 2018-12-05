using UnityEngine;

public class StopTrigger2 : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "어서 숲을 나가야해. 다리로 가자!";
        }
    }
}
