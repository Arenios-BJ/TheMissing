using UnityEngine;

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
                    if (hit.transform.name == "Circle_mold")
                    {
                        if (CirCheck == true)
                        {
                            SoundManager.instance.SelectSound(hit.transform.name);

                            CircleCount++;

                            if (CircleCount == 1)
                            {
                                if (cir.Circle1 == true)
                                {
                                    circle1.SetActive(true);
                                    inven.items.Remove(GameObject.Find("Item_CircleElectric"));
                                    Destroy(GameObject.Find("Item_CircleElectric"));
                                }
                            }

                            if (CircleCount == 2)
                            {
                                if (cir.Circle2 == true)
                                {
                                    circle2.SetActive(true);
                                    inven.items.Remove(GameObject.Find("Item_CircleSub"));
                                    Destroy(GameObject.Find("Item_CircleSub"));
                                }
                            }

                            if (CircleCount == 3)
                            {
                                if (cir.Circle3 == true)
                                {
                                    circle3.SetActive(true);
                                    inven.items.Remove(GameObject.Find("Item_CircleBoss"));
                                    Destroy(GameObject.Find("Item_CircleBoss"));
                                }
                            }

                            if (CircleCount == 4)
                            {
                                if (cir.Circle4 == true)
                                {
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
