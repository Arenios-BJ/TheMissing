using UnityEngine;
using UnityEngine.SceneManagement;

// 첫 번째 오두막에서 도끼 애니메이션 스크립트
// 사용한 방법 : bool / Animator / Find / SetActive / Destroy / RaycastHit / SetBool / GetCurrentAnimatorStateInfo

public class Attack_Ani : MonoBehaviour
{
    public bool sound;

    private float time;

    public GameObject jaildoor;

    public GameObject arms2;
    private Animator AxeAni;
    public bool AxeCheck;

    private FirstPersonCamera player2;

    private bool storycheck;

    public PlayerNear PlayerN;


    void sceneloaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Chapter1")
        {
            if (!jaildoor)
                jaildoor = GameObject.Find("jail_door");
            if (!PlayerN)
                PlayerN = GameObject.Find("PlayerNear").GetComponent<PlayerNear>();
            SceneManager.sceneLoaded -= sceneloaded;
        }

    }

    void Awake()
    {
        SceneManager.sceneLoaded += sceneloaded;
        AxeAni = GameObject.Find("FpsArmsLow2").GetComponent<Animator>();
        arms2.SetActive(false);
    }

    void Start()
    {
        sound = false;

        player2 = FirstPersonCamera.player;

        AxeCheck = false; // 도끼를 얻었는가?

        time = 0;

        storycheck = true;
    }

    void Update()
    {
        Ani();

        // 도끼를 든 상태로 -> 문을 클릭했을 때 -> 4초가 지난다면 도끼-문과 관련된 모든 것들을 삭제
        if (time >= 4)
        {
            Destroy(arms2);
            Destroy(AxeAni);
            Destroy(jaildoor);

            if (sound == false)
            {
                SoundManager.instance.Broken_Door();
                GameObject.Find("_Environments").GetComponent<BackSound>().back.enabled = true;
                GameObject.Find("Player").GetComponent<FirstPersonCamera>().In = false;
                sound = true;
            }

            AxeCheck = false;

            // 도끼로 문을 부순 후 대사가 나온다.
            if (storycheck == true)
            {
                GameObject.Find("Ch1_Story").GetComponent<Ch1_Script>().Ch1Script.SetActive(true);
                storycheck = false;
            }
            player2.ChangeMoveRotaState(true);
            Destroy(this, 2f);
        }
    }

    void Ani()
    {
            if (AxeCheck == true) // 도끼를 사용했음
            {
                arms2.SetActive(true); // 도끼를 들고 있는 팔 등장!

            if (PlayerN.near == true)
            {
                RaycastHit hit = player2.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
                if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (GameManager.gm.panelOpen == false)
                        {
                            if (hit.transform.name == "jail_door")   // 충돌한 이름이 "jail_door"라면
                            {
                                AxeAni.SetBool("AxeAttack", true);
                                player2.ChangeMoveRotaState(false);

                                // 도끼에 붙어있던 콜라이더 삭제
                                // 이유 : 콜라이더가 붙어있으면 문에 달려있는 콜라이더와 충돌이 일어나서 플레이어가 팅기기 때문
                                Destroy(GameObject.Find("Item_Axe (1)").GetComponent<CapsuleCollider>());
                            }
                        }
                    }
                }
            }

            // 도끼를 휘두르는 애니메이션이 작동중이라면
            if (AxeAni.GetCurrentAnimatorStateInfo(0).IsName("AxeAttack"))
            {
                Debug.Log("공격중");
                time += Time.deltaTime;
            }
        }
    }
}
