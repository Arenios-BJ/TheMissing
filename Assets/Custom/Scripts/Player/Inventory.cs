using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct ItemInfo   // 아이템 설명서의 구조체.
{
    public string objectName;   // 영문. prefab에 쓰이는 이름과 동일.
    public string itemName;     // 한글. 설명창에 띄어줄 이름.
    public string script;       // 아이템 설명.
}
[System.Serializable]
public class ItemInfoArray  // 아이템 설명집?
{
    public ItemInfo[] data;
}

public class Inventory : MonoBehaviour
{
    public GameManager_Ch3 poolList;
    public List<GameObject> getStone = new List<GameObject>();

    [SerializeField] private Button usedButton;                 // '사용하기' 버튼. 상호작용 상태 변경을 위해.
    [SerializeField] private GameObject itemBackgroundPrefab;   // 아이템의 배경 프리팹.
    [SerializeField] private TextAsset itemText;                // Assets 폴더에서 json 파일을 읽어와 저장할 객체.
    private ItemInfoArray itemScripts;      // 읽어온 json 파일을 파싱하여 담은 객체.

    private FirstPersonCamera player;       // RaycastHit의 정보를 가지고 있는 player 스크립트.
    private MouseLock mouse;                // 인벤토리 상태에 따른 마우스 잠금을 위한 마우스 상태 관리 스크립트.
    private EventSystem eventSystem;        // Graphic(UI) Raycast를 사용하기 위한 UI요소 관련 이벤트 정보를 가지고 있는 객체.
    private IEnumerator coItemPick;         // ItemPick() 코루틴의 정보를 담는 변수.

    public List<GameObject> items = new List<GameObject>(); // 아이템들이 담길 리스트
    public List<Sprite> itemPrefabs;    // 아이템 프리팹(생성 리소스)가 담길 리스트.
    public CanvasGroup inventoryCG;     // 인벤토리 영역의 CanvasGroup 컴포넌트.
    public Text itemInfo;               // 아이템 설명을 표시할 Text 컴포넌트.
    public GameObject itemListBox;      // 아이템 아이콘들이 표시되는 이미지 영역.

    private GraphicRaycaster raycaster;  // UI 요소용 RayCaster.
    private PointerEventData pointerEventData;  // EventSystem 내부의 InputModule 및 Input 이벤트의 결과값을 가진 객체. (여러종류)
    private List<RaycastResult> raycastResults = new List<RaycastResult>(); // GraphicRaycaster.Raycast는 충돌된 모든 UI 요소를 반환하기 때문에 리스트 형식이 필요.
    private GameObject fstSelected = null, sndSelected = null;  // 아이템 선택(Pick) 분별 작업에 사용되는 빈 객체.
    private GameObject selectedItem = null; // 선택된 아이템을 저장할 객체.
    private string itemName = null;

    public bool IsOpened { get; private set; }  // 인벤토리가 열렸는지 여부 판단.

    public bool BlueKey; // 파란 열쇠를 먹었는지 여부 판단

    public bool CarKey;
    public bool PowerKey;
    public bool Hint;

    public bool Circle1;
    public bool Circle2;
    public bool Circle3;
    public bool Circle4;

    public GameObject ImgHint;

    private void Awake()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += sceneloaded;
        ImgHint.SetActive(false);

        Circle1 = false;
        Circle2 = false;
        Circle3 = false;
        Circle4 = false;
        CarKey = false;
        PowerKey = false;
    }

    private void sceneloaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "Chapter3")
        {
            poolList = GameObject.Find("GameManager_Ch3").GetComponent<GameManager_Ch3>();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= sceneloaded;
        }
    }

    void Start()
    {
        if (!itemText)
            itemText = Resources.Load("Prefabs/Items/ItemText") as TextAsset;   // json 파일 로드.
        if (itemText)
            itemScripts = JsonUtility.FromJson<ItemInfoArray>(itemText.ToString()); // 파싱.

        // 직접 할당.
        if (player == null)
            player = FirstPersonCamera.player;
        if (mouse == null)
            mouse = GameManager.gm.GetComponent<MouseLock>();
        if (eventSystem == null)
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        // Canvas에 기존으로 붙어있는 인스턴스를 사용한다.
        raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        // 보통 이벤트 함수의 매개변수로 사용되기에 기타 외부 함수에서 쓰려면 특정 EventSystem을 걸어놓고 생성한다.
        pointerEventData = new PointerEventData(eventSystem);

        IsOpened = false; // 인벤토리가 처음 시작했을 때는 닫혀있으니까 false로 시작한다.

        BlueKey = false;
        Hint = false;
    }

    void Update()
    {
        // 게임 종료 패널이 열려있다면 이하 모든 프로세스 패스.
        if (GameManager.gm.panelOpen)
            return;

        // 아이템 습득.
        // 포인터(마우스커서)가 EventSystem(GUI) 위에 있지 않을 때.
        if (EventSystem.current.IsPointerOverGameObject() == false)
            if (Input.GetMouseButtonUp(0))      // 마우스 '왼쪽' 버튼을 클릭했다가 떼었을 때,
                ItemGain();                     // 제가 아이템을 한 번 먹어보겠습니다.

        // 인벤토리 열고 닫기.
        if (IsOpened == false)  // 인벤토리가 열려있지 않다면.
        {
            if (Input.GetKeyDown(KeyCode.I))    // 'I'키를 눌렀을 때.
            {
                IsOpened = true; // 인벤토리 체크여부를 true로 바꿔준다.
                inventoryCG.interactable = true;            // 상호작용 활성화.
                inventoryCG.blocksRaycasts = true;          // (그래픽 == UI) 레이캐스트 충돌 활성화.
                inventoryCG.alpha = IsOpened ? 1f : 0f;     // ? <- 삼항 연산자. 조건 ? 참일 경우 : 거짓일 경우
                coItemPick = ItemPick();                    // 코루틴 함수를 IEnumerator 열거자형 변수에 담아준다.
                StartCoroutine(coItemPick);                 // 아이템 선택 작업(코루틴) 시작. ('아이템 사용'의 선결 작업)
                mouse.ChangeMouseLock(false);   // 마우스 잠금 해제.
                Time.timeScale = 0.0f;  // 게임을 일시 정지.
                SoundManager.instance.InvenS();
            }
        }
        else if (IsOpened == true)   // 인벤토리가 열려있다면
        {
            if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
            {
                IsOpened = false; // 다시 false로 바꿔주고
                inventoryCG.alpha = IsOpened ? 1f : 0f;
                inventoryCG.interactable = false;       // 상호작용 비활성화.
                inventoryCG.blocksRaycasts = false;     // (그래픽 == UI)레이캐스트 충돌 비활성화.
                coItemPick.MoveNext();                  // ItemPick 코루틴을 종료하기 위해 대기중인 yield를 강제 호출한다.

                mouse.ChangeMouseLock(true);    // 마우스 강제 잠금.
                Time.timeScale = 1.0f; // 일시 정지를 푼다.
            }
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameObject.Find("Img_Map"))
            {
                GameObject.Find("Img_Map").SetActive(false);
                Time.timeScale = 1f;
            }

            if (GameObject.Find("Img_Hint"))
            {
                GameObject.Find("Img_Hint").SetActive(false);
                Time.timeScale = 1f;
            }
        }

        // 인벤토리가 열려있지 않다면 이후 작업은 실행할 필요가 없기 때문.
        if (!IsOpened)
            return;

        // 아무 아이템도 선택되어 있지 않거나, 플레이어가 직접적으로 사용하는 아이템이 아닐 시
        // '사용하기' 버튼 비활성화.
        if (selectedItem == null || selectedItem.name.Contains("Key") || selectedItem.name.Contains("Circle"))
            usedButton.interactable = false;
        else
        {
            // 선택된 아이템이 '건전지'일 경우 손전등의 사용 유무에 따른 '사용하기' 버튼 상태 설정.
            if (selectedItem.name == "Item_Battery")
                if (player.m_bHasFlashlight == false)
                {
                    usedButton.interactable = false;
                    return;
                }

            usedButton.interactable = true;     // 하나라도 선택되어 있으면 활성화.
        }
    }

    // 아이템을 습득하는 함수.
    private void ItemGain()
    {
        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (hit.transform.CompareTag("Item"))   // 그 충돌한 무언가의 태그가 아이템이라면.
            {
                if (itemPrefabs.Count > 0)          // itemPrefab, 즉 인게임 아이템 DB가 0보다 크다면.
                {
                    GameObject tmp = null;          // 인벤토리에 없는 아이템을 경우 생성을 위해 미리 빈 껍데기 생성.
                    for (int i = 0; i < itemPrefabs.Count; i++) // DB의 수 만큼 돈다. (비효율이겠지... orz)
                    {
                        if (hit.transform.name.Contains(itemPrefabs[i].name))   // 먹을 아이템의 이름과 DB 내의 아이템의 이름을 비교.
                        {
                            bool alreadyExist = false;      // 인벤토리에 같은 아이템이 존재할 경우를 판단하기 위한 변수.
                            for (int j = 0; j < items.Count; j++)   // 인벤토리에 아이템이 존재한다면 반복 검사.
                            {
                                if (items[j].name.Contains(hit.transform.name))    // 인벤토리 내의 아이템과 먹을 아이템의 이름을 비교.
                                {
                                    int count = int.Parse(items[j].transform.Find("Text").GetComponent<Text>().text);   // 기존 아이템의 갯수를 받아와서.
                                    count++;    // 증가시켜주고.
                                    items[j].transform.Find("Text").GetComponent<Text>().text = count.ToString();       // 다시 갯수 갱신.

                                    alreadyExist = true;    // 그리고 이미 있으니까 true 바꿔준다.
                                    break;                  // 아이템 추가했으니 반복문 탈출.
                                }
                            }

                            if (alreadyExist == true)
                                break;                      // 아이템 추가했으니 여기도 탈출.

                            // 여기까지 왔으면 인벤토리에 없는 아이템이니 새로 생성해서 추가.
                            tmp = Instantiate(itemBackgroundPrefab);    // 생성해서 정보를 간직하고.
                            tmp.transform.Find("ItemImage").GetComponent<RawImage>().texture = itemPrefabs[i].texture;  // 하위에 아이템 이미지를 교체하고.
                            tmp.transform.Find("Text").GetComponent<Text>().text = "1"; // 아이템 갯수 1로 바꿔주고.
                            tmp.name = hit.transform.name;              // 먹은 아이템으로 이름을 바꿔주고.
                            tmp.transform.SetParent(itemListBox.transform); // 인벤토리 밑으로 이동.
                            items.Add(tmp); // 그리고 아이템 리스트에 추가.
                        }
                    }
                }

                if (hit.transform.name == "Item_Stone")
                {
                    if (poolList != null)
                    {
                        Debug.Log("돌 줍줍");
                        hit.collider.gameObject.SetActive(false);
                        poolList.MyStone.Add(hit.collider.gameObject);
                        poolList.StonePool.Remove(hit.collider.gameObject);
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }
                }
                else
                    Destroy(hit.transform.gameObject);  // 맵 상의 아이템 오브젝트 제거.

                if (hit.transform.name == "Item_BlueKey")
                {
                    BlueKey = true;
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_CarKey")
                {
                    CarKey = true;
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_PowerKey")
                {
                    PowerKey = true;
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_Hint")
                {
                    Hint = true;
                    GameObject.Find("MainCamera").GetComponent<Camera>().fieldOfView = 60;
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_CircleElectric")
                {
                    Circle1 = true;
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_CircleSub")
                {
                    Circle2 = true;
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_CircleBoss")
                {
                    Circle3 = true;
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_CircleCCTV")
                {
                    Circle4 = true;
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_Battery")
                {
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_Compass")
                {
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_Axe")
                {
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_Flashlight")
                {
                    SoundManager.instance.SelectSound(hit.transform.name);
                }

                if (hit.transform.name == "Item_Map")
                {
                    SoundManager.instance.SelectSound(hit.transform.name);
                }
            }
        }
    }

    // 아이템(슬롯)을 선택하는 함수.
    private IEnumerator ItemPick()
    {
        WaitUntil until = new WaitUntil(() => Input.GetMouseButtonUp(0));   // 마우스 '왼쪽' 클릭할 때 코루틴 다음 구문 실행. (return용 객체 미리 생성)
        bool swap = true;   // PickProcess()의 인자값을 바꿔주기 위한 bool 변수.

        // 코루틴을 중지할 때까지 반복. == 인벤토리가 닫힐 때까지.
        while (IsOpened)
        {
            yield return until; // 최초 마우스 클릭을 기다림. 이후는 클릭 대기.

            if (IsOpened == false)  // 마우스 입력(until)을 대기하다가 인벤토리를 꺼버렸을 경우.
                break;

            if (swap)
                PickProcess(ref fstSelected, ref sndSelected);
            else
                PickProcess(ref sndSelected, ref fstSelected);
            swap = !swap;   // 번갈아가면서 위, 아래, 위, 아래.

            // 선택된 아이템에 해당하는 정보를 아이템 설명창에 출력해주는 구문.
            if (selectedItem == null)   // 아이템이 선택되지 않았으면 설명창 초기화.
                itemInfo.text = "";
            else
            {
                foreach (ItemInfo info in itemScripts.data)     // 선택된 아이템을 InfoArray에서 검색하여 설명창에 대입.
                    if (selectedItem.name.Contains(info.objectName))
                    {
                        itemInfo.text = info.itemName + "\n\n" + info.script;
                        itemName = info.itemName;
                    }
            }

            yield return null;  // until 이전에 null을 넘겨주는 이유는 until의 마우스 클릭 검사가 프레임 단위이기 때문에 프레임이 바뀌기 전에는 
                                // until의 값이 지속적으로 true가 된다. 그러므로 다시 until에 Input을 쓸 때에는 꼭 프레임을 넘겨주자.
        }

        // 코루틴 내부의 반복문이 끝났다는 것은! 인벤토리가 닫혔다는 얘기.
        if (selectedItem != null)
            selectedItem.GetComponent<Outline>().enabled = false;   // 선택되있던 슬롯의 아웃라인 비활성화.
        selectedItem = fstSelected = sndSelected = null;            // 사용된 Object 변수들 초기화.
        itemInfo.text = "";                                         // 인벤토리가 닫히면 설명창도 초기화.
    }

    // 코루틴 내부에서 사용될 '슬롯 선택 및 처리' 함수.
    private void PickProcess(ref GameObject fst, ref GameObject snd)
    {
        pointerEventData.position = Input.mousePosition;    // 현재 마우스 위치를 전달.
        raycastResults.Clear(); // Raycast의 결과값을 받기 전 초기화.
        raycaster.Raycast(pointerEventData, raycastResults);    // GraphicRaycaster.Raycast()

        // Raycast의 결과값이 담긴 리스트를 탐색. (foreach구문 사용 외의 개선 방향 탐구 필요)
        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.name.Contains("Item_"))   // 클릭 좌표의 객체 중 아이템 객체 유무 검사.
            {
                fst = result.gameObject;    // 있다면 객체 저장.
                break;  // 반복문 탈출
            }
            else
                fst = null; // 없다면 '사용하기'버튼 비활성화를 위해 null 처리를 해주자.
        }

        if (fst != null)    // 아이템이 선택되었다면.
        {
            fst.GetComponent<Outline>().enabled = true; // 선택된 아이템 슬롯은 아웃라인 활성화.
            if (fst == snd) // 기존 선택된 슬롯과 새로 선택한 슬롯이 같은 아이템이라면 패쑤.
                return;
            else if (snd != null)   // 기존에 선택된 슬롯이 있었다면 그 슬롯은 아웃라인 비활성화.
                snd.GetComponent<Outline>().enabled = false;

            selectedItem = fst; // 선택된 아이템을 따로 저장.
        }
        else    // 아이템이 선택 안 되었다면. == 아이템 슬롯 외의 빈 곳을 클릭.
        {
            if (snd != null)    // 기존에 선택된 아이템 슬롯이 있었다면 그 슬롯은 아웃라인 비활성화.
            {
                snd.GetComponent<Outline>().enabled = false;
                snd = null; // '사용하기'버튼 비활성화를 위해 null 처리를 해주자.
            }

            selectedItem = null;    // 선택된 아이템이 없으니 null.
        }
    }    

    public void ItemUse()
    {
        if (selectedItem == null)   // 선택된 아이템이 없으면. (어차피 버튼이 비활성화라 못 들어올테지만...)
            return;

        ItemFunctions itemFunction = new ItemFunctions();   // 아이템 기능을 구현하는 객체를 생성. (실행은 되지만 분석 필요)
        itemFunction.ItemFunction(itemName);                // 어떤 아이템의 기능을 쓸 것인지 인자값으로 이름 전달.

        if (itemName == "지도" || itemName == "단서")
        {
            // 인벤토리 창 닫음.
            IsOpened = false; // 다시 false로 바꿔주고
            inventoryCG.alpha = IsOpened ? 1f : 0f;
            inventoryCG.interactable = false;       // 상호작용 비활성화.
            inventoryCG.blocksRaycasts = false;     // (그래픽 == UI)레이캐스트 충돌 비활성화.
            coItemPick.MoveNext();                  // ItemPick 코루틴을 종료하기 위해 대기중인 yield를 강제 호출한다.
            mouse.ChangeMouseLock(true);            // 마우스 강제 잠금.
            selectedItem = null;
            return;
        }

        // 아이템 갯수 감소와 감소에 따른 슬롯 제거.
        int count = int.Parse(selectedItem.transform.Find("Text").GetComponent<Text>().text);
        count--;
        selectedItem.transform.Find("Text").GetComponent<Text>().text = count.ToString();
        if (count <= 0)
        {
            items.Remove(selectedItem); // 인벤토리 리스트에서 제거.
            Destroy(selectedItem);      // 게임(UI)에서 제거.
        }
        selectedItem = null;
    }

    // 저장된 인벤토리 데이터를 읽어서 인벤토리에 대입.
    public void SetSavedInventory(string _itemNames, string _itemCounts)
    {
        Debug.Log("Load : " + _itemNames);
        Debug.Log("Load : " + _itemCounts);
        string[] _items = _itemNames.Split('#');
        string[] _counts = _itemCounts.Split('#');

        GameObject tmp = null;
        for (int i = 0; i < _items.Length; i++)
        {
            if (_items.Length == 1 && _items[i] == "")
                break;

            int textureIdx = 0;
            for (int j = 0; j < itemPrefabs.Count; j++)
            {
                if (_items[i] == itemPrefabs[j].name)
                {
                    textureIdx = j;
                    break;
                }
            }

            tmp = Instantiate(itemBackgroundPrefab);    // 생성한 인스턴스 정보를 저장하고.
            tmp.transform.Find("ItemImage").GetComponent<RawImage>().texture = itemPrefabs[textureIdx].texture;  // 하위에 아이템 이미지를 교체하고.
            tmp.transform.Find("Text").GetComponent<Text>().text = _counts[i]; // 아이템 갯수 바꿔주고.
            tmp.name = "Item_" + _items[i];              // 이름 바꿔주고.
            tmp.transform.SetParent(itemListBox.transform); // 인벤토리 밑으로 이동.
            items.Add(tmp); // 그리고 아이템 리스트에 추가.

            // 아이템 유무 체크.
            if (tmp.name == "Item_CircleElectric")
                Circle1 = true;
            else if (tmp.name == "Item_CircleSub")
                Circle2 = true;
            else if (tmp.name == "Item_CircleBoss")
                Circle3 = true;
            else if (tmp.name == "Item_CircleCCTV")
                Circle4 = true;
            else if (tmp.name == "Item_CarKey")
                CarKey = true;
            else if (tmp.name == "Item_PowerKey")
                PowerKey = true;
        }
        tmp = null; // GC를 위해 다 쓴 변수는 null.
    }

    // 인벤토리 데이터를 PlayerPrefs에 저장.
    public void SaveInventory()
    {
        string _itemNames = "";
        string _itemCounts = "";

        // 인벤토리에 아이템이 없을 경우는 패쓰.
        if (items.Count != 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i != 0)
                {
                    // 후에 데이터 로드 시 구분할 첨자.
                    _itemNames += "#";
                    _itemCounts += "#";
                }

                // 'Item_'을 잘라서 추가.
                _itemNames += items[i].name.Substring(5);
                // 해당 아이템의 갯수를 추가.
                _itemCounts += items[i].transform.Find("Text").GetComponent<Text>().text;
            }
        }

        // PlayerPrefs에 저장.
        Debug.Log("Save : " + _itemNames);
        Debug.Log("Save : " + _itemCounts);
        PlayerPrefs.SetString("Inventory_Item", _itemNames);
        PlayerPrefs.SetString("Inventory_Count", _itemCounts);
    }
}
