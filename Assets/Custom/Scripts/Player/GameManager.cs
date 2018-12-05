using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MouseLock))]
public class GameManager : MonoBehaviour {
    private static GameManager gameManager = null;
    public static GameManager gm
    {
        private set
        {
            if (value != null)
                gameManager = value;
        }
        get { return gameManager; }
    }

    [SerializeField] private int FirstLoadingScene = 1;
    private GameObject exitPanel = null;
    private bool isFirstLoaded = true;
    public bool panelOpen = false;
    public bool isPlayerMoved = true;
    public bool isMouseLocked = true;

    private void Awake()
    {
        if (!gameManager)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // 이 구문을 Awake()에서 처리하면 이 스크립트가 인스턴스되는 씬이 로드 되었을 때에도 이벤트를 호출.
        // Start()에서 처리하면 현재 씬이 아닌 다음 씬이 로드 되었을 때부터 이벤트를 호출.
        // 이를 통해, Awake() -> sceneLoaded 이벤트 처리 -> Start() 순서로 처리됨을 짐작할 수 있다.
        //
        // 새로운 Scene이 로드된 후에 실행될(혹은 해야할) 작업을 SceneManager에 추가.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        // 지그킥?
        if (Input.GetKeyUp(KeyCode.Home))
        {
            gm.panelOpen = false;
            PlayerPrefs.SetInt("Chapter", 0);
            GetComponent<MouseLock>().ChangeMouseLock(false);
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))   // 'Esc' 키를 눌렀을 때.
        {
            if (!exitPanel)
            {
                if (SceneManager.GetActiveScene().name == "Main")
                    exitPanel = GameObject.Find("Canvas").transform.Find("Pnl_ExitMenu").gameObject;
                else if (SceneManager.GetActiveScene().name == "Start")
                {
                    exitPanel = null;
                    return;
                }
            }

            if (exitPanel)
            {
                if (panelOpen)
                {
                    if (isPlayerMoved) // 기존에 timeScale이 1f 이었을 때.
                        Time.timeScale = 1f;

                    exitPanel.SetActive(false);
                    panelOpen = false;

                    if (isMouseLocked) // 기존에 마우스 상태가 lock이었을 때.
                        GetComponent<MouseLock>().ChangeMouseLock(true);
                }
                else
                {
                    // 기존에 timeScale이 0f 이었으면 false, 아니었으면 true.
                    isPlayerMoved = Time.timeScale == 0f ? false : true;
                    // 기존에 마우스 상태가 unlock이었으면 false, 아니었으면 true.
                    isMouseLocked = GetComponent<MouseLock>().m_IsLock ? true : false;

                    Time.timeScale = 0f;
                    exitPanel.SetActive(true);
                    panelOpen = true;
                    GetComponent<MouseLock>().ChangeMouseLock(false);
                }
            }
        }
    }

    private void OnDisable()
    {
        // Scene 로드 시 추가한 작업을 다시 제거.
        // (GameManager가 없어지면 게임이 끝난거라 사실 필요없는 구문이기도 함)
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerPrefs.Save();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.LogFormat("SceneLoaded : {0}, {1}, index : {2}", scene.name, mode, scene.buildIndex);

        if (scene.name == "Main")
        {
            gm.panelOpen = false;
            gm.GetComponent<MouseLock>().ChangeMouseLock(true);
            if (GameObject.Find("Pnl_ClimberGame"))
                GameObject.Find("Pnl_ClimberGame").SetActive(false);
            if (GameObject.Find("Img_Map"))
                GameObject.Find("Img_Map").SetActive(false);

            // 해당 값이 '0'일 때는 에디터로 테스트 중일 때.
            if (FirstLoadingScene == 0)
                return;

            // Chapter1 외의 Chapter를 불러오는 경우 PlayerPrefs에 저장된 데이터를 로딩.
            if (FirstLoadingScene != 1)
                LoadSavedData();

            // Main씬에 Additive로 ChapterN씬을 불러온다.
            SceneManager.LoadScene(FirstLoadingScene + 1, LoadSceneMode.Additive);
        }
        else if (scene.name != "Start") // Main도 아니고 Start도 아닌 Scene일 때.
        {
            // 게임 시작 시 플레이어의 처음 위치를 시작하는 챕터에 맞게 조정.
            if (isFirstLoaded)
            {
                isFirstLoaded = false;
                string _name = "Chapter" + FirstLoadingScene.ToString() + "_StartingPoint";
                if (GameObject.Find(_name))
                {
                    Transform _player = GameObject.FindWithTag("Player").transform;
                    Transform _point = GameObject.Find(_name).transform;

                    _player.position = _point.position;
                    _player.rotation = _point.rotation;
                }
            }

            // 체스게임 씬을 불러올 때는 패스.
            if (scene.name == "ChessGame")
                return;

            // 불러온 챕터의 정보(1챕터인지, 2챕터인지)를 PlayerPrefs에 저장.
            PlayerPrefs.SetInt("Chapter", scene.buildIndex - 1);
            // 나머지 데이터 저장.
            SaveData();
        }
    }

    // PlayerPrefs에 데이터 저장.
    private void SaveData()
    {
        // 인벤토리 저장.
        GameObject.Find("Inventory").GetComponent<Inventory>().SaveInventory();
        // 손전등 관련 정보 저장.
        if (FirstPersonCamera.player.m_bHasFlashlight)
        {
            // 배터리 잔량 값 저장.
            PlayerPrefs.SetInt("Battery", (int)(FirstPersonCamera.player.m_fCur_Battery * 100));
            PlayerPrefs.SetInt("Flashlight", 1);
        }
        else    // 손전등을 사용하지 않았을 경우.
        {
            PlayerPrefs.SetInt("Battery", 0);
            PlayerPrefs.SetInt("Flashlight", 0);
        }

        Debug.Log("GameManager.SaveData()");
    }

    // PlayerPrefs에서 데이터 불러오기.
    public void LoadSavedData()
    {
        // 인벤토리 정보.
        string items = PlayerPrefs.GetString("Inventory_Item");
        string counts = PlayerPrefs.GetString("Inventory_Count");
        GameObject.Find("Inventory").GetComponent<Inventory>().SetSavedInventory(items, counts);
        // 손전등 유무.
        int _Flash = PlayerPrefs.GetInt("Flashlight");
        if (_Flash == 1)
            FirstPersonCamera.player.m_bHasFlashlight = true;

        Debug.Log("GameManager.LoadSavedData()");
    }

    public void LoadSavedScene(int chapter_Idx = 1)
    {
        FirstLoadingScene = chapter_Idx;
        isFirstLoaded = true;
        SceneManager.LoadScene("Main");
    }
}
