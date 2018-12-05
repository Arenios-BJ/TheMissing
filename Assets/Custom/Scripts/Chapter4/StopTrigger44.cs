using UnityEngine;

public class StopTrigger44 : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "지금은 우선 집 안으로 들어가자.";
        }

    }
}
