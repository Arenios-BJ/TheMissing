using UnityEngine;

// BoxCollider 생성 -> 플레이어와 Trigger이벤트가 발생되면 스토리가 나온다.
// 사용한 방법 : bool / OnTriggerEnter / Find / SetActive / enabled

public class MunJaPoint : MonoBehaviour {

    private bool MunjaCheck;
    public AudioSource Hole;

    public AudioSource FireSound;
    void Start () {

        MunjaCheck = false;
        Hole.enabled = false;

        if (GameObject.Find("Ch6_BossRoom_TimeLine"))
            FireSound = GameObject.Find("Ch6_BossRoom_TimeLine").GetComponent<Ch6_SceneChange>().Fire_Sound1;
    }
	

    void OnTriggerEnter(Collider col)
    {
       if(col.gameObject.name == "Player")
       {
            if(MunjaCheck == false)
            {
                GameObject.Find("Ch6_Story").GetComponent<Ch6_Script>().Ch6Script.SetActive(true);
                Hole.enabled = true;
                MunjaCheck = true;

                if (FireSound)
                    FireSound.Stop();
            }

       }
    }
}
