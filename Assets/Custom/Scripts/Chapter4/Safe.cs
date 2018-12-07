using UnityEngine;
using UnityEngine.UI;

// 첫 번째 오두막에서, 열쇠의 비밀번호를 제대로 입력했는지 체크함
// 사용한 방법 : Animator / enabled / Find / enabled / Button / Instantiate / DetachChildren / childCount / GetChild / SetParent / Time.timeScale

public class Safe : MonoBehaviour
{
    public GameObject Safe_canvas;
    public GameObject Lock;
    public GameObject Chain;

    private Animator CabinetAni;
    private MouseLock mouse;

    // 숫자Text프리팹을 담을 GameObject
    // Button오브젝트 아래에 있음
    public GameObject OneText;
    public GameObject TwoText;
    public GameObject ThreeText;
    public GameObject FourText;
    public GameObject FiveText;
    public GameObject SixText;
    public GameObject SevenText;
    public GameObject EightText;
    public GameObject NineText;
    public GameObject ZeroText;
    public GameObject StarText;
    public GameObject ShapText;

    // 비밀번호 자판 패널
    // 패널에 달려있는 Button컴포넌트를 제어하기 위함
    private GameObject OnePanel;
    private GameObject TwoPanel;
    private GameObject ThreePanel;
    private GameObject FourPanel;
    private GameObject FivePanel;
    private GameObject SixPanel;
    private GameObject SevenPanel;
    private GameObject EightPanel;
    private GameObject NinePanel;
    private GameObject ZeroPanel;
    private GameObject StarPanel;
    private GameObject ShapPanel;

    // 비밀번호가 표시되는 패널
    private GameObject Pas1;
    private GameObject Pas2;
    private GameObject Pas3;
    private GameObject Pas4;

    // 공용 게임 오브젝트
    // 플레이어가 어떤 비밀번호를 입력할지 모르기 때문에 사용
    // 만약 플레이어가 2를 눌렀다면, STATIC은 2가 되고, 3을 눌렀다면 3이 된다.
    private GameObject STATIC;

    // 플레이어가 클릭한 횟수를 체크하기 위함
    float count;

    // 올바른 비밀 번호를 눌렀는지 확인하기 위함
    public bool check1;
    public bool check2;
    public bool check3;
    public bool check4;

    // '확인'버튼을 눌렀는지 확인하기 위함
    public bool Okcheck;

    private void Awake()
    {
        CabinetAni = GameObject.Find("Cabinet").GetComponent<Animator>();
        CabinetAni.enabled = false;
    }

    void Start()
    {
        mouse = GameManager.gm.GetComponent<MouseLock>();
        
        // Hierarchy에 있는 Pas1오브젝트를 (위에서 선언한)Pas1오브젝트에 담는다
        // 이하 위 설명과 같음
        Pas1 = GameObject.Find("Pas1");
        Pas2 = GameObject.Find("Pas2");
        Pas3 = GameObject.Find("Pas3");
        Pas4 = GameObject.Find("Pas4");

        // Hierarchy에 있는 OnePanel오브젝트를 (위에서 선언한)OnePanel오브젝트에 담는다
        // 이하 위 설명과 같음
        OnePanel = GameObject.Find("OnePanel");
        TwoPanel = GameObject.Find("TwoPanel");
        ThreePanel = GameObject.Find("ThreePanel");
        FourPanel = GameObject.Find("FourPanel");
        FivePanel = GameObject.Find("FivePanel");
        SixPanel = GameObject.Find("SixPanel");
        SevenPanel = GameObject.Find("SevenPanel");
        EightPanel = GameObject.Find("EightPanel");
        NinePanel = GameObject.Find("NinePanel");
        ZeroPanel = GameObject.Find("ZeroPanel");
        StarPanel = GameObject.Find("StarPanel");
        ShapPanel = GameObject.Find("ShapPanel");


        // 올바른 비밀번호를 확인하기 위한 bool 변수도 false 해준다.
        check1 = false;
        check2 = false;
        check3 = false;
        check4 = false;

        // '확인' 버튼을 false 한다.
        Okcheck = false;
    }


    void Update()
    {

        // 플레이어가 올바른 비밀번호를 눌렀는지 체크하는 함수
        OpenCheck();

        // count가 1, 2, 3 이라면
        if (count < 4)
        {
            // 모든 Button을 활성화 시킨다
            OnePanel.GetComponent<Button>().enabled = true;
            TwoPanel.GetComponent<Button>().enabled = true;
            ThreePanel.GetComponent<Button>().enabled = true;
            FourPanel.GetComponent<Button>().enabled = true;
            FivePanel.GetComponent<Button>().enabled = true;
            SixPanel.GetComponent<Button>().enabled = true;
            SevenPanel.GetComponent<Button>().enabled = true;
            EightPanel.GetComponent<Button>().enabled = true;
            NinePanel.GetComponent<Button>().enabled = true;
            ZeroPanel.GetComponent<Button>().enabled = true;
            StarPanel.GetComponent<Button>().enabled = true;
            ShapPanel.GetComponent<Button>().enabled = true;
        }

        // count가 4라면(즉, 비밀번호가 표시 되는 곳이 다 찼다면)
        if (count == 4)
        {
            // 모든 Button을 비활성화 시킨다. (누를 수 없도록)
            OnePanel.GetComponent<Button>().enabled = false;
            TwoPanel.GetComponent<Button>().enabled = false;
            ThreePanel.GetComponent<Button>().enabled = false;
            FourPanel.GetComponent<Button>().enabled = false;
            FivePanel.GetComponent<Button>().enabled = false;
            SixPanel.GetComponent<Button>().enabled = false;
            SevenPanel.GetComponent<Button>().enabled = false;
            EightPanel.GetComponent<Button>().enabled = false;
            NinePanel.GetComponent<Button>().enabled = false;
            ZeroPanel.GetComponent<Button>().enabled = false;
            StarPanel.GetComponent<Button>().enabled = false;
            ShapPanel.GetComponent<Button>().enabled = false;
        }

        // 비밀번호가 표시되는 곳을 확인함
        // count가 1이라면
        // 이하 설명 같음
        if (count == 1)
        {
            // 비밀번호가 표시되는 곳 첫 번째 칸을 활성화 시킨다.
            Pas1.GetComponent<Button>().enabled = true;
            // 비밀번호가 표시되는 곳 두 번째 칸을 비활성화 시킨다.
            Pas2.GetComponent<Button>().enabled = false;
        }

        if (count == 2)
        {
            Pas1.GetComponent<Button>().enabled = false;
            Pas2.GetComponent<Button>().enabled = true;
            Pas3.GetComponent<Button>().enabled = false;
        }

        if (count == 3)
        {
            Pas2.GetComponent<Button>().enabled = false;
            Pas3.GetComponent<Button>().enabled = true;
            Pas4.GetComponent<Button>().enabled = false;
        }

        if (count == 4)
        {
            Pas3.GetComponent<Button>().enabled = false;
            Pas4.GetComponent<Button>().enabled = true;
        }
    }

    public void OnClick(int a)
    {
        // 숫자 1을 눌렀다면
        if (a == 1)
        {
            SoundManager.instance.Button_Click();
            // 숫자1 프리팹을 생성한다
            OneText = Instantiate(OneText) as GameObject;
            // 위에서 선언한 공용 게임 오브젝트 STATIC는 OneText가 된다.
            STATIC = OneText;
            //카운트 증가
            count++;
            // 카운트 함수 호출
            // 카운트 함수는 프리팹이 생성될 부모를 정해주는 곳이다.
            // 자세한 설명은 Count 함수로!
            Count();

            // count는 4보다 커질 수 없다.
            if (count >= 4)
            {
                count = 4;
            }
        }

        // 숫자 2를 눌렀다면
        // 이하 설명 같음
        if (a == 2)
        {
            SoundManager.instance.Button_Click();
            TwoText = Instantiate(TwoText) as GameObject;
            STATIC = TwoText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        if (a == 3)
        {
            SoundManager.instance.Button_Click();
            ThreeText = Instantiate(ThreeText) as GameObject;
            STATIC = ThreeText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        if (a == 4)
        {
            SoundManager.instance.Button_Click();
            FourText = Instantiate(FourText) as GameObject;
            STATIC = FourText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        if (a == 5)
        {
            SoundManager.instance.Button_Click();
            FiveText = Instantiate(FiveText) as GameObject;
            STATIC = FiveText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        if (a == 6)
        {
            SoundManager.instance.Button_Click();
            SixText = Instantiate(SixText) as GameObject;
            STATIC = SixText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        if (a == 7)
        {
            SoundManager.instance.Button_Click();
            SevenText = Instantiate(SevenText) as GameObject;
            STATIC = SevenText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        if (a == 8)
        {
            SoundManager.instance.Button_Click();
            EightText = Instantiate(EightText) as GameObject;
            STATIC = EightText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        if (a == 9)
        {
            SoundManager.instance.Button_Click();
            NineText = Instantiate(NineText) as GameObject;
            STATIC = NineText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        if (a == 0)
        {
            SoundManager.instance.Button_Click();
            ZeroText = Instantiate(ZeroText) as GameObject;
            STATIC = ZeroText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        // *을 눌렀다면
        if (a == 11)
        {
            SoundManager.instance.Button_Click();
            StarText = Instantiate(StarText) as GameObject;
            STATIC = StarText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }
        }

        // #을 눌렀다면
        if (a == 12)
        {
            SoundManager.instance.Button_Click();
            ShapText = Instantiate(ShapText) as GameObject;
            STATIC = ShapText;
            count++;
            Count();

            if (count >= 4)
            {
                count = 4;
            }

        }

        // 비밀번호가 표시되는 곳 첫 번째 칸을 눌렀다면
        if (a == 20)
        {
            SoundManager.instance.Button_Click();
            //Destroy(Pas1.transform.GetChild(0).gameObject);

            //Pas1로부터 자식을 해제한다. (Pas1(부모)와 생성된 프리팹(자식)을 떼어놓는다)
            Pas1.transform.DetachChildren();
            // count 감소
            count--;

            // count는 0보다 작아질 수 없다.
            if (count <= 0)
            {
                count = 0;
            }

            // 확인 버튼을 false로 바꿔준다.
            Okcheck = false;
        }

        // 이하 설명 같음
        // 비밀번호가 표시되는 곳 두 번째 칸을 눌렀다면
        if (a == 21)
        {
            SoundManager.instance.Button_Click();
            //Destroy(Pas2.transform.GetChild(0).gameObject);
            Pas2.transform.DetachChildren();
            count--;

            if (count <= 0)
            {
                count = 0;
            }

            Okcheck = false;
        }

        // 비밀번호가 표시되는 곳 세 번째 칸을 눌렀다면
        if (a == 22)
        {
            SoundManager.instance.Button_Click();
            //Destroy(Pas3.transform.GetChild(0).gameObject);
            Pas3.transform.DetachChildren();
            count--;

            if (count <= 0)
            {
                count = 0;
            }

            Okcheck = false;
        }

        // 비밀번호가 표시되는 곳 네 번째 칸을 눌렀다면
        if (a == 23)
        {
            SoundManager.instance.Button_Click();
            //Destroy(Pas4.transform.GetChild(0).gameObject);
            Pas4.transform.DetachChildren();
            count--;

            if (count <= 0)
            {
                count = 0;
            }

            Okcheck = false;
        }

        // '확인'버튼을 눌렀다면
        if (a == 30)
        {
            SoundManager.instance.Button_Click();
            // 확인 버튼 체크를 true로 바꿔준다.
            Okcheck = true;
        }

        if (a == 31)
        {
            SoundManager.instance.Button_Click();
            count = 0;
            if(Pas1.transform.childCount == 1)
            {
                Pas1.transform.DetachChildren();
            }

            if (Pas2.transform.childCount == 1)
            {
                Pas2.transform.DetachChildren();
            }

            if (Pas3.transform.childCount == 1)
            {
                Pas3.transform.DetachChildren();
            }

            if (Pas4.transform.childCount == 1)
            {
                Pas4.transform.DetachChildren();
            }

        }
    }

    // Count함수는 프리팹이 생성될 부모를 정해주기 위한 함수이다.
    // 플레이어가 12가지의 숫자(0~9, *, #)중에서 어떤 것을 누를지 모르기 때문에, 비밀번호가 입력되는 곳 첫 번째 칸에는, 1이 들어갈 수도, 2가 들어갈 수도 있다.
    // 그렇기 때문에 위에서 어떤 숫자를 누르던 count++을 해주었던 것이다.
    void Count()
    {
        // 플레이어가 12가지의 숫자 중에서 어떤 것을 누르던 count는 1이된다.
        if (count == 1)
        {
            // STATIC는 공용 오브젝트로, 플레이어가 만약 1을 눌렀다면, OneText프리팹이 STATIC에 들어가게 된다.
            // 즉, [비밀번호가 입력 되는 곳], [첫 번째 칸]에는 [플레이어가 누른 숫자]가 들어가게 된다. 
            STATIC.transform.SetParent(Pas1.transform);
        }

        if (count == 2)
        {
            // [비밀번호가 입력 되는 곳], [두 번째 칸]에는 [플레이어가 누른 숫자]가 들어가게 된다. 
            STATIC.transform.SetParent(Pas2.transform);
        }

        if (count == 3)
        {
            // [비밀번호가 입력 되는 곳], [세 번째 칸]에는 [플레이어가 누른 숫자]가 들어가게 된다.
            STATIC.transform.SetParent(Pas3.transform);
        }

        if (count == 4)
        {
            // [비밀번호가 입력 되는 곳], [네 번째 칸]에는 [플레이어가 누른 숫자]가 들어가게 된다.
            STATIC.transform.SetParent(Pas4.transform);
        }
    }

    // 올바른 비밀 번호를 입력했는지 체크하기 위한 함수
    void OpenCheck()
    {
        if(Pas1.transform.childCount == 0)
        {
            check1 = false;
            Okcheck = false;
        }
        // Pas1의 자식이 하나이고
        else if (Pas1.transform.childCount == 1)
        {
            // Pas1이 가진 하나의 자식의 tag가 "ONE"이라면
            if (Pas1.transform.GetChild(0).tag == "ZERO")
            {
                // 비밀번호가 입력되는 곳 첫 번째 칸은 true가 된다.
                check1 = true;
            }
        }

        if (Pas2.transform.childCount == 0)
        {
            check2 = false;
            Okcheck = false;
        }
        // Pas2의 자식이 하나이고
        else if (Pas2.transform.childCount == 1)
        {
            // Pas2가 가진 하나의 자식의 tag가 "TWO"라면
            if (Pas2.transform.GetChild(0).tag == "SIX")
            {
                // 비밀번호가 입력되는 곳 두 번째 칸은 true가 된다.
                check2 = true;
            }
        }

        // 이하 설명 같음

        if (Pas3.transform.childCount == 0)
        {
            check3 = false;
            Okcheck = false;
        }
        else if (Pas3.transform.childCount == 1)
        {
            if (Pas3.transform.GetChild(0).tag == "ONE")
            {
                check3 = true;
            }
        }

        if (Pas4.transform.childCount == 0)
        {
            check4 = false;
            Okcheck = false;
        }
        else if (Pas4.transform.childCount == 1)
        {
            if (Pas4.transform.GetChild(0).tag == "TWO")
            {
                check4 = true;
            }
        }

        // 그래서 모든 숫자가 올바르게 들어가고, 확인버튼을 눌렀다면
        if (check1 == true && check2 == true && check3 == true && check4 == true && Okcheck == true)
        {
            // '문 열림'오브젝트가 활성화 되게 된다. 
            //Open.SetActive(true);
            Destroy(Safe_canvas);
            Destroy(Lock);
            Destroy(Chain);
            CabinetAni.enabled = true;
            GameObject.Find("DoorLeftGrp").tag = "Untagged";
            GameObject.Find("DoorRightGrp").tag = "Untagged";
            Time.timeScale = 1.0f;
            mouse.ChangeMouseLock(true);
            Destroy(GameObject.Find("OpenSafe"), 2f);
            SoundManager.instance.ChainDoor_Open();
            check1 = false;
            check2 = false;
            check3 = false;
            check4 = false;
            Okcheck = false;
        }
    }
}
