using UnityEngine;

// 플레이어가 집 안에 있는지, 밖에 있는지 체크함
// 사용한 방법 : OnTriggerStay / OnTriggerExit / Find / enabled

public class PlayerIncheck2 : MonoBehaviour {

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            GameObject.Find("Player").GetComponent<FirstPersonCamera>().In = true;
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
