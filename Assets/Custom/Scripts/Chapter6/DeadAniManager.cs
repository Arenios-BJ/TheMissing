using UnityEngine;

// 원주민들과 관련된 애니메이션 스크립트 -> enabled(false)상태로 있다가 플레이어가 차를 타는 순간 true상태로 바뀐다.
// 사용한 방법 : Animator / Find / enabled

public class DeadAniManager : MonoBehaviour {

    private Animator DeadShaman;
    private Animator DeadShaman1;
    private Animator DeadShaman2;
    private Animator DeadShaman3;
    private Animator DeadShaman4;
    private Animator DeadShaman5;
    private Animator DeadShaman6;
    private Animator DeadShaman7;
    private Animator DeadShaman8;
    private Animator DeadShaman9;
    private Animator DeadShaman10;
    private Animator DeadShaman11;
    private Animator DeadShaman12;
    private Animator DeadShaman13;

    void Start () {

        DeadShaman = GameObject.Find("DeadShaman_LOD").GetComponent<Animator>();
        DeadShaman1 = GameObject.Find("DeadShaman_LOD (1)").GetComponent<Animator>();
        DeadShaman2 = GameObject.Find("DeadShaman_LOD (2)").GetComponent<Animator>();
        DeadShaman3 = GameObject.Find("DeadShaman_LOD (3)").GetComponent<Animator>();
        DeadShaman4 = GameObject.Find("DeadShaman_LOD (5)").GetComponent<Animator>();
        DeadShaman5 = GameObject.Find("DeadShaman_LOD (6)").GetComponent<Animator>();
        DeadShaman6 = GameObject.Find("DeadShaman_LOD (7)").GetComponent<Animator>();
        DeadShaman7 = GameObject.Find("DeadShaman_LOD (8)").GetComponent<Animator>();
        DeadShaman8 = GameObject.Find("DeadShaman_LOD (10)").GetComponent<Animator>();
        DeadShaman9 = GameObject.Find("DeadShaman_LOD (11)").GetComponent<Animator>();
        DeadShaman10 = GameObject.Find("DeadShaman_LOD (12)").GetComponent<Animator>();
        DeadShaman11 = GameObject.Find("DeadShaman_LOD (15)").GetComponent<Animator>();
        DeadShaman12 = GameObject.Find("DeadShaman_LOD (16)").GetComponent<Animator>();
        DeadShaman13 = GameObject.Find("DeadShaman_LOD (17)").GetComponent<Animator>();

        DeadShaman.enabled = false;
        DeadShaman1.enabled = false;
        DeadShaman2.enabled = false;
        DeadShaman3.enabled = false;
        DeadShaman4.enabled = false;
        DeadShaman5.enabled = false;
        DeadShaman6.enabled = false;
        DeadShaman7.enabled = false;
        DeadShaman8.enabled = false;
        DeadShaman9.enabled = false;
        DeadShaman10.enabled = false;
        DeadShaman11.enabled = false;
        DeadShaman12.enabled = false;
        DeadShaman13.enabled = false;



    }

    public void Ani()
    {
        DeadShaman.enabled = true;
        DeadShaman1.enabled = true;
        DeadShaman2.enabled = true;
        DeadShaman3.enabled = true;
        DeadShaman4.enabled = true;
        DeadShaman5.enabled = true;
        DeadShaman6.enabled = true;
        DeadShaman7.enabled = true;
        DeadShaman8.enabled = true;
        DeadShaman9.enabled = true;
        DeadShaman10.enabled = true;
        DeadShaman11.enabled = true;
        DeadShaman12.enabled = true;
        DeadShaman13.enabled = true;
    }
}
