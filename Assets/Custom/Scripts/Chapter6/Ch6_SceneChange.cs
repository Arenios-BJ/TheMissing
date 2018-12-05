using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class Ch6_SceneChange : MonoBehaviour
{
    public float timer;
    public PlayableDirector director;
    public TimelineAsset time1, time2;
    public bool Chess_Check;
    public bool Chess_Check_End;
    public GameObject textCanvas;
    public GameObject Arrow;

    public GameObject Player;
    public Camera Ch6_camera;
    public AudioListener listener;

    private FirstPersonCamera player;
    private MouseLock mouse;

    public ParticleSystem Fire_Wall1;
    public ParticleSystem Fire_Wall2;
    public ParticleSystem Fire_Wall3;
    public ParticleSystem Fire_Wall4;

    private Light Fire_Light1;
    private Light Fire_Light2;
    private Light Fire_Light3;
    private Light Fire_Light4;

    public AudioSource Fire_Sound1;

    public bool _vitory;
    public bool _defeat;

    private float time;

    private bool mouseCheck;

    private void Start()
    {
        Fire_Light1 = GameObject.Find("WallOfFireLight").GetComponent<Light>();
        Fire_Light2 = GameObject.Find("WallOfFireLight (1)").GetComponent<Light>();
        Fire_Light3 = GameObject.Find("WallOfFireLight (2)").GetComponent<Light>();
        Fire_Light4 = GameObject.Find("WallOfFireLight (3)").GetComponent<Light>();

        Fire_Wall1.Stop();
        Fire_Wall2.Stop();
        Fire_Wall3.Stop();
        Fire_Wall4.Stop();

        Fire_Light1.enabled = false;
        Fire_Light2.enabled = false;
        Fire_Light3.enabled = false;
        Fire_Light4.enabled = false;

        director = GetComponent<PlayableDirector>();
        director.stopped += stop; //어떤 타임라인이든 끝나면 stop함수를 부른다(함수임에도 특이하게 ()를 안쓴다)
        Chess_Check = true;
        Chess_Check_End = false;

        Player = GameObject.FindWithTag("Player");
        Player.SetActive(false);

        _vitory = false;
        _defeat = false;

        mouseCheck = false;
    }


    public void Update()
    {
        timer += Time.deltaTime;
        ChangeScene();       

        if(_defeat == true)
        {
            time += Time.deltaTime;
        }
    }

    private void stop(PlayableDirector obj)
    {
        if (director.playableAsset.duration < timer)
        {
            if (Chess_Check == true)
            {
                SceneManager.LoadSceneAsync("ChessGame", LoadSceneMode.Additive);

                listener.enabled = false;
                GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(false);
                
                //director.stopped -= stop; //이 함수를 지우게되면 다시는 이 함수를 못불러오는 모양
                timer = 0.0f;
                Chess_Check = false;
                Arrow.SetActive(true);

            }
            else if (Chess_Check == false && _vitory == true)
            {
                Player.SetActive(true);
                Player.transform.position = new Vector3(364.8f, 105.1f, 292.3f);
                Ch6_camera.enabled = false;
                Fire_Sound1.Play();
            }
            else if(Chess_Check == false && _defeat == true)
            {
               
                if (time > 5f)
                {
                    GameObject.Find("Post_VinetteManager").GetComponent<Post_Vinet>().GameOver();
                    GameObject.Find("Post_VinetteManager").GetComponent<Post_Vinet>().text.SetActive(true);
                    time = 0;
                }
            }
        }
    }

    void ChangeScene()
    {
        if (Chess_Check_End == true)
        {
            listener.enabled = true;
            textCanvas.SetActive(true);

            if (mouseCheck == false)
            {
                GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(true);
                mouseCheck = true;
            }

            if (director.time > 15f)
            {
                Fire_Wall1.Play();
                Fire_Wall2.Play();
                Fire_Wall3.Play();
                Fire_Wall4.Play();
                Fire_Light1.enabled = true;
                Fire_Light2.enabled = true;
                Fire_Light3.enabled = true;
                Fire_Light4.enabled = true; 
            }
        }
    }
}
