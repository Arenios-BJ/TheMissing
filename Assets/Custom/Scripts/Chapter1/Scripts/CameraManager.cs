using UnityEngine;

// 첫 번째 오두막에서 문이 부서졌을 때 연출을 위한 카메라 컨트롤 스크립트
// 사용한 방법 : bool / Animator / Find / FindWithTag / SetActive / AnimatorStateInfo / normalizedTime / Destroy
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
            // 문이 부서졌다면
            if (!jaildoor)
            {
                // 연출을 위한 카메라를 키고
                radiocam.SetActive(true);
                // 메인 카메라를 끈다.
                maincam.SetActive(false);

                mainCamCheck = false;
                CamsCheck = false;
            }
        }

        // 메인 카메라가 꺼졌을 때
        if(mainCamCheck == false)
        {
            AnimatorStateInfo stateinfo;
            if (ani)
            {
                //GetCurrentAnimatorStateInfo(0) -> 작동중인 애니메이션의 상태를 가져온다.
                stateinfo = ani.GetCurrentAnimatorStateInfo(0);

                // 1.0f보다 커졌다는 것은 작동중인 애니메이션이 끝났다는 뜻
                if (stateinfo.normalizedTime > 1.0f)
                {
                    // 연출을 위한 카메라를 삭제하고
                    Destroy(radiocam);
                    // 다시 메인 카메라를 킨다.
                    maincam.SetActive(true);

                    mainCamCheck = true;
                }
            }
        }
    }
}
