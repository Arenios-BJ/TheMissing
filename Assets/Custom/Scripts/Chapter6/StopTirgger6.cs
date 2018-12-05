using UnityEngine;

public class StopTirgger6 : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "이쪽은 위험해!";
        }
    }
}
