using UnityEngine;
using UnityEngine.SceneManagement;

// 애니메이션 스크립트

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
                arms2.SetActive(true);

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

                                Destroy(GameObject.Find("Item_Axe (1)").GetComponent<CapsuleCollider>());
                            }
                        }
                    }
                }
            }

            if (AxeAni.GetCurrentAnimatorStateInfo(0).IsName("AxeAttack"))
            {
                Debug.Log("공격중");
                time += Time.deltaTime;
            }
        }
    }
}
