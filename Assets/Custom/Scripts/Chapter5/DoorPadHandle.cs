using UnityEngine;
using UnityEngine.Playables;

public class DoorPadHandle : MonoBehaviour {
    public Transform setPos;        // 상호작용 시 플레이어 카메라 고정 위치.
    public Transform padParent;     // 비밀 패드들의 Parent.

    private Vector3 originPos;      // 상호작용 전의 카메라 위치를 담을 벡터.
    private Quaternion originRot;   // 상호작용 전의 카메라 회전을 담을 쿼터니언.
    private Rigidbody rigid;        // 핸들의 리지드바디.

    private FirstPersonCamera player;   // RayCast 및 플레이어 이동 조작을 컨트롤 하기 위함.
    private Camera cam;                 // 플레이어의 카메라.
    private bool isGaming = false;      // 상호작용 중인지?
    private bool isClear = false;       // 이 핸들에 붙은 작업을 다 하였는지?

    private Vector2 beforePoint;
    private Vector2 midPoint;           // 2D 화면의 중점.
    private float timer = 0f;

    private PlayableDirector zoomIn;
    private double timelineDuration;
    private bool timelinePlaying = false;
    [SerializeField] private ForTimelineScript scripts;
    [SerializeField] private GameObject TimelineCamera;

    [SerializeField] private float clearedVelocity = 4f;
    [SerializeField] private float accelerateValue = 1f;

    public AudioSource Handle;

    private void Start()
    {
        if (!player)
            player = FirstPersonCamera.player;
        zoomIn = GetComponent<PlayableDirector>();

        if (!padParent || !player || !zoomIn)    // 에러 방지. 뭐라도 하나라도 없으면...
        {
            tag = "Untagged";
            Destroy(GetComponent<Collider>());
            Destroy(GetComponent<QuickOutline>());
            Destroy(this, 1f);
            return;
        }

        // 타임라인에 MainCamera 직접 바인딩. Why? > 에디터에서 드래그앤드랍 해도 다른 Scene이라서 로딩시에 missing.
        UnityEngine.Timeline.TimelineAsset timeline = zoomIn.playableAsset as UnityEngine.Timeline.TimelineAsset;
        zoomIn.SetGenericBinding(timeline.GetOutputTrack(3), Camera.main.gameObject);
        timelineDuration = timeline.duration;

        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        rigid = GetComponent<Rigidbody>();
        beforePoint = Vector3.zero;
        midPoint = cam.pixelRect.max / 2f;  // 화면의 중앙 픽셀 위치.

        Handle.enabled = false;
    }

    private void Update()
    {
        // 클리어 조건을 충족 후 타임라인이 실행되는 동안.
        if (timelinePlaying)
        {
            timer += Time.deltaTime;

            if (timer > timelineDuration)
                TimelineStopped();

            return;
        }

        if (isGaming == false)  // 상호작용 중이 아닐 때.
        {
            if (player)
            {
                if (player.getRaycastHit().collider != null
                    && player.getRaycastHit().collider.gameObject == this.gameObject)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (GameManager.gm.panelOpen == false)
                        {
                            Transform tmp = this.cam.transform;
                            originPos = tmp.localPosition;
                            originRot = tmp.localRotation;

                            player.ChangeMoveRotaState(false);
                            tmp.position = setPos.position;
                            tmp.rotation = setPos.rotation;
                            GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(false);

                            tag = "Untagged";
                            GetComponent<Collider>().enabled = false;
                            rigid.angularDrag = 0.1f;
                            isGaming = true;
                        }
                    }
                }
            }
        }
        else    // isGaming == true. 상호작용 중일 때.
        {
            if (isClear)
            {
                rigid.angularDrag = 1f;
                Destroy(GetComponent<Collider>());
                Destroy(GetComponent<QuickOutline>());
                GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(true);
                isGaming = false;

                scripts.SetScript(0);
                timer = 0f;
                timelinePlaying = true;
                zoomIn.Play();

                Handle.enabled = false;
                SoundManager.instance.Ch5RockS();

                return;
            }

            if (Input.GetKeyDown(KeyCode.Tab))  // 상호작용 종료 키를 'Tab'으로.
            {
                if (GameManager.gm.panelOpen == false)
                {
                    // 클리어와 상관없이 상호작용 종료 후 세팅.
                    GetComponent<Collider>().enabled = true; // 상호작용 오브젝트의 콜라이더를 다시 활성화.
                    tag = "Handle"; // 태그 설정.
                    isGaming = false;
                    rigid.angularDrag = 1f; // 회전이 빨리 멈추도록 상호작용 오브젝트의 회전 저항값을 크게 준다.

                    // 플레이어의 위치 원복 및 이동 가능.
                    Transform tmp = cam.transform;
                    tmp.localPosition = originPos;
                    tmp.localRotation = originRot;
                    GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(true);
                    player.ChangeMoveRotaState(true);

                    Handle.enabled = false;

                    return;
                }
            }

            if (!isClear)
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        timer = 0f;

                        // 마우스의 버튼을 누른 순간의 위치 정보를 저장.
                        beforePoint = Input.mousePosition;
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        // beforePoint에 이전 프레임의 마우스 위치 정보가 입력되지 않았을 때.
                        if (beforePoint == Vector2.zero)
                        {
                            beforePoint = Input.mousePosition;
                            return;
                        }

                        timer += Time.deltaTime;

                        float value = 0f;
                        if (timer > 0.2f)
                        {
                            Handle.enabled = true;

                            // 상호작용 오브젝트의 회전 속도값이 클리어 조건값보다 커졌을 때.
                            if (rigid.angularVelocity.magnitude > clearedVelocity)
                            {
                                isClear = true;
                                return;
                            }

                            Vector2 nowPoint = Input.mousePosition;
                            nowPoint = (nowPoint - midPoint).normalized;        // 중점에서 현재 마우스 위치로 향하는 노멀벡터.
                            beforePoint = (beforePoint - midPoint).normalized;  // 중점에서 이전 마우스 위치로 향하는 노멀벡터.
                            float dot = Vector2.Dot(nowPoint, beforePoint);     // 두 벡터를 내적.
                            dot = dot / (nowPoint.magnitude * beforePoint.magnitude);   // 내적 값을 두 벡터의 크기곱으로 나누면 cos값이 나온다.

                            // cos값의 범위는 1 ~ -1. 값의 부호에 따른 value(회전에 가하는 힘)를 계산.
                            if (dot >= 0f)
                                value = (((dot - 1f) * -1f) * 90f) * (accelerateValue * 0.1f);
                            else
                                value = (((dot * -1f) * 90f) + 90f) * (accelerateValue * 0.1f);

                            if (Vector3.Cross(beforePoint, nowPoint).z > 0f)
                                value = -value;

                            beforePoint = Vector2.zero; // 이전 마우스 위치값을 초기화.
                            timer = 0f; // 타이머 초기화.
                        }
                        else
                            return;

                        // value값이 존재하면 회전력을 가한다.
                        if (value != 0f)
                            rigid.AddTorque(value, 0f, 0f, ForceMode.Force);
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        // 버튼을 뗀 순간 위치 정보 초기화.
                        beforePoint = Vector2.zero;
                    }
                }
            } // if(!isClear) END.
        }
    } // Update() END.

    // 타임라인이 끝났을 때 실행될 함수.
    private void TimelineStopped()
    {
        timelinePlaying = false;
        TimelineCamera.SetActive(false);
        // PlayableDirector 컴포넌트 삭제.
        Destroy(zoomIn);
        // MainCamera 위치 원복.
        cam.gameObject.SetActive(true);
        Transform tmp = cam.transform;
        tmp.localPosition = originPos;
        tmp.localRotation = originRot;
        // 플레이어 이동 및 회전 가능.
        player.ChangeMoveRotaState(true);
        // 이 스크립트 삭제.
        Destroy(this);
    }
}
