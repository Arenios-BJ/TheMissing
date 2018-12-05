using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Interact_Window : MonoBehaviour {
    private CapsuleCollider capsuleColl;    // 충돌 판단 트리거 콜라이더.(attached)
    private GameObject player;
    private bool isIn = false;

    //[SerializeField] private SpriteRenderer notice; // 상호작용 안내 문구를 띄우는 컴포넌트.
    //private GameObject noticeGO;    // notice가 붙어있는 GO의 부모 GO. 빌보드는 이 GO의 회전으로 작동된다.

    [SerializeField] private PlayableDirector comeIn;   // 안으로 들어가는 타임라인.
    [SerializeField] private PlayableDirector comeOut;  // 밖으로 나가는 타임라인.
    [SerializeField] private PlayableDirector timeline; // 보스와 닥터의 타임라인.
    [SerializeField] private GameObject cabinet;
    [SerializeField] private ForTimelineScript scripts;
    [SerializeField] private GameObject forwardTrigger;
    [SerializeField] private GameObject bookTrigger;
    [SerializeField] private GameObject goHouseTrigger;
    public GameObject backTrigger;

    private QuickOutline leftOutline, rightOutline;

    private void Start()
    {
        capsuleColl = GetComponent<CapsuleCollider>();
        capsuleColl.isTrigger = true;

        // 다른 Scene에 있는 플레이어 GO를 찾아, 플레이어 GO를 필요로 하는
        // 타임라인(실질적으로는 Playable Director 컴포넌트)에 바인딩.
        if (GameObject.FindWithTag("Player"))
        {
            GameObject player = GameObject.FindWithTag("Player");

            // 오두막 안으로 들어가는 타임라인부터 할당.
            // 트랙 정보는 Timeline에 있기 때문에 PlayableDirector 컴포넌트에서 할당되어 있는 TimelineAsset을 가져온다.
            // PlayableAsset 클래스가 TimelineAsset의 부모(슈퍼) 클래스이기 때문에 형변환 가능.
            TimelineAsset tmp = comeIn.playableAsset as TimelineAsset;
            // PlayableDirector.SetGenericBindding(a, b) a = 오브젝트(b)를 바인딩할 트랙, b = 트랙(a)에 바인딩할 오브젝트.
            // TimelineAsset.GetOutputTrack(n) n = 타임라인에 있는 n+1번째 트랙을 반환한다. (인덱스는 0부터 시작하니깐)
            comeIn.SetGenericBinding(tmp.GetOutputTrack(2), player);

            // 오두막 밖으로 나가는 타임라인에 할당.
            tmp = comeOut.playableAsset as TimelineAsset;
            comeOut.SetGenericBinding(tmp.GetOutputTrack(0), player);

            // 보스와 닥터 타임라인에 할당.
            tmp = timeline.playableAsset as TimelineAsset;
            timeline.SetGenericBinding(tmp.GetOutputTrack(2), player.GetComponentInChildren<Camera>().gameObject);
        }

        if (comeIn.GetComponent<QuickOutline>())
            leftOutline = comeIn.GetComponent<QuickOutline>();
        if (comeOut.GetComponent<QuickOutline>())
            rightOutline = comeOut.GetComponent<QuickOutline>();

        forwardTrigger.SetActive(false);
    }

    private void OnEnable()
    {
        if (!capsuleColl)   // 트리거 콜라이더 세팅.
        {
            capsuleColl = GetComponent<CapsuleCollider>();
            capsuleColl.isTrigger = true;
        }

        if (!player)    // 플레이어 GO 세팅.
        {
            if (GameObject.FindWithTag("Player"))
                player = GameObject.FindWithTag("Player");
        }


        if (comeIn)   // 타임라인.
        {
            comeIn.played += ComeIn_played;     // 타임라인이 실행될 때 발생할 커스텀 이벤트 추가.
            comeIn.stopped += ComeIn_stopped;   // 타임라인이 멈췄을 때(강제 정지 포함) 발생할 커스텀 이벤트(Action) 추가.
        }
        if (comeOut)
        {
            comeOut.played += ComeOut_played;
            comeOut.stopped += ComeOut_stopped;
        }
    }

    private void ComeIn_played(PlayableDirector obj)
    {
        StartCoroutine(Timer(comeIn));
    }

    private void ComeIn_stopped(PlayableDirector obj)
    {
        forwardTrigger.SetActive(true);
        backTrigger.GetComponent<BackTrigger>().setPosition();
        Destroy(goHouseTrigger);

        // 타임라인 실행 전 잠궜던 플레이어의 이동 및 회전을 다시 해제.
        FirstPersonCamera.player.ChangeMoveRotaState(true);
        // 특정 조건을 만족하지 않으면 상호작용 할 수 없도록 트리거 콜라이더 비활성화.
        GetComponent<CapsuleCollider>().enabled = false;
        // 나갈 수 없도록 물리 콜라이더 활성화.
        GetComponent<BoxCollider>().enabled = true;
        isIn = true;

        // 디렉터에 추가해줬던 이벤트 제거.
        comeIn.played -= ComeIn_played;
        comeIn.stopped -= ComeIn_stopped;
        comeIn = null;
    }

    private void ComeOut_played(PlayableDirector obj)
    {
        StartCoroutine(Timer(comeOut));
    }

    private void ComeOut_stopped(PlayableDirector obj)
    {
        if (timeline != null)
        {
            // 보스닥터 타임라인용 스크립트로 설정.
            scripts.SetScript(2);
            scripts = null;

            // 보스닥터 타임라인에 이벤트 추가 및 재생.
            timeline.played += BossAndDoctor_play;
            timeline.stopped += BossAndDoctor;
            timeline.Play();
        }

        // 디렉터에 추가해줬던 이벤트 제거.
        comeOut.played -= ComeOut_played;
        comeOut.stopped -= ComeOut_stopped;
        comeOut = null;
    }

    private void BossAndDoctor_play(PlayableDirector obj)
    {
        StartCoroutine(Timer(timeline));
    }

    private void BossAndDoctor(PlayableDirector obj)
    {
        if (cabinet)
        {   // 창문으로 진행하였으니 캐비닛 수색 연출을 위해 트리거 콜라이더 활성화 및 태그 변경.
            cabinet.tag = "Outline";
            cabinet.GetComponent<SphereCollider>().enabled = true;
        }
        timeline.transform.Find("cctvdoor").tag = "Untagged";

        // DoctorBook 오브젝트가 Chapter2 씬의 RootObjejct 중 하나이므로 scene.GetRootGameObjects()로 찾아 활성화 시킨다.
        GameObject[] tmp = gameObject.scene.GetRootGameObjects();
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i].name == "DoctorBook")
            {
                tmp[i].SetActive(true);
                bookTrigger.SetActive(true);
                break;
            }
        }

        FirstPersonCamera.player.ChangeMoveRotaState(true);

        // 디렉터에 추가해줬던 이벤트 제거.
        timeline.played -= BossAndDoctor_play;
        timeline.stopped -= BossAndDoctor;
        timeline = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 집 안에서 나가기 위해 창문과 상호작용할 때 아웃라인 처리.
            // 이렇게 하는 경우는 this.gameObject가 메쉬가 없는 스크립트 객체이고
            // 창문의 콜라이더는 이미 제거되었기 때문.
            if (isIn)
            {
                if (FirstPersonCamera.player.getRaycastHit().transform != null)
                {
                    if (FirstPersonCamera.player.getRaycastHit().transform == transform)
                    {
                        leftOutline.enabled = true;
                        rightOutline.enabled = true;
                    }
                    else
                    {
                        leftOutline.enabled = false;
                        rightOutline.enabled = false;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (isIn)   // 집 안에 있을 때.
                    {
                        if (comeOut != null)
                        {
                            // 더이상 상호작용이 되지 않도록 콜라이더 모두 제거.
                            Destroy(GetComponent<CapsuleCollider>());
                            Destroy(GetComponent<BoxCollider>());

                            if (leftOutline.enabled || rightOutline.enabled)
                            {
                                leftOutline.enabled = false;
                                rightOutline.enabled = false;
                            }

                            // 플레이어의 이동 및 회전을 잠금.
                            FirstPersonCamera.player.ChangeMoveRotaState(false);
                            comeOut.Play();     // 타임라인 실행.
                        }
                    }
                    else        // 집 밖에 있을 때.
                    {
                        if (comeIn != null)
                        {
                            // 창문의 콜라이더 제거.
                            Destroy(comeIn.GetComponent<Collider>());
                            Destroy(comeOut.GetComponent<Collider>());
                            // 플레이어의 이동 및 회전을 잠금.
                            FirstPersonCamera.player.ChangeMoveRotaState(false);
                            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().SetScript("오케이. 여긴 열려있어.");
                            comeIn.Play();      // 타임라인 실행.
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 트리거 콜라이더에서 벗어날 경우 창문의 아웃라인 비활성화.
            if (leftOutline.enabled || rightOutline.enabled)
            {
                leftOutline.enabled = false;
                rightOutline.enabled = false;
            }
        }
    }


    private IEnumerator Timer(PlayableDirector director)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float t = 0;
        float d = (float)director.playableAsset.duration;
        while (t < d)
        {
            t += Time.deltaTime;
            yield return wait;
        }
        director.Stop();
    }
}
