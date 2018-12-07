using UnityEngine;

// 첫 번째 오두막에서 그림을 제대로 맞췄을 때, 벽에 붙어있는 그림과 관련된 스크립트
// 사용한 방법 : bool / Translate / position / RaycastHit / Find / SetActive

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

        // 그림을 맞췄다면 액자 x의 위치가 243.9f보다 작을때까지만 이동한다.
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
        // 플레이어가 액자를 눌렀을 때, 그림 맞추기(미니 게임) 캔버스가 뜬다
        RaycastHit hit = player.getRaycastHit();    
        if (hit.transform != null)                  
        {
            if (hit.transform.name == "picture")   
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
