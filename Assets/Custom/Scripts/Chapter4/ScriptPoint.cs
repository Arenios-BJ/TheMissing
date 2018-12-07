using UnityEngine;

// BoxCollider 생성 -> 플레이어와 Trigger이벤트가 발생되면 스토리가 나온다.
// 사용한 방법 : bool / Find / Destroy

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
