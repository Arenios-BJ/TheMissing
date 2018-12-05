using UnityEngine;

public class HotelSafe : MonoBehaviour {

    private FirstPersonCamera player;
    private bool SafeCamera;
    private MouseLock mouse;

    public Transform posit;

    private Animator doorani;
    private bool dooranicheck;
    private bool unlock;

    public MeshCollider safe;

    // Use this for initialization
    void Start () {

        player = FirstPersonCamera.player;

        SafeCamera = false;

        mouse = GameManager.gm.GetComponent<MouseLock>();

        doorani = GameObject.Find("SafeDoor").GetComponent<Animator>();

        dooranicheck = true;
        unlock = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (hit.transform.name == "Safe")
                    {
                        if (SafeCamera == false)
                        {
                            safe.convex = false;
                            GameObject.Find("MainCamera").GetComponent<Camera>().fieldOfView = 7;
                            GameObject.Find("MainCamera").GetComponent<Camera>().transform.LookAt(posit);
                            SafeCamera = true;
                            mouse.ChangeMouseLock(false);
                            player.ChangeMoveRotaState(false);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.gm.panelOpen == false)
            {
                if (SafeCamera == true)
                {
                    if (unlock)
                        safe.convex = false;
                    else
                        safe.convex = true;
                    GameObject.Find("MainCamera").GetComponent<Camera>().fieldOfView = 60;
                    SafeCamera = false;
                    mouse.ChangeMouseLock(true);
                    player.ChangeMoveRotaState(true);
                }
            }
        }

        if (doorani.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor"))
        {
            if (dooranicheck == true)
            {
                unlock = true;
                safe.convex = false;
                mouse.ChangeMouseLock(true);
                dooranicheck = false;
                player.ChangeMoveRotaState(true);
            }
        }
    }

}
