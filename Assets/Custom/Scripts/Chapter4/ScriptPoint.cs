using UnityEngine;

public class ScriptPoint : MonoBehaviour {

    private bool TriggerCheck;

    private GameObject stop5;

	void Start () {

        TriggerCheck = false;

        stop5 = GameObject.Find("StopTrigger555");
    }
	

    void OnTriggerEnter(Collider col)
    {
        if(TriggerCheck == false)
        {
            GameObject.Find("ScriptPoint").GetComponent<Ch4_Script>().Ch4Script.SetActive(true);
            TriggerCheck = true;

            Destroy(stop5);
        }
    }
}
