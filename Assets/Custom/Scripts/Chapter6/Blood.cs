using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

// 차와 원주민이 부딪혔을 때 피가 튀는 것을 연출하기 위해 만들었음 -> 피가 튀는 것을 자연스럽게 보이기 위해서.
// 사용한 방법 : PlayableDirector / bool / SetActive / playerdirector.time / IEnumerator

public class Blood : MonoBehaviour {

    PlayableDirector playerdirector;

    public GameObject bl;
    public GameObject bl2;
    public GameObject bl3;
    public GameObject bl4;
    public GameObject bl5;

    float count;

    private bool exit;

    void Start () {

        bl.SetActive(false);
        bl2.SetActive(false);
        bl3.SetActive(false);
        bl4.SetActive(false);
        bl5.SetActive(false);

        exit = false;

        playerdirector = GameObject.Find("Car").GetComponent<PlayableDirector>();

    }
	
	void Update () {

        // 타임라인에서 차와 원주민이 부딪히는 시간을 체크해서 코루틴 실행
        if (exit == false)
        {
            if (playerdirector.time >= 9f)
            {
                StartCoroutine("Bld");
            }

            if(playerdirector.time >= 11f)
            {
                StartCoroutine("Bld2");
                exit = true;
            }
        }
	}

    // 여기서 코루틴을 쓴 이유는 피가 뿌려지는 것처럼 보여지게 하기 위해서임
    IEnumerator Bld()
    {
        bl.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);

        bl2.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);

        bl3.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);
    }

    IEnumerator Bld2()
    {
        bl4.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);

        bl5.SetActive(true);
        yield return new WaitForSecondsRealtime(0.3f);

    }
}
