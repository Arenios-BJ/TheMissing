using UnityEngine;

public class Wall2 : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {

        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "저 앞에 차가 보인다. 저걸 타면 될 것 같아.";

    }
}
