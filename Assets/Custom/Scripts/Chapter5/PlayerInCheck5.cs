using UnityEngine;

public class PlayerInCheck5 : MonoBehaviour {

    public AudioSource PowerRoom;

    void Start()
    {
        PowerRoom.enabled = false;
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<FirstPersonCamera>().In = true;
            GameObject.Find("_Environments").GetComponent<BackSound>().back.enabled = false;
            PowerRoom.enabled = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<FirstPersonCamera>().In = false;
            GameObject.Find("_Environments").GetComponent<BackSound>().back.enabled = true;
            PowerRoom.enabled = false;
        }
    }
}
