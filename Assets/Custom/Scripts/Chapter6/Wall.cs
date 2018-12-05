using UnityEngine;

public class Wall : MonoBehaviour {


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "차를 타고 가는 게 좋을 거야";
        }

    }
}
