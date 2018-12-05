using UnityEngine;

public class CabinCol : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "으악! 뜨거워";
        }

    }
}
