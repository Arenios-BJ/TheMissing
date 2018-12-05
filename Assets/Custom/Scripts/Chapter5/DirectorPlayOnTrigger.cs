using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class DirectorPlayOnTrigger : MonoBehaviour {

    [SerializeField] private GameObject director;
    [SerializeField] private ForTimelineScript scripts;
    private FirstPersonCamera player;

    private Inventory inven;

    private void Start()
    {
        player = FirstPersonCamera.player;
        inven = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    private IEnumerator Timer()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float t = 0;
        float d = (float)director.GetComponent<PlayableDirector>().playableAsset.duration;
        while (t < d)
        {           
            t += Time.deltaTime;
            yield return wait;
        }
        director.GetComponent<PlayableDirector>().Stop();

        // 타임라인 종료 후 씬 정리 및 호출.
        if (SceneManager.GetSceneByName("Chapter4").isLoaded)
            SceneManager.UnloadSceneAsync(5);
        if (SceneManager.GetSceneByName("Chapter5").isLoaded)
            SceneManager.UnloadSceneAsync(6);
        SceneManager.LoadSceneAsync(7, LoadSceneMode.Additive);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (inven.Circle1 == true)
                    {
                        if (player.getRaycastHit().transform == transform)
                        {
                            GameObject.Find("Electric_House").GetComponent<AudioSource>().enabled = false;
                            GameObject.Find("_Environments").GetComponent<BackSound>().back.enabled = true;
                            // 타임라인 실행.
                            scripts.SetScript(2);
                            director.GetComponent<PlayableDirector>().Play();

                            StartCoroutine("Timer");

                            // 아웃라인 처리를 위해 태그 변경, 콜라이더 제거 및 스크립트 제거.
                            gameObject.tag = "Untagged";
                            var tmp = GetComponents<BoxCollider>();
                            for (int i = 0; i < tmp.Length; i++)
                            {
                                if (tmp[i].isTrigger)
                                {
                                    Destroy(tmp[i]);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
