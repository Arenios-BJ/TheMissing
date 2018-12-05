using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TreeClimberGame : MonoBehaviour {
    [SerializeField] private GameObject panel;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private RectTransform handle;
    [SerializeField] private GameObject touchRange;
    [SerializeField] private float m_fHandleSpeed = 0.01f;
    [SerializeField] private float m_fIncreaseSpeedValue = 0.001f;

    private IEnumerator settingTouchRange;
    private RectTransform touch;
    private Transform player;
    private Vector3 originPos;
    private Quaternion originRot;
    private bool m_bIsGameStart;
    private bool m_bIsGameClear;
    private bool m_bIsHandleIncrease;
    private bool m_bCorrectlyTiming;
    private int m_iGameClearCount = 5;
    private GameObject climberTreesCCTV;

    public TimelineAsset timeline;
    public AnimationPlayableAsset timeclip;

    private void OnEnable()
    {
        originPos = Vector3.zero;           // 이건 참조 변수가 아니라능. 빈 벡터를 넣어줬다능.
        originRot = Quaternion.identity;    // 이것도 참조가 아니라능. 월드 좌표계 초기 사원수값을 넣어줬다능.
        m_bIsGameStart = false;
        m_bIsGameClear = false;
        m_bIsHandleIncrease = true;
        m_bCorrectlyTiming = false;

        if (panel == null)  // 유니티 에디터의 Inspector창에서 할당을 안 해줬을 경우, 직접 할당.
            panel = GameObject.Find("Pnl_ClimberGame");
        if (scrollbar == null)
            scrollbar = panel.GetComponentInChildren<Scrollbar>();
        if (handle == null)
            handle = panel.transform.Find("Scrollbar/Sliding Area/Handle").GetComponent<RectTransform>();
        if (touchRange == null)
            touchRange = Resources.Load("Prefab/TouchRange") as GameObject; // 에셋 폴더 내의 'Resources'폴더 내의 저 경로의 파일을 로드.

        panel.SetActive(false); // 초기에 패널은 보이지 않는다.
    }

    // 정답 범위(RawImage)의 스크롤바 상의 위치를 갱신하는 코루틴 함수.
    private IEnumerator SetTouchRange()
    {
        WaitUntil until = new WaitUntil(() => m_bCorrectlyTiming == true);  // 람다식. () : bool 형태의 값이 들어와야하고, => 우측 식으로 분별한다.
        touch = Instantiate(touchRange, scrollbar.transform).transform as RectTransform;    // RectTransform은 Transform을 상속한다.
        float x;    // 정답의 x좌표 변수(실질적으로는 x앵커의 값이지만 anchoredPosition의 값을 0으로 잡음으로써 위치값으로 사용된다).
        int count = 0;  // 게임 정답 횟수.
        
        for (int i = 0; i < m_iGameClearCount; i++)     // 게임 클리어를 위한 정답 횟수만큼 반복.
        {
            x = Random.Range(0f, 1f);                   // 앵커값의 범위인 0 ~ 1.
            touch.transform.position = new Vector3(0f, scrollbar.transform.position.y, 0f); // 정답 범위의 오브젝트 위치를 스크롤바의 y값과 일치.(월드 좌표계)
            Vector2 anchor = new Vector2(x, 0.5f);      // 앵커의 y값은 y축 대응이라 0.5로 고정.
            touch.anchorMax = touch.anchorMin = anchor; // 랜덤값의 x를 기반으로한 앵커(anchor 변수) 대입.
            touch.anchoredPosition = Vector2.zero;      // 앵커포지션은 zero로 해야 앵커에 따라 위치가 변화한다.

            m_bCorrectlyTiming = false;                 // yield를 걸기 위함.

            yield return until;

            if (m_bIsGameClear) // 중간에 게임이 종료되어 코루틴을 중지해야 할 때를 위해 추가 검사.
            {
                m_fHandleSpeed -= count * m_fIncreaseSpeedValue;    // 중간에 게임이 종료되었을 시 증가했던 핸들의 이동속도를 원복.
                break;
            }

            player.Translate(Vector3.up * 2.5f);        // 플레이어의 위치 증가.(나무 타는거...)
            m_fHandleSpeed += m_fIncreaseSpeedValue;    // 스크롤 핸들의 이동속도 증가.
            count++;                                    // 맞춘 정답 횟수 증가.
            if (count == m_iGameClearCount)             // 횟수가 게임 종료 조건을 만족했을 경우.
            {
                SetupAfterMiniGame(player.gameObject);
                //StartCoroutine(ClearTimeline());
            }

            if (m_bIsGameClear) // '게임 종료'를 판단하는 bool 변수를 통해 반복문 이탈.
                break;
        }
        Destroy(touch.gameObject);  // 정답 범위 객체 제거.
    }

    // 스크롤 핸들 자동 이동 코루틴 함수.
    private IEnumerator HandleSlide()
    {
        // 새로운 게임을 위한 기본 세팅.
        scrollbar.value = 0.5f;     // 핸들 위치 가운데 고정.
        m_bIsHandleIncrease = true; // 초기값 : 증가 == 핸들 우측으로 이동.
        panel.SetActive(true);      // GUI 활성화.

        WaitForSeconds wait = new WaitForSeconds(1f);
        yield return wait;
        settingTouchRange = SetTouchRange();
        StartCoroutine(settingTouchRange);    // 정답 범위 생성 코루틴 시작.
        yield return wait;

        while (m_bIsGameStart)
        {
            if (m_bIsHandleIncrease)    // bool 값에 따라 핸들의 가감 연산.(가감에 다른 좌, 우 이동)
                scrollbar.value += m_fHandleSpeed;
            else
                scrollbar.value -= m_fHandleSpeed;

            if (scrollbar.value == 0f || scrollbar.value == 1f) // scrollbar.value의 값의 범위는 0 ~ 1. 미만, 초과시 범위 내의 값으로 고정된다.
                m_bIsHandleIncrease = !m_bIsHandleIncrease;

            if (m_bIsGameClear) // '게임 종료'를 판단하는 bool 변수를 통해 반복문 종료.
                m_bIsGameStart = false;

            // '스페이스' 키를 입력했을 시의 정답(타이밍) 체크.
            if (m_bIsGameStart && Input.GetKeyDown(KeyCode.Space))
            {
                if (!m_bCorrectlyTiming)
                    // 핸들의 위치(핸들 크기의 중점)가 정답 범위의 사이즈(sizeDelta) 안에 위치했을 경우 == 정답.
                    if (touch.localPosition.x - (touch.sizeDelta.x * 0.5f) < handle.localPosition.x
                        && handle.localPosition.x < touch.localPosition.x + (touch.sizeDelta.x * 0.5f))
                    {
                        m_bCorrectlyTiming = true;
                    }
            }

            yield return null;  // 핸들 이동 및 키 입력은 프레임마다 계산 및 검사.
        }

        settingTouchRange.MoveNext();
    }
    
    private IEnumerator ClearTimeline()
    {
        PlayableDirector director = climberTreesCCTV.GetComponent<PlayableDirector>();
        director.Play();
        player.GetComponentInChildren<FirstPersonCamera>().ChangeMoveRotaState(false);
        climberTreesCCTV.transform.parent.Find("Tree").GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitUntil(() => director.state == PlayState.Paused);
        player.GetComponentInChildren<FirstPersonCamera>().ChangeMoveRotaState(true);
    }

    /************************/
    /*** P U B L I C 함수 ***/
    /************************/
    public void SetupBeforeMiniGame(GameObject other, GameObject cctv)
    {
        player = other.transform;               // 플레이어의 위치. 참조 변수라서 이거 바꾸면 플레이어 위치 바뀜.
        climberTreesCCTV = cctv;
        originPos = other.transform.position;   // 미니게임 시작 전 위치와 회전 저장. 이건 값임. 밑에 것도 마찬가지.
        originRot = other.transform.rotation;
        other.GetComponentInChildren<FirstPersonCamera>().ChangeMoveRotaState(false);    // 플레이어의 이동 및 카메라 무빙 중지.
        GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(false);   // 마우스 강제 풀림.
        other.transform.Find("MainCamera").transform.localRotation = Quaternion.AngleAxis(60f, Vector3.left);   // 카메라 상향 고정.

        m_bIsGameStart = true;
        m_bIsGameClear = false;
        StartCoroutine("HandleSlide");  // 자, 게임을 시작하지.
    }

    // 이 함수가 불리는 경우가 두 가지 있다.
    // 첫번째, ESC키로 게임 종료 시 ClimberTrees.cs->OnTriggerStay()에서 호출.
    // 두번째, 게임 클리어로 게임 종료 시 본 스크립트의 SetTouchRange()에서 호출.
    public void SetupAfterMiniGame(GameObject other)
    {
        panel.SetActive(false); // GUI 비활성화.

        other.transform.position = originPos; // 게임 시작 전 기존의 정보(Vector3, Quaternion)로 리셋.
        other.transform.rotation = originRot;
        other.transform.Find("MainCamera").transform.localRotation = Quaternion.Euler(2.5f, 0f, 0f);    // 카메라의 회전값 리셋.
        GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(true);   // 마우스 강제 잠금.
        other.GetComponentInChildren<FirstPersonCamera>().ChangeMoveRotaState(true);    // 플레이어의 이동 및 카메라 무빙 활성화.

        m_bIsGameStart = false;
        m_bIsGameClear = true;

        if (m_iGameClearCount == 5) // 게임을 클리어해서 끝났을 때.
        {
            climberTreesCCTV.transform.parent.Find("Tree").tag = "Untagged";    // 태그 변경으로 'Outline' 처리 안 함.
            var cm = climberTreesCCTV.transform.GetChild(0).GetComponent<CameraMovement>();
            if (cm != null)
                cm.enabled = false; 
            var go = climberTreesCCTV.transform.GetChild(0).GetChild(0).gameObject;
            if (go != null)
                go.SetActive(false);
        }
    }

    public bool IsGaming()
    {
        return m_bIsGameStart;
    }
}