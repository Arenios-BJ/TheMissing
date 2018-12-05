/* FirstPersonCamera.cs / Last modified date : 2018-08-08 / Last modifier : Yang Jae Hyeok
 * 
 *** Script Description
 * 플레이어 캐릭터 1인칭 시점 컨트롤러. MainCamera에 Add하고 MainCamera는 캐릭터 모델링의 자식으로 두자.
 * 상하좌우(현재 좌우는 주석처리)의 회전 시야각을 조절할 수 있다. = Mathf.Clamp()
 * 
 *** 참고 사항
 * 스크립트가 붙어있는 카메라나 부모에 RigidBody가 붙어있어야 한다.
 * 마우스 커서 컨트롤은 MouseLock 스크립트를 받아와서 처리한다.
 * 
 * 완....성?
*/

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]       // 필요 컴포넌트(리지드바디).
public class FirstPersonCamera : MonoBehaviour
{
    public static FirstPersonCamera player { private set; get; }

    [Header("Sensitivity Setting")]
    [SerializeField] private float sensitivityX = 5.0f; // 마우스 가로축 감도.
    [SerializeField] private float sensitivityY = 5.0f; // 마우스 세로축 감도.
    [SerializeField] private float minimumY = -45.0f;   // (X축 기준)아래쪽 방향 회전 최소값. 
    [SerializeField] private float maximumY = 45.0f;    // (X축 기준)위쪽 방향 회전 최소값.

    [Header("Moving Setting")]
    [SerializeField] private bool isWalking;            // 걷기, 달리기 체크.
    [SerializeField] private float walkSpeed = 4.0f;    // 걷기 속도. Input 값에 곱해짐.
    [SerializeField] private float runSpeed = 8.0f;     // 달리기 속도.

    [Header("Jump Setting")]
    [SerializeField] private bool isJumping = false;
    [SerializeField] private float jumpDuration = 0.3f; // 점프 시 올라가는 시간.
    [SerializeField] private float jumpSpeed = 0.2f;    // 점프 높이.
    private bool isJump;                                // 점프 체크.

    [Header("Crouch Setting")]
    [SerializeField] private bool isCrouching = false;  // 웅크리기(?)
    [SerializeField] private float crouchAmount = 0.3f;
    [SerializeField] private float crouchSpeed = 2.0f;  // 웅크렸을 때 이동 속도.

    [Header("Flashlight Setting")]
    [SerializeField] private GameObject flashLight;     // 손전등 Light 컴포넌트.
    [SerializeField] public Image batteryImage;         // 손전등 배터리 Image 컴포넌트.
    public bool m_bHasFlashlight = false;               // 손전등의 습득 여부 판단.
    public float m_fCur_Battery = 0f;                   // 현재 배터리 양.
    private float m_fBatterTimer = 0f;                  // 배터리 감소 루틴에 사용되는 타이머.

    [Header(" ")]
    private GameObject cam;             // 플레이어 객체의 카메라. Player 오브젝트의 자식으로 배치.
    private bool m_bMoveRotaState;      // 이동, 회전을 할 수 있는가. 없는가.
    private Rigidbody playerRB;         // 캐릭터의 이동을 책임질 RigidBody.
    private float movementX = 0.0f;     // 이동값...
    private float movementY = 0.0f;

    private float rotationX = 0.0f;     // 회전값...
    private float rotationY = 0.0f;
    private Quaternion quaternionX;     // 회전값을 반영한 쿼터니언...
    private Quaternion quaternionY;
    private Quaternion camOriginRotation;  // 기존의 회전값.
    private Quaternion originRotation;

    private bool m_bIsMouseLock;        // 락 여부 판단.
    private RaycastHit hit;
    private GameObject hitGameObject;
    private Ray ray;                    // Raycast에 사용 될 Ray.
    [SerializeField] private float rayDistance = 5f; // 쏘아보낼 ray의 길이.

    public Animator lootmotion;
    public bool AniCheck = false;
    public bool LightCheck;

    public AudioSource walk;
    public AudioClip[] MoveSound;

    public bool In; // 플레이어가 내부에 있는지 확인

    private void Awake()
    {
        player = this;
    }

    private void Start()
    {
        In = true;

        isWalking = true;
        isJump = false;

        cam = GameObject.FindWithTag("MainCamera");
        m_bMoveRotaState = false;

        camOriginRotation = cam.transform.localRotation;    // 카메라의 기존 회전값을 저장. 이후 회전값을 계산할 때 기존값 추가.
        originRotation = transform.rotation;                // 플레이어 객체의 기존 회전값.

        m_bIsMouseLock = true;  // 마우스 잠금 초기값. = 잠금

        // 리지드바디 세팅.
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerRB.angularDrag = Mathf.Infinity;
        playerRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (flashLight == null) // 손전등 Light 컴포넌트 세팅.
            if (GameObject.Find("Flashlight"))
                flashLight = GameObject.Find("Flashlight");
        if (flashLight != null)
            flashLight.SetActive(false); // 초기 컴포넌트 Off.

        if (batteryImage == null)   // 손전등 배터리 UI 세팅.
            if (GameObject.Find("Battery_On"))
                batteryImage = GameObject.Find("Battery_On").GetComponent<Image>();
        if (batteryImage != null)
        {
            batteryImage.transform.parent.gameObject.SetActive(false);  // 손전등을 먹기 전에는 배터리 UI 비활성화.
            batteryImage.fillAmount = 0f;

            // Start()문에서 해당 bool 변수가 true라면 세이브 데이터를 로드한 경우이기에,
            // 배터리 이미지를 활성화 및 배터리 잔량을 세이브 데이터와 동기화 한다.
            if (m_bHasFlashlight)
            {
                batteryImage.transform.parent.gameObject.SetActive(true);
                float remine = PlayerPrefs.GetInt("Battery") * 0.01f;
                batteryImage.fillAmount = 1f;
                m_fCur_Battery = 1f;
                Debug.LogFormat("GetInt : {0} / remine = {1}", PlayerPrefs.GetInt("Battery"), remine);
            }
        }

        ray = new Ray();    // Raycast에 사용 될 Ray 초기화.

        lootmotion = cam.GetComponent<Animator>();
        lootmotion.applyRootMotion = false;

        LightCheck = false;

        walk.Stop();
    }

    private void FixedUpdate()
    {
        if (m_bMoveRotaState)           // 캐릭터 이동 가능 상태에 따라.
        {
            if (GameManager.gm.panelOpen)
                return;

            if (m_bIsMouseLock)         // 마우스 커서 락 여부에 따라.
                CharacterRotation();    // 회전 처리.

            if (Input.GetKeyDown(KeyCode.Space))    // 점프 키 입력.
                if (!isJumping && !isJump)
                    if (playerRB.velocity.y > -1f)
                        isJump = true;

            if (!isCrouching)           // 웅크리고 있을 때.
            {
                var col = GetComponent<CapsuleCollider>();
                if (col != null)
                {
                    col.height = 1.5f;  // 콜라이더의 크기 조절.
                    if (cam != null)
                    {
                        Vector3 tmp = cam.transform.localPosition;
                        tmp.y = 0.8f;
                        cam.transform.localPosition = tmp;  // 카메라의 위치 조절.
                    }
                }
            }
            else                        // 일반 상태. (웅크리기 X)
            {
                var col = GetComponent<CapsuleCollider>();
                if (col != null)
                {
                    col.height = 1f;
                    if (cam != null)
                    {
                        Vector3 tmp = cam.transform.localPosition;
                        tmp.y = crouchAmount;
                        cam.transform.localPosition = tmp;
                    }
                }
            }

            isCrouching = Input.GetKey(KeyCode.LeftControl);// 왼쪽 ctrl키가 눌러져 있는(holds down) 동안.
            isWalking = !Input.GetKey(KeyCode.LeftShift);   // 왼쪽 shift키가 눌러져 있는(holds down) 동안.

            CharacterMovement();        // 이동 처리.
        }
    }

    private void Update()
    {
        if (lootmotion)
        {
            if (AniCheck == false)
            {
                AnimatorStateInfo stateinfo = lootmotion.GetCurrentAnimatorStateInfo(0);
                if (stateinfo.normalizedTime > 1.0f)
                {
                    Debug.Log("애니메이션끝남");
                    AniCheck = true;
                    lootmotion.applyRootMotion = true;
                    m_bMoveRotaState = true;
                }
            }
            else
            {
                lootmotion.applyRootMotion = true;
                lootmotion.runtimeAnimatorController = null;
                lootmotion = null;
                m_bMoveRotaState = true;
            }
        }

        // UI화면 클릭을 위해 예외 처리.
        if (EventSystem.current != null)
            if (EventSystem.current.IsPointerOverGameObject())  // 마우스 포인터가 UI 위로 Over되면 True.
            {
                // UI가 켜진 후 3D 오브젝트의 클릭을 차단하기 위해 RaycastHit(구조체)를 초기화.
                if (hit.collider != null)
                    hit = new RaycastHit();

                return;
            }

        if (Time.timeScale != 0f)
        {
            int layerMask = 1 << 9; // 비트를 왼쪽으로 9번 밀면 9번 레이어(Player)만 남는다.
            layerMask = ~layerMask; // 해당 변수의 비트를 뒤집는다. 9번 레이어(Player)만 제외한 나머지가 들어간다.
            ray.origin = cam.transform.position;
            ray.direction = cam.transform.forward;

            if (Physics.Raycast(ray, out hit, rayDistance, layerMask))     // 레이캐스트.
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("Item")
                    || hit.transform.CompareTag("Handle")
                    || hit.transform.CompareTag("Totem")
                    || hit.transform.CompareTag("Outline"))
                    QuickOutline(hit);
                else
                {
                    if (hitGameObject != null)
                    {
                        if (hitGameObject.GetComponent<QuickOutline>())
                            hitGameObject.GetComponent<QuickOutline>().enabled = false;
                        hitGameObject = null;
                    }
                }
            }
            else
            {
                if (hitGameObject != null)
                {
                    if (hitGameObject.GetComponent<QuickOutline>())
                        hitGameObject.GetComponent<QuickOutline>().enabled = false;
                    hitGameObject = null;
                }
            }
        }

        // Scene 창에서의 레이의 궤적.
        Debug.DrawRay(cam.transform.position, cam.transform.forward * rayDistance, Color.red, 0.5f);

        if (!isJumping) // 현재 점프중인지 판단.
            if (isJump)
                StartCoroutine(CharacterJump());    // 점프 코루틴 실행.

        // 마우스 커서의 상태에 따라 상태 여부를 체크하는 bool 변수값 변경.
        if (Cursor.lockState == CursorLockMode.Locked && !Cursor.visible)
            m_bIsMouseLock = true;
        else
            m_bIsMouseLock = false;

        if (m_bHasFlashlight)   // 손전등을 습득했을 경우에만.
            FlashLight();       // 손전등 관련 작업 수행.
    } // Update(). END

    // 손전등 관련 작업 수행 함수.
    private void FlashLight()
    {
        // 현재 배터리가 없으면
        if (m_fCur_Battery == 0f)
        {
            // 배터리의 fillAmount도 0
            batteryImage.fillAmount = 0;
        }

        if (LightCheck == false)
        {
            if (Input.GetKeyDown(KeyCode.F))    // 손전등 On / Off.
            {
                if (GameManager.gm.panelOpen == false)
                {
                    SoundManager.instance.Flash_Button();

                    if (m_fCur_Battery > 0.0f)
                    {
                        flashLight.SetActive(true);
                        LightCheck = true;

                        Debug.Log("손전등 켜짐");
                    }
                }
            }
        }
        else if (LightCheck == true)
        {
            if (Input.GetKeyDown(KeyCode.F))    // 손전등 On / Off.
            {
                if (GameManager.gm.panelOpen == false)
                {
                    SoundManager.instance.Flash_Button();

                    if (m_fCur_Battery > 0.0f)
                    {
                        flashLight.SetActive(false);
                        LightCheck = false;

                        Debug.Log("손전등 꺼짐");
                    }
                }
            }
        }

        if (flashLight.activeSelf == true)
        {
            m_fBatterTimer += Time.deltaTime;   // 배터리 감소 타이머 증가.
        }

        if (m_fBatterTimer >= 3.0f && flashLight.activeSelf == true)
        {
            Debug.Log("배터리 감소");
            m_fCur_Battery -= 0.03f;
            m_fBatterTimer = 0f;

            batteryImage.fillAmount = m_fCur_Battery;

            if (m_fCur_Battery <= 0f)   // 배터리의 양이 0 이하로 떨어지면 손전등 Off.
                flashLight.SetActive(false);
        }
    }

    // 캐릭터(및 카메라) 회전 함수.
    private void CharacterRotation()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;   // 마우스 가로축(X) 변화량 * 감도
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;   // 마우스 세로축(Y) 변화량 * 감도

        if (rotationX != 0 || rotationY != 0)
        {
            // Mathf.Clamp(1, 2, 3) : 1의 값을 2와 3의 사이값으로 고정하여 반환.
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);     // X축 범위 제한. (Y축은 무한 회전이기에 제한하지 않음)

            quaternionX = Quaternion.AngleAxis(rotationX, Vector3.up);   // Y축(up) 기준으로 회전. 에디터상 RotationY 값 변화.
            quaternionY = Quaternion.AngleAxis(rotationY, Vector3.left); // X축(left) 기준으로 회전. 에디터상 RotationX 값 변화.

            cam.transform.localRotation = camOriginRotation * quaternionY;   // Transform에 회전 적용.
            transform.rotation = originRotation * quaternionX;
        }
    }

    // 캐릭터 이동 함수.
    private void CharacterMovement()
    {
        movementX = Input.GetAxis("Vertical");      // 전진, 후진 Input 값.
        movementY = Input.GetAxis("Horizontal");    // 좌, 우 이동 Input 값.

        if (movementX != 0 || movementY != 0)
        {
            float speed = isWalking ? walkSpeed : runSpeed; // 이동 상태에 따라 속도값 변화.
            if (isCrouching)    // 웅크렸을 때 속도값 조절.
                speed = crouchSpeed;

            Vector3 tmp = Vector3.zero;
            tmp = (cam.transform.forward * movementX) + (cam.transform.right * movementY);  // 카메라가 보는 방향에 Input값 적용 후 이동 벡터에 대입.
            tmp = tmp * speed * Time.deltaTime;
            tmp.y = 0f; // y값 고정~
            playerRB.MovePosition(tmp + playerRB.position); // 이동~


            if (In == true && isWalking == false)
            {
                GetComponent<AudioSource>().clip = MoveSound[1];
                if (!walk.isPlaying)
                {
                    walk.Play();
                }
                else if (walk.isPlaying)
                {
                    return;
                }

            }

            if(In == true && isWalking == true)
            {
                GetComponent<AudioSource>().clip = MoveSound[0];
                if (!walk.isPlaying)
                {
                    walk.Play();
                }
                else if (walk.isPlaying)
                {
                    return;
                }
            }

            if (In == false && isWalking == false)
            {
                GetComponent<AudioSource>().clip = MoveSound[3];
                if (!walk.isPlaying)
                {
                    walk.Play();
                }
                else if (walk.isPlaying)
                {
                    return;
                }

            }

            if (In == false && isWalking == true)
            {
                GetComponent<AudioSource>().clip = MoveSound[2];
                if (!walk.isPlaying)
                {
                    walk.Play();
                }
                else if (walk.isPlaying)
                {
                    return;
                }
            }
        }
    }

    // 점프 코루틴 함수.
    private IEnumerator CharacterJump()
    {
        isJumping = true;
        float t = 0f;
        WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();

        while (t < jumpDuration)    // 점프 동작 길이.
        {
            float y = Mathf.Lerp(jumpSpeed, 0f, t / jumpDuration);
            t += Time.fixedDeltaTime;
            // RigidBody의 y축 이동을 막아놨기에 Transform.Translate()를 사용.
            transform.Translate(Vector3.up * y, Space.World);

            yield return waitFixed;
        }

        // 점프(뛰어 오르는) 동작이 끝났으므로.
        isJump = false;
        isJumping = false;
    }

    private void QuickOutline(RaycastHit hit)
    {
        // 이전 프레임에서 충돌한 물체가 없다면.
        if (hitGameObject == null)
        {
            // 현재 프레임에서 충돌한 물체를 hitGameObject에 대입.
            hitGameObject = hit.transform.gameObject;
            // QuickOutline 컴포넌트를 가지고 있다면 활성화.
            if (hitGameObject.GetComponent<QuickOutline>())
                hitGameObject.GetComponent<QuickOutline>().enabled = true;
        }
        // 이전 프레임에서 충돌한 물체가 있다면.
        else
        {
            // 이전 충돌한 물체가 현재 충돌한 물체와 다르다면.
            if (hitGameObject != hit.transform.gameObject)
            {
                // 이전 충돌한 물체가 QuickOutline을 가지고 있으면 비활성화.
                if (hitGameObject.GetComponent<QuickOutline>())
                    hitGameObject.GetComponent<QuickOutline>().enabled = false;
                // hitGameObject에 현재 충돌한 물체를 대입.
                hitGameObject = hit.transform.gameObject;
                // 현재 충돌한 물체가 QuickOutline을 가지고 있으면 활성화.
                if (hitGameObject.GetComponent<QuickOutline>())
                    hitGameObject.GetComponent<QuickOutline>().enabled = true;
            }
        }
    }

    /// <summary>
    /// 플레이어의 이동 및 회전 조작을 강제로 설정.
    /// </summary>
    /// <param name="state">true : 가능 / false : 불가능</param>
    public void ChangeMoveRotaState(bool state)
    {
        m_bMoveRotaState = state;
    }

    public RaycastHit getRaycastHit()   // Physics.Raycast() 결과 반환.
    {
        return hit;
    }
}