using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject maincam;
    public GameObject radiocam;

    private bool mainCamCheck; // 메인카메라를 사용중인가?
    private bool CamsCheck;

    private GameObject jaildoor;

    private Animator ani;

    private void Awake()
    {
        if (!radiocam)
            radiocam = GameObject.Find("RadioCamera");
        if (radiocam)
            ani = radiocam.GetComponent<Animator>();

        maincam = GameObject.FindWithTag("MainCamera");
        maincam.SetActive(true);
    }

    void Start () {
        radiocam.SetActive(false);

        mainCamCheck = true;

        jaildoor = GameObject.FindWithTag("Player").GetComponent<Attack_Ani>().jaildoor;

        CamsCheck = true;
    }
	
	void Update () {

        if (CamsCheck == true)
        {
            if (!jaildoor)
            {
                radiocam.SetActive(true);
                maincam.SetActive(false);

                mainCamCheck = false;
                CamsCheck = false;
            }
        }

        if(mainCamCheck == false)
        {
            AnimatorStateInfo stateinfo;
            if (ani)
            {
                stateinfo = ani.GetCurrentAnimatorStateInfo(0);

                if (stateinfo.normalizedTime > 1.0f)
                {
                    Destroy(radiocam);
                    maincam.SetActive(true);

                    mainCamCheck = true;
                }
            }
        }
    }
}
