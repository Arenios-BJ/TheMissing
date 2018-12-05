using UnityEngine;
using UnityEngine.EventSystems;

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
