using UnityEngine;

public class DoctorBook : MonoBehaviour {

    private FirstPersonCamera player;
    public GameObject BookCanvas1;
    public GameObject BookCanvas2;
    public GameObject BookCanvas3;

    private bool Page1;
    private bool Page2;
    private bool Page3;

    public GameObject NextButton;
    public GameObject BeforButton;

    private float count;

    private bool BookCheck;
    public bool isOpened { private set; get; }

    private MouseLock mouse;

    void Start () {

        player = Camera.main.transform.parent.GetComponent<FirstPersonCamera>();
        BookCanvas1.SetActive(false);
        BookCanvas2.SetActive(false);
        BookCanvas3.SetActive(false);

        BeforButton.SetActive(false);
        NextButton.SetActive(false);

        Page1 = false;
        Page2 = false;
        Page3 = false;

        BookCheck = false;
        isOpened = false;

        mouse = GameManager.gm.GetComponent<MouseLock>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (hit.transform.name == "DoctorBook")
                    {
                        if (BookCheck == false)
                        {
                            // 첫 번째 페이지 활성화
                            BookCanvas1.SetActive(true);
                            // 다음으로 버튼 활성화
                            NextButton.SetActive(true);
                            Page1 = true;
                            mouse.ChangeMouseLock(false);
                            Time.timeScale = 0.0f;
                            BookCheck = true;
                            isOpened = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.gm.panelOpen == false)
            {
                BookCanvas1.SetActive(false);
                BookCanvas2.SetActive(false);
                BookCanvas3.SetActive(false);
                BeforButton.SetActive(false);
                NextButton.SetActive(false);
                mouse.ChangeMouseLock(true);
                Time.timeScale = 1.0f;
                BookCheck = false;
            }
        }
    }

    public void NextBut()
    {
        // 첫 번째 페이지에서 다음으로 선택했을 때
        if(Page1 == true)
        {
            // 첫 번째 페이지는 숨기고
            BookCanvas1.SetActive(false);
            // 두 번째 페이지를 활성화
            BookCanvas2.SetActive(true);
            // 이전으로 버튼 활성화
            BeforButton.SetActive(true);
            Page2 = true;
            Page1 = false;
            SoundManager.instance.Paper_Touch();
        }
        // 두 번째 페이지에서 다음으로 선택했을 때
        else if(Page2 == true)
        {
            // 세 번째 페이지를 활성화
            BookCanvas3.SetActive(true);
            // 두 번째 페이지를 비활성화
            BookCanvas2.SetActive(false);
            // 다음으로 버튼 비활성화
            NextButton.SetActive(false);
            Page3 = true;
            Page2 = false;
            SoundManager.instance.Paper_Touch();
        }
    }

    public void BeforBut()
    {
        // 두 번째 페이지에서 이전으로 선택했을 때
        if (Page2 == true)
        {
            // 첫 번째 페이지 활성화
            BookCanvas1.SetActive(true);
            // 두 번째 페이지 비활성화
            BookCanvas2.SetActive(false);
            // 이전으로 버튼 비활성화
            BeforButton.SetActive(false);
            Page1 = true;
            Page2 = false;
            SoundManager.instance.Paper_Touch();
        }

        // 세 번째 페이지에서 이전으로 선택했을 때
        else if(Page3 == true)
        {
            // 세 번째 페이지 비활성화
            BookCanvas3.SetActive(false);
            // 두 번째 페이지 활성화
            BookCanvas2.SetActive(true);
            // 다음으로 버튼 활성화
            NextButton.SetActive(true);
            // 이전으로 버튼 활성화
            BeforButton.SetActive(true);
            Page3 = false;
            Page2 = true;
            SoundManager.instance.Paper_Touch();
        }
    }
}
