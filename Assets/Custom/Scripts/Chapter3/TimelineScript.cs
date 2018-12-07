using UnityEngine;
using UnityEngine.Playables;

// 적 베이스에서 보스와 부하들이 대화하는 장면 연출 스크립트
// 사용한 방법 : PlayableDirector / bool / BoxCollider / Find / SetActive / Destroy

public class TimelineScript : MonoBehaviour {

    PlayableDirector playerdirector;

    public GameObject timee;

    private bool timeCheck;

    private BoxCollider box;

    public GameObject Trig;

    public BoxCollider cctvBackTrigger;

	void Start () {

        playerdirector = GameObject.Find("SkelMesh_Bodyguard_01").GetComponent<PlayableDirector>();
        box = GetComponent<BoxCollider>();
        timee.SetActive(false);

        timeCheck = false;
    }
	
	void Update () {

        if (timeCheck == true)
        {
            if (playerdirector.time > 17.3f)
            {
                Destroy(timee);
                timeCheck = false;
            }
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Player")
        {
            if (timeCheck == false)
            {
                playerdirector.Play();
                timee.SetActive(true);
                Destroy(box);
                timeCheck = true;
                GameObject.Find("Ch3_Story").GetComponent<Ch3_Script>().Ch3Script.SetActive(true);
                cctvBackTrigger.isTrigger = false;
                Destroy(Trig);
            }
        }
    }

}
