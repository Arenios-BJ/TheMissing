using UnityEngine;

public class PlayerInCheck : MonoBehaviour {

    public void OnTriggerStay(Collider col)
    {
        if(col.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<FirstPersonCamera>().In = true;
            GameObject.Find("_Environments").GetComponent<BackSound>().back.enabled = false;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<FirstPersonCamera>().In = false;
            GameObject.Find("_Environments").GetComponent<BackSound>().back.enabled = true;
        }
    }
}
