using UnityEngine;

// BoxCollider 생성 -> 플레이어와 Trigger이벤트가 발생되면 스토리가 나온다.
// 사용한 방법 : bool / OnTriggerEnter / Find / SetActive / enabled

public class StroyPoint : MonoBehaviour {

    private bool Check;

	void Start () {

        Check = false;

    }
	

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Check == false)
            {
                GameObject.Find("Ch6_Story").GetComponent<Ch6_Script>().Ch6Script.SetActive(true);
                Check = true;
                GameObject.Find("MunJaPoint").GetComponent<MunJaPoint>().Hole.enabled = false;
            }
        }
    }
}
