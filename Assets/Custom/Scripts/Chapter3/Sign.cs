using UnityEngine;

public class Sign : MonoBehaviour {

    public GameObject SignCanvas;
    private FirstPersonCamera player;

    void Start () {

        SignCanvas.SetActive(false);
        player = FirstPersonCamera.player;
    }

    void Update()
    {

        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (hit.transform.name == "sign")   //
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameManager.gm.panelOpen == false)
                    {
                        SignCanvas.SetActive(true);
                        Time.timeScale = 0.0f;
                    }
                }

            }
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.gm.panelOpen == false)
            {
                SignCanvas.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
}
