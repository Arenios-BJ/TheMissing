using UnityEngine;
using UnityEngine.EventSystems;

// 첫 번째 오두막에서 바닥에 있는 힌트를 눌렀을 때, 힌트가 UI로 뜨게하는 스크립트
// 사용한 방법 : SetActive / IsPointerOverGameObject / RaycastHit / Time.timeScale 

public class Hint : MonoBehaviour
{
    private FirstPersonCamera player;

    public GameObject hint1;
    public GameObject hint2;

    void Start()
    {

        player = FirstPersonCamera.player;

        hint1.SetActive(false);
        hint2.SetActive(false);

    }

    void Update()
    {

        Hint1();
    }

    void Hint1()
    {
        if (EventSystem.current != null)
            if (EventSystem.current.IsPointerOverGameObject())  // 마우스 포인터가 UI 위로 Over되면 True.
            {
                return;
            }

        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (hit.transform.name == "S_Paper6")   //
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hint1.SetActive(true);
                        Time.timeScale = 0.0f;
                    }
                }
            }

            if (hit.transform.name == "S_Paper8")   //
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hint2.SetActive(true);
                        Time.timeScale = 0.0f;
                        SoundManager.instance.SelectSound(hit.transform.name);
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
                Time.timeScale = 1.0f;
            }
        }
    }
}
