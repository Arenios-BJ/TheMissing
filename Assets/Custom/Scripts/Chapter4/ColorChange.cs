using UnityEngine;

// 보스의 집에서 자동차키가 들어있는 금고 스크립트
// 누를 때마다 빨강-초록-노랑 색깔이 변함
// 사용한 방법 : Animator / material.color / Find / enabled / RaycastHit

public class ColorChange : MonoBehaviour {

    public GameObject Red;
    public GameObject Green;
    public GameObject Yellow;

    public float RedCount;
    public float GreenCount;
    public float YellowCount;

    private Animator SafeDoorAni;

    private FirstPersonCamera player;

    void Start () {

        Red.GetComponent<MeshRenderer>().material.color = Color.red;
        Green.GetComponent<MeshRenderer>().material.color = Color.green;
        Yellow.GetComponent<MeshRenderer>().material.color = Color.yellow;

        RedCount = 0;
        GreenCount = 0;
        YellowCount = 0;

        SafeDoorAni = GameObject.Find("PowerDoor").GetComponent<Animator>();
        SafeDoorAni.enabled = false;

        player = FirstPersonCamera.player;
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
                    if (hit.transform.name == "PowerLock")
                    {
                        RedCount++;
                        SoundManager.instance.SelectSound(hit.transform.name);

                        if (RedCount == 1)
                        {
                            Red.GetComponent<MeshRenderer>().material.color = Color.green;
                        }

                        if (RedCount == 2)
                        {
                            Red.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        }

                        if (RedCount == 3)
                        {
                            Red.GetComponent<MeshRenderer>().material.color = Color.blue;
                        }

                        if (RedCount == 4)
                        {
                            Red.GetComponent<MeshRenderer>().material.color = Color.red;
                            RedCount = 0;
                        }

                    }

                    if (hit.transform.name == "PowerLock1")
                    {
                        GreenCount++;
                        SoundManager.instance.SelectSound(hit.transform.name);

                        if (GreenCount == 1)
                        {
                            Green.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        }

                        if (GreenCount == 2)
                        {
                            Green.GetComponent<MeshRenderer>().material.color = Color.blue;
                        }

                        if (GreenCount == 3)
                        {
                            Green.GetComponent<MeshRenderer>().material.color = Color.red;
                        }

                        if (GreenCount == 4)
                        {
                            Green.GetComponent<MeshRenderer>().material.color = Color.green;
                            GreenCount = 0;
                        }
                    }

                    if (hit.transform.name == "PowerLock2")
                    {
                        YellowCount++;
                        SoundManager.instance.SelectSound(hit.transform.name);

                        if (YellowCount == 1)
                        {
                            Yellow.GetComponent<MeshRenderer>().material.color = Color.blue;
                        }

                        if (YellowCount == 2)
                        {
                            Yellow.GetComponent<MeshRenderer>().material.color = Color.red;
                        }

                        if (YellowCount == 3)
                        {
                            Yellow.GetComponent<MeshRenderer>().material.color = Color.green;
                        }

                        if (YellowCount == 4)
                        {
                            Yellow.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            YellowCount = 0;
                        }
                    }
                }
            }
        }

        if(RedCount == 2 && GreenCount == 3 && YellowCount == 1)
        {
            SafeDoorAni.enabled = true;
            SoundManager.instance.ColorS();
            RedCount = 0;
            GreenCount = 0;
            YellowCount = 0;
        }
    }
}
