using UnityEngine;

// 첫 번째 오두막에서 금고 캔버스를 비활성화/활성화
// 금고 애니메이션 실행
// 사용한 방법 : SetActive / RaycastHit / Time.timeScale / Input.GetKeyDown

public class OpenSafe : MonoBehaviour
{
    // 금고 캔버스
    public GameObject safe_canvas;

    // 플레이어 레이캐스트
    private FirstPersonCamera player;
    private MouseLock mouse;

    void Start()
    {

        // 금고 캔버스 비활성화
        safe_canvas.SetActive(false);

        // 플레이어 레이캐스트 가져오기
        player = FirstPersonCamera.player;
        mouse = GameManager.gm.GetComponent<MouseLock>();

    }

    void Update()
    {
        Opensafe();
    }

    // 금고 활성화
    public void Opensafe()
    {
        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (hit.transform.name == "Lock")   //
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameManager.gm.panelOpen == false)
                    {
                        if (safe_canvas)
                        {
                            mouse.ChangeMouseLock(false);   // 마우스 잠금 해제.
                            safe_canvas.SetActive(true);
                            Time.timeScale = 0.0f;
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.gm.panelOpen == false)
            {
                if (safe_canvas.activeSelf)
                {
                    safe_canvas.SetActive(false);
                    mouse.ChangeMouseLock(true);
                    Time.timeScale = 1.0f;
                }
            }
        }
    }
}
