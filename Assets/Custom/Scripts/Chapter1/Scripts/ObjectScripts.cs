using UnityEngine;
using UnityEngine.UI;

public class ObjectScripts : MonoBehaviour {

    public GameObject Story;

    public Text ChangeText;

    private float time = 0;

    private FirstPersonCamera player;

    private Inventory inven;

    void Start () {

        player = FirstPersonCamera.player;

        Story.SetActive(false);

        inven = GameObject.Find("Inventory").GetComponent<Inventory>();
    }
	
	void Update () {

        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (hit.transform.CompareTag("Handle")
                    || hit.transform.CompareTag("Outline"))
                    {
                        Story.SetActive(true);
                        time = 0f;
                        ChangeText.text = "";

                        Debug.Log("ObjectText : " + hit.transform.name);
                        switch (hit.transform.name)
                        {
                            case "folder":
                                ChangeText.text = "오래되어 보이는 파일이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "Bottle3":
                                ChangeText.text = "물이 들어있지는 않다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "Can_3 (1)":
                                ChangeText.text = "통조림이다. 지금은 배가 고프지 않아";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "S_Paper6":
                                ChangeText.text = "이런 것이 왜 여기 있을까?";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "S_Paper":
                                ChangeText.text = "실종자 전단지이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "S_Paper (7)":
                                ChangeText.text = "오래된 문명의 그림이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "Cigarette (3)":
                                ChangeText.text = "침대 아래에 담배가 떨어져있다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "old_wooden_door":
                                if (!GameObject.Find("Inventory").GetComponent<Inventory>().BlueKey)
                                {
                                    ChangeText.text = "잠겨있다. 열쇠나 도구가 필요할 것 같아.";
                                    SoundManager.instance.SelectSound(hit.transform.name);
                                }
                                else
                                {
                                    SoundManager.instance.SelectSound(hit.transform.name);
                                }
                                break;
                            case "jail_door":
                                if (!player.gameObject.GetComponent<Attack_Ani>().AxeCheck)
                                {
                                    ChangeText.text = "잠겨있지만 문을 부술 수는 있을 것 같아.";
                                    SoundManager.instance.SelectSound(hit.transform.name);
                                }
                                else
                                {
                                    SoundManager.instance.SelectSound(hit.transform.name);
                                }
                                break;
                            case "kos":
                                ChangeText.text = "냄새...";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "radio_low_poly (2)":
                                ChangeText.text = "무전기다. 어떻게 작동하는거지?";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "pPlane3":
                                ChangeText.text = "침대 아래에 있던 그 담배야";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "Lighter":
                                ChangeText.text = "다 쓴 라이터다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "book_0001a (1)":
                                ChangeText.text = "문명에 관한 이야기가 적혀있다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "book_0001b (1)":
                                ChangeText.text = "희귀 동물에 관련된 서적이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "DoorLeftGrp":
                            case "DoorRightGrp":
                                ChangeText.text = "잠겨 있어서 열어볼 수 없다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "cctvdoor":
                                ChangeText.text = "문이 잠겨있다. 다른 곳을 살펴보자.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "cabinetDoor":
                                if (GameObject.Find("cabinetDoor").GetComponent<Interact_Cabinet>().isOpened)
                                    ChangeText.text = "안에는 아무 것도 없어.";
                                break;
                            case "Topor_lod0":
                                ChangeText.text = "날이 녹슨 것 같다. 쓰지 말자.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "M9_Knife":
                                ChangeText.text = "이런 위험한 건 쓰는게 아니라고 배웠어.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "bread_01":
                                ChangeText.text = "누군가가 먹다 남긴 빵이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "knife_01":
                                ChangeText.text = "빵을 자를 때 사용한 칼이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "TeapotBase_LOD0":
                                ChangeText.text = "도자기로 만든 주전자다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "Teacup_LOD0":
                                ChangeText.text = "도자기로 만든 컵이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "chukies_02":
                                ChangeText.text = "딸기맛 시리얼인가?";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "Tabledraw1":
                                ChangeText.text = "으악, 총이잖아?";
                                break;
                            case "shoes":
                                ChangeText.text = "낡은 신발이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "Beddraw01":
                            case "Beddraw02":
                            case "kitchendraw03":
                            case "tvStandDoor_R":
                            case "tvStandDoor_L":
                            case "BathroomdoorR":
                                ChangeText.text = "뭔가 있어. 읽어보자";
                                break;
                            case "Mug_LOD0":
                                ChangeText.text = "사용한지 얼마 안 되어 보이는 컵이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "PFB_Fridge":
                                ChangeText.text = "지금 냉장고를 열어 볼 필요는 없어.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "PFB_Stove":
                                ChangeText.text = "비싸 보이는 가스레인지다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "skeleton_statue":
                                ChangeText.text = "해골 모양의 동상이다.";
                                break;
                            case "skeleton_statue (1)":
                                ChangeText.text = "움직일 수 있을 것 같은데...";
                                break;
                            case "door":
                                ChangeText.text = "잠겨있어. 힘으로는 열 수 없을 것 같아.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "DoorPadInteraction":
                                ChangeText.text = "이거 돌아가는 것 같은데?";
                                break;
                            case "Bucket":
                                ChangeText.text = "여기도 이상한 석판이 있네...";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "SubstationBig":
                                if (GameObject.Find("ElectricalBox").GetComponent<StopGenerator>().green == true && GameObject.Find("ElectricalBox").GetComponent<StopGenerator>().yellow == true)
                                    ChangeText.text = "좋아! 멈췄어! 이제 밖으로 나가자";
                                else
                                    ChangeText.text = "이걸 멈춰야 하는데.. 방법이...";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "cover":
                                ChangeText.text = "전력선이다! 무엇을 끊어야하지?";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "Generator":
                                ChangeText.text = "이동용 발전기다. 오래된 것 같아.";
                                break;
                            case "binbag 8":
                                ChangeText.text = "뭐가 담겨 있을까...?";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "sign":
                                ChangeText.text = "이정표인 것 같은데... 어디를 뜻하는 걸까?";
                                break;
                            case "RedDisc":
                                ChangeText.text = "빨간 원 모양이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "BlueDisc":
                                ChangeText.text = "파란 원 모양이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "BlackDisc":
                                ChangeText.text = "까만 원 모양이다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "fireext":
                                ChangeText.text = "소화기다. 오래된 것 같아.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "tv":
                                ChangeText.text = "오래된 TV다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "brocken projector":
                                ChangeText.text = "빔인가?";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "panel1":
                            case "panel3":
                                ChangeText.text = "무언가를 조작하는 기계다.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "disc 4":
                                ChangeText.text = "어디에 쓰는 물건일까?";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "projector 4":
                                ChangeText.text = "근처에서 본 것 같아.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "pencere2":
                            case "pencere3":
                                ChangeText.text = "잠겨있어.";
                                SoundManager.instance.SelectSound(hit.transform.name);
                                break;
                            case "Ch5ExitDoor":
                                ChangeText.text = "집안을 조금 더 찾아보자.";
                                if (inven.Circle1 == true)
                                {
                                    ChangeText.text = "";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else if (hit.transform.CompareTag("Item"))
                    {
                        Story.SetActive(true);
                        time = 0f;
                        ChangeText.text = "아이템을 습득했다.";
                    }

                    else if (hit.transform.CompareTag("Totem"))
                    {
                        Story.SetActive(true);
                        time = 0f;
                        ChangeText.text = "집안에 변화가 생긴 것 같아.";
                    }
                }
            }
        }

        // 오브젝트 스크립트가 표시되고 있다면.
        if (Story.activeSelf == true)
        {
            time += Time.deltaTime;

            if (time >= 2)
            {
                Story.SetActive(false);
                time = 0f;
            }
        }    
    }

    // 외부 접근용... (이런건 최대한 자제...)
    public void SetScript(string text)
    {
        Story.SetActive(true);
        time = 0f;
        ChangeText.text = text;
    }
}
