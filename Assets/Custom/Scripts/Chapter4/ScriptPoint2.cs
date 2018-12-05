using UnityEngine;

public class ScriptPoint2 : MonoBehaviour {

    private bool TriggerCheck;

    void Start () {

        TriggerCheck = false;


    }
	

    void OnTriggerEnter(Collider col)
    {
        if (TriggerCheck == false)
        {
            GameObject.Find("ScriptPoint").GetComponent<Ch4_Script>().Ch4Script.SetActive(true);
            TriggerCheck = true;
        }
    }
}
