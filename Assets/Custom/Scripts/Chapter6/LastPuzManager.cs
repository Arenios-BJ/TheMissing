using UnityEngine;
using UnityEngine.SceneManagement;

// 마지막 퍼즐과 관련된 스크립트 -> 밟아야 할 석판을 제대로 밟았는가를 체크함
// 사용한 방법 : bool / Find / SetActive / enabled / OnCollisionEnter / transform.position

public class LastPuzManager : MonoBehaviour {

    public GameObject OkPanel;

    public int count;
    public bool LastPuzCheck;
    private bool LastPuzCheck1;
    private bool LastPuzCheck2;
    private bool LastPuzCheck3;
    public bool OkPanelCheck;
    public bool reset;
    private bool OpenCheck;

    public GameObject word;
    public GameObject word1;
    public GameObject word2;
    public GameObject word3;

    void sceneloaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Chapter6")
        {
            if (!word)
                word = GameObject.Find("munja");
            SceneManager.sceneLoaded -= sceneloaded;

            if (!word1)
                word1 = GameObject.Find("munja1");
            SceneManager.sceneLoaded -= sceneloaded;

            if (!word2)
                word2 = GameObject.Find("munja2");
            SceneManager.sceneLoaded -= sceneloaded;

            if (!word3)
                word3 = GameObject.Find("munja3");
            SceneManager.sceneLoaded -= sceneloaded;

            if (!OkPanel)
                OkPanel = GameObject.Find("munja16");
            SceneManager.sceneLoaded -= sceneloaded;

        }

    }

    void Awake()
    {
        SceneManager.sceneLoaded += sceneloaded;
    }


    void Start () {

        LastPuzCheck = false;
        LastPuzCheck1 = false;
        LastPuzCheck2 = false;
        LastPuzCheck3 = false;

        OkPanelCheck = false;

        reset = false;

        count = 0;

        OpenCheck = false;
    }
	
	void Update () {

        if (OpenCheck == false)
        {
            // 네 개의 석판을 제대로 밟았을 때
            if (LastPuzCheck == true && LastPuzCheck1 == true && LastPuzCheck2 == true && LastPuzCheck3 == true && OkPanelCheck == true)
            {
                reset = false;

                GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
                GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "문이 열렸어!";
                OpenCheck = true;

                GameObject.Find("Group230").GetComponent<FenceAni>().ani.enabled = true;
                GameObject.Find("Group230").GetComponent<FenceAni>().FenceSound.enabled = true;
            }

            // 석판을 제대로 밟지 않았을 때
            if (LastPuzCheck == false && OkPanelCheck == true)
            {
                GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
                GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "이게 아닌 것 같아.";

                LastPuzCheck = false;
                LastPuzCheck1 = false;
                LastPuzCheck2 = false;
                LastPuzCheck3 = false;

                OkPanelCheck = false;

                count = 1;

                // 플레이어는 정해진 위치로 자동 이동
                if (reset == true)
                {
                    transform.position = new Vector3(357, 106, 323);
                    reset = false;
                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {

        // 무조건 처음으로 켜져야 할 오브젝트
        if (col.gameObject.name == "munja1")
        {

            if (LastPuzCheck1 == false && LastPuzCheck2 == false && LastPuzCheck3 == false)
            {
                LastPuzCheck = true;
            }
        }

        if (col.gameObject.name == "munja")
        {

            if (LastPuzCheck == true && LastPuzCheck2 == false && LastPuzCheck3 == false)
            {
                LastPuzCheck1 = true;
            }
        }

        if (col.gameObject.name == "munja2")
        {

            if (LastPuzCheck == true && LastPuzCheck1 == true && LastPuzCheck3 == false)
            {
                LastPuzCheck2 = true;
            }
        }

        if (col.gameObject.name == "munja3")
        {

            if (LastPuzCheck == true && LastPuzCheck1 == true && LastPuzCheck2 == true)
            {
                LastPuzCheck3 = true;
            }
        }

        // 무조건 마지막에 눌러야 하는 발판
        if(col.gameObject.name == "munja16")
        {
            OkPanelCheck = true;
            reset = true;
        }

        // 정답이 아닌 것을 밟았을 때
        if (col.gameObject.name != "munja" && col.gameObject.name != "munja1" && col.gameObject.name != "munja2" && col.gameObject.name != "munja3" && col.gameObject.name != "munja16")
        {
            LastPuzCheck = false;
            LastPuzCheck1 = false;
            LastPuzCheck2 = false;
            LastPuzCheck3 = false;

            OkPanelCheck = false;

            reset = false;

            count = 0;

            OpenCheck = false;

        }

    }

    
}
