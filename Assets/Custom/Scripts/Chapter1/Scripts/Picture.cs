using UnityEngine;

public class Picture : MonoBehaviour {

    public GameObject canvas;

    public GameObject picture;

    private FirstPersonCamera player;
    private MouseLock mouse;

    public bool pic;

    public bool sound;

    void Start () {
        player = FirstPersonCamera.player;
        mouse = GameManager.gm.GetComponent<MouseLock>();

        pic = false;

        sound = false;
    }
	
	void Update () {

        OpenPicture();

        if(!canvas)
        {
            if (picture)
            {
                if (pic == false)
                {
                    picture.transform.Translate(Vector3.right * 1f * Time.deltaTime);

                    if (sound == false)
                    {
                        SoundManager.instance.FrameMove();
                        sound = true;
                    }

                    if (picture.transform.position.x < 243.9f)
                    {
                        pic = true;
                        picture.tag = "Untagged";
                    }
                }
            }
        }
     }

    public void OpenPicture()
    {
        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (hit.transform.name == "picture")   // 충돌한 이름이 "picture"라면
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameManager.gm.panelOpen == false)
                    {
                        GameObject.Find("Puzzle").GetComponent<Puz_Canvas_Manager>().Puzcanvas.SetActive(true);
                        mouse.ChangeMouseLock(false);
                        Time.timeScale = 0.0f;
                    }
                }
            }
        }
    }

}
