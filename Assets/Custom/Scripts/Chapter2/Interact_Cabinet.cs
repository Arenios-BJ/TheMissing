using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Interact_Cabinet : MonoBehaviour {
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableDirector bossAndDoctor;
    [SerializeField] private TimelineAsset In;
    [SerializeField] private TimelineAsset Out;
    [SerializeField] private GameObject window;
    [SerializeField] private ForTimelineScript scripts;
    [SerializeField] private GameObject bookTrigger;
    public bool isOpened = false;

    private void Start()
    {
        if (!director)
            if (GetComponent<PlayableDirector>())
                director = GetComponent<PlayableDirector>();

        // 타임라인에 오브젝트 바인딩. Interact_Window.cs->Start() 참고.
        if (GameObject.FindWithTag("Player"))
        {
            GameObject player = GameObject.FindWithTag("Player");
            // 캐비닛에 들어가는 타임라인.
            TimelineAsset tmp = director.playableAsset as TimelineAsset;
            director.SetGenericBinding(tmp.GetOutputTrack(0), player);

            tmp = bossAndDoctor.playableAsset as TimelineAsset;
            // Interact_Window.cs에서 보스와닥터 타임라인에 플레이어 카메라를 할당을 하나, 만약으로 안되어 있을 경우.
            if (bossAndDoctor.GetGenericBinding(tmp.GetOutputTrack(2)) == null)
                bossAndDoctor.SetGenericBinding(tmp.GetOutputTrack(2), player.GetComponentInChildren<Camera>().gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.gm.panelOpen == false)
            {
                if (other.CompareTag("Player"))
                {
                    if (tag == "Handle")
                    {
                        // 타임라인이 실행중이 아닐 때.
                        if (director.state != PlayState.Playing)
                        {
                            // 플레이어의 이동 및 회전 잠금.
                            FirstPersonCamera.player.ChangeMoveRotaState(false);

                            if (director && In)
                            {
                                director.playableAsset = In;
                                director.SetGenericBinding((director.playableAsset as TimelineAsset).GetOutputTrack(1), GameObject.FindWithTag("Player"));
                                director.played += In_Played;
                                director.stopped += In_Stopped;
                                director.Play();
                            }
                        }
                    }
                    else if (tag == "Outline" && isOpened == false)
                    {
                        scripts.SetScript(0);
                        FirstPersonCamera.player.ChangeMoveRotaState(false);
                        GetComponent<SphereCollider>().enabled = false;
                        director.stopped += search_Stopped;
                        director.Play();
                    }
                }
            }
        }
    }

    private void search_Stopped(PlayableDirector obj)
    {
        FirstPersonCamera.player.ChangeMoveRotaState(true);
        isOpened = true;
        director.stopped -= search_Stopped;
    }

    private void In_Played(PlayableDirector obj)
    {
        // 아웃라인이 켜지지 않게 태그 변경.
        gameObject.tag = "Untagged";

        // 더이상 타임라인이 실행되지 않게 트리거 콜라이더 제거.
        if (GetComponent<SphereCollider>())
            Destroy(GetComponent<SphereCollider>());
    }

    private void In_Stopped(PlayableDirector obj)
    {
        // 커스텀 이벤트 제거.
        director.played -= In_Played;
        director.stopped -= In_Stopped;

        if (bossAndDoctor)
        {
            // 보스닥터 타임라인용 스크립트로 설정.
            scripts.SetScript(2);

            // 보스닥터 타임라인에 이벤트 추가 및 재생.
            bossAndDoctor.stopped += BossAndDoctor_stopped;
            bossAndDoctor.Play();
        }
    }

    private void BossAndDoctor_stopped(PlayableDirector obj)
    {
        if (Out)
        {
            // 보스와닥터 연출 시에 사용한 오두막의 문의 태그를 제거.
            bossAndDoctor.transform.Find("cctvdoor").tag = "Untagged";

            // PlayableDirector 컴포넌트의 타임라인을 변경, 플레이어 바인딩, 이벤트 추가.
            director.playableAsset = Out;
            director.SetGenericBinding((director.playableAsset as TimelineAsset).GetOutputTrack(1), GameObject.FindWithTag("Player"));
            director.stopped += Out_Stopped;
            //In.Play(Out);   // Play() 함수의 인자로 다른 TimelineAsset을 넘겨줌으로 그 타임라인을 재생.
            director.Play();
        }
    }

    private void Out_Stopped(PlayableDirector obj)
    {
        if (window) // 캐비닛으로 진행을 했으니 창문의 콜라이더 모두 제거.
        {
            Destroy(window.GetComponent<CapsuleCollider>());
            Destroy(window.GetComponent<BoxCollider>());
        }

        // DoctorBook 오브젝트가 Chapter2 씬의 RootObjejct 중 하나이므로 scene.GetRootGameObjects()로 찾아 활성화 시킨다.
        GameObject[] tmp = gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < tmp.Length; i++)
        {
            Debug.Log(tmp[i].name);
            if (tmp[i].name == "DoctorBook")
            {
                tmp[i].SetActive(true);
                bookTrigger.SetActive(true);
                break;
            }
        }

        FirstPersonCamera.player.ChangeMoveRotaState(true);

        // 추가한 커스텀 이벤트 삭제.
        bossAndDoctor.stopped -= BossAndDoctor_stopped;
        bossAndDoctor = null;
        director.stopped -= Out_Stopped;
        director = null;
    }
}
