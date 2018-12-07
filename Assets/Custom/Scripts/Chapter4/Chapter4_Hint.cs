using UnityEngine;

// 보스의 집에서 힌트와 관련된 오브젝트를 선택했을 때 UI로 뜨게하기 위함
// 사용한 방법 : SetActive / RaycastHit / switch / Time.timeScale / Input.GetKeyDown

public class Chapter4_Hint : MonoBehaviour
{
    private FirstPersonCamera player;

    public GameObject hint1;
    public GameObject hint2;
    public GameObject hint3;
    public GameObject hint4;
    public GameObject hint5;
    public GameObject hint6;
    public GameObject hint7;
    public GameObject hint8;
    public GameObject hint9;
    public GameObject hint13;
    public GameObject hint14;
    public GameObject hint15;
    public GameObject hint16;
    public GameObject hint17;


    void Start()
    {
        player = FirstPersonCamera.player;
        hint1.SetActive(false);
        hint2.SetActive(false);
        hint3.SetActive(false);
        hint4.SetActive(false);
        hint5.SetActive(false);
        hint6.SetActive(false);
        hint7.SetActive(false);
        hint8.SetActive(false);
        hint9.SetActive(false);
        hint13.SetActive(false);
        hint14.SetActive(false);
        hint15.SetActive(false);
        hint16.SetActive(false);
        hint17.SetActive(false);
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
                    switch (hit.transform.name)
                    {
                        case "S_Paper (18)":
                            hint1.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;
                        case "S_Paper (16)":
                            hint2.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;
                        case "S_Paper (17)":
                            hint3.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;
                        case "S_Paper (13)":
                            hint4.SetActive(true);
                            hint5.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;
                        case "S_Paper (14)":
                            hint6.SetActive(true);
                            hint7.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;
                        case "S_Paper1717":
                            hint8.SetActive(true);
                            Time.timeScale = 0.0f;
                            break;
                        case "S_Paper1818":
                            hint9.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;
                        case "Police_Pinboard":
                            hint13.SetActive(true);
                            hint14.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;
                        case "BrownBook":
                            hint15.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;
                        case "card":
                            hint16.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;
                        case "colorfull":
                            hint17.SetActive(true);
                            SoundManager.instance.SelectSound(hit.transform.name);
                            Time.timeScale = 0.0f;
                            break;

                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.gm.panelOpen == false)
            {
                hint1.SetActive(false);
                hint2.SetActive(false);
                hint3.SetActive(false);
                hint4.SetActive(false);
                hint5.SetActive(false);
                hint6.SetActive(false);
                hint7.SetActive(false);
                hint8.SetActive(false);
                hint9.SetActive(false);
                hint13.SetActive(false);
                hint14.SetActive(false);
                hint15.SetActive(false);
                hint16.SetActive(false);
                hint17.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
}
