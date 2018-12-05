using UnityEngine;

public class StopTrigger4 : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "그쪽이 아니야.";

    }
}
