using UnityEngine;

public class Window : MonoBehaviour {

    private Animator Window_Left;
    private FirstPersonCamera player;

    private bool WindowCheck;

    void Start () {

        Window_Left = GetComponent<Animator>();
        Window_Left.enabled = false;

        player = FirstPersonCamera.player;

        WindowCheck = false;
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
                    if (hit.transform.name == "Window_Left")
                    {
                        if (WindowCheck == false)
                        {
                            Window_Left.enabled = true;
                            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "좋아 여기로 들어갈 수 있겠어!";
                            SoundManager.instance.SelectSound(hit.transform.name);
                            WindowCheck = true;
                        }
                    }
                }
            }
        }
    }
}
