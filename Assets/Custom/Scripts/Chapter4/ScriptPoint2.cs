using UnityEngine;

// BoxCollider 생성 -> 플레이어와 Trigger이벤트가 발생되면 스토리가 나온다.
// 사용한 방법 : bool / OnTriggerEnter / Find

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
