using UnityEngine;

// 석판과 관련된 스크립트 -> 플레이어가 4가지의 유물을 먹고, 4가지의 유물을 끼울 때 스크립트
// 사용한 방법 : bool / Animator / Find / SetActive / enabled / RaycastHit / AnimatorStateInfo / Destroy

public class Circle : MonoBehaviour {

    public GameObject circle1;
    public GameObject circle2;
    public GameObject circle3;
    public GameObject circle4;

    private Inventory cir;

    private bool CirCheck;

    private FirstPersonCamera player;

    public float CircleCount;

    private Animator leftFence;
    private Animator rightFence;

    private Animator FenceCamera;

    public GameObject OpenfenceCamera;
    private bool FenceCameraCheck;

    public Animator skelet;
    public Animator skelet2;

    private Inventory inven;

    void Awake()
    {
        FenceCamera = GameObject.Find("OpenFenceCamera").GetComponent<Animator>();
    }


    void Start () {

        circle1.SetActive(false);
        circle2.SetActive(false);
        circle3.SetActive(false);
        circle4.SetActive(false);

        player = FirstPersonCamera.player;

        cir = GameObject.Find("Inventory").GetComponent<Inventory>();

        CirCheck = false;

        leftFence = GameObject.Find("Group229Left").GetComponent<Animator>();
        rightFence = GameObject.Find("Group230Right").GetComponent<Animator>();

        leftFence.enabled = false;
        rightFence.enabled = false;

        OpenfenceCamera.SetActive(false);

        FenceCameraCheck = false;

        skelet.enabled = false;
        skelet2.enabled = false;

        inven = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    void Update()
    {
        // 모든 석판(유물)을 먹었다면
        if (cir.Circle1 == true && cir.Circle2 == true && cir.Circle3 == true && cir.Circle4 == true)
        {
            CirCheck = true;
        }

        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    // 모든 유물을 가지고 있고, 유물을 끼우는 석판을 눌렀다면
                    if (hit.transform.name == "Circle_mold") 
                    {
                        if (CirCheck == true)
                        {
                            SoundManager.instance.SelectSound(hit.transform.name);

                            // CircleCount증가
                            CircleCount++;

                            // 첫 번째 유물이 끼워지고, 아이템 리스트에서 삭제
                            if (CircleCount == 1)
                            {
                                if (cir.Circle1 == true)
                                {
                                    circle1.SetActive(true);
                                    inven.items.Remove(GameObject.Find("Item_CircleElectric"));
                                    Destroy(GameObject.Find("Item_CircleElectric"));
                                }
                            }

                            // 두 번째 유물이 끼워지고, 아이템 리스트에서 삭제
                            if (CircleCount == 2)
                            {
                                if (cir.Circle2 == true)
                                {
                                    circle2.SetActive(true);
                                    inven.items.Remove(GameObject.Find("Item_CircleSub"));
                                    Destroy(GameObject.Find("Item_CircleSub"));
                                }
                            }

                            // 세 번째 유물이 끼워지고, 아이템 리스트에서 삭제
                            if (CircleCount == 3)
                            {
                                if (cir.Circle3 == true)
                                {
                                    circle3.SetActive(true);
                                    inven.items.Remove(GameObject.Find("Item_CircleBoss"));
                                    Destroy(GameObject.Find("Item_CircleBoss"));
                                }
                            }

                            // 그리고 마지막 네 번째 석판까지 끼웠다면
                            if (CircleCount == 4)
                            {
                                if (cir.Circle4 == true)
                                {
                                    // 각종 애니메이션과 연출 실행
                                    transform.parent.parent.tag = "Untagged";
                                    circle4.SetActive(true);
                                    CirCheck = false;
                                    leftFence.enabled = true;
                                    rightFence.enabled = true;
                                    OpenfenceCamera.SetActive(true);
                                    FenceCameraCheck = true;
                                    skelet.enabled = true;
                                    skelet2.enabled = true;
                                    SoundManager.instance.StoneS();
                                    inven.items.Remove(GameObject.Find("Item_CircleCCTV"));
                                    Destroy(GameObject.Find("Item_CircleCCTV"));
                                    GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "이제 지나갈 수 있겠어.";
                                }
                            }
                        }
                    }
                }
            }
        }

        // 문이 열리는 연출이 끝났다면, 해당되는 카메라를 삭제(필요없으니까)
        if(FenceCameraCheck == true)
        {
            AnimatorStateInfo stateinfo;
            stateinfo = FenceCamera.GetCurrentAnimatorStateInfo(0);

            if (stateinfo.normalizedTime > 1.0f)
            {
                Destroy(OpenfenceCamera);

                FenceCameraCheck = false;
            }
        }
    }
}
