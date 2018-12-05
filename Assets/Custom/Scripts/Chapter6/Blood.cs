using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

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
