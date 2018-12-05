using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour {

    // 다시하기 버튼
    private GameObject re_button;
    [SerializeField] private GameObject ok_button;
    
    // 총알 프리팹이 생성될 위치
    private GameObject but;

    // 플레이어 오브젝트
    public GameObject player;

    // 실패 텍스트
    private Text text;

    // 성공 텍스트
    private Text text2;

    public Text T_time;
    public float _time;

    Vector2 m_LeftBottom;
    Vector2 m_RightTop;    

    // 총알 프리팹 
    public GameObject Mis;
    private float offset;
    public float speed = 5.0f;
    private bool clear = false;

    void Start () {

        re_button = GameObject.Find("Btn_ReStart");
  
        text = GameObject.Find("Txt_Fail").GetComponent<Text>();
        text2 = GameObject.Find("Txt_Win").GetComponent<Text>();

        but = GameObject.Find("Bullet");

        text.enabled = false;
        text2.enabled = false;
        re_button.SetActive(false);
        ok_button.SetActive(false);
        offset = Mis.GetComponent<RawImage>().texture.height * 0.5f; 

        RectTransform canvas = gameObject.transform.root.GetComponentInChildren<RectTransform>() as RectTransform;
        m_LeftBottom = new Vector2(canvas.rect.xMin, canvas.rect.yMin);
        m_RightTop = new Vector2(canvas.rect.xMax, canvas.rect.yMax);

        // 1.0초 뒤에, 0.1초마다 Posit함수를 호출한다.
        InvokeRepeating("Posit", 1.0f, 0.1f);
    }

    void Update ()
    {
        if (!clear)
        {
            // 플레이어가 없다면, 즉 죽었다면
            if (!player.activeSelf)
            {
                clear = true;
                // 실패 텍스트를 활성화 한다
                text.enabled = true;
                // 다시하기 버튼을 활성화 한다
                re_button.SetActive(true);

                for (int i = 0; i < but.transform.childCount; i++)
                    but.transform.GetChild(i).GetComponent<Rigidbody>().velocity = Vector3.zero;

                GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(false);
                return;
            }

            // 타이머
            _time += Time.deltaTime;
            T_time.text = _time.ToString("F2");

            // 10초 이상이 됐다면
            if (_time >= 3f)
            {
                clear = true;
                // 성공 텍스트를 활성화 한다.
                text2.enabled = true;
                player.GetComponent<SphereCollider>().enabled = false;
                CancelInvoke("Posit");
                //OnClick();
                player.SetActive(false);
                ok_button.SetActive(true);
                GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(false);
            }
        }
    }

    //다시하기 버튼을 누르면 씬을 다시 불러온다.
    public void OnClick()
    {
        SoundManager.instance.Mouse_Click();

        clear = false;
        _time = 0f;

        text.enabled = false;
        player.SetActive(true);
        player.transform.localPosition = Vector3.zero;

        for (int i = 0; i < but.transform.childCount; i++)
            Destroy(but.transform.GetChild(i).gameObject);

        re_button.SetActive(false);
        GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(true);
        Time.timeScale = 1.0f;
    }

    public void Clear()
    {
        SoundManager.instance.Mouse_Click();
        cctvController controller = transform.parent.parent.GetComponentInChildren<cctvController>();
        controller.IsCleared = true;
        transform.parent.gameObject.SetActive(false);
        GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(true);

        // 게임 클리어 시의 스크립트 표시.
        if (GameObject.Find("ScriptsManager"))
        {
            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().SetScript("이거.. CCTV화면인가? 살펴볼까?");
            Invoke("HelpScript", 2.0f);
        }
    }

    // 총알의 생성 위치
    void Posit()
    {
        if (transform.root.gameObject.activeSelf)
        {
            if (!clear)
            {
                if (player.activeSelf)
                {
                    GameObject instance = Instantiate(Mis, but.transform);
                    (instance.transform as RectTransform).localScale = Vector3.one;
                    instance.transform.localRotation = Quaternion.identity;
                    float InputX = Random.Range(m_LeftBottom.x, m_RightTop.x);
                    float InputY = Random.Range(m_LeftBottom.y, m_RightTop.y);

                    int Num = Random.Range(0, 4);
                    switch (Num)
                    {
                        case 0: // 위쪽
                            instance.transform.localPosition = new Vector3(InputX, m_RightTop.y + offset, 0f);
                            break;
                        case 1: // 왼쪽                      
                            instance.transform.localPosition = new Vector3(m_LeftBottom.x - offset, InputY, 0f);
                            break;
                        case 2: // 오른쪽                    
                            instance.transform.localPosition = new Vector3(m_RightTop.x + offset, InputY, 0f);
                            break;
                        case 3: // 아래쪽                    
                            instance.transform.localPosition = new Vector3(InputX, m_LeftBottom.y - offset, 0f);
                            break;
                    }

                    Vector3 targetPos = player.transform.localPosition - instance.transform.localPosition;
                    targetPos = targetPos.normalized;
                    targetPos *= speed;
                    instance.GetComponent<Rigidbody>().AddRelativeForce(targetPos);
                }
            }
        }
    }

    // Invoke용 함수.
    private void HelpScript()
    {
        Help help = GameObject.Find("Help").GetComponent<Help>();
        help.HelpText.text = "Tab을 눌러 그만 보기";
        help.HelpCanvas.SetActive(true);
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().SetScript("[좌,우 이동키로 카메라 전환이 가능]");
    }
}
