using UnityEngine;

// 플레이어가 보스의 집안에 있는지 확인하기 위한 스크립트
// 집안에 들어왔을 때, 특정 아이템들을 먹지 못하면 밖으로 나갈 수 없음
// 사용한 방법 : bool / SetActive / OnTriggerEnter / Destroy

public class INCheck : MonoBehaviour {

    private bool PlayerIn;

    public GameObject OutCheck;

    public GameObject ScriptPoint;

    public GameObject backTrigger;

    void Start () {

        PlayerIn = false;
        
        OutCheck.SetActive(false);
        ScriptPoint.SetActive(false);
    }


    void OnTriggerEnter(Collider col)
    {
        if(PlayerIn == false)
        {
            if (col.gameObject.name == "Player")
            {
                OutCheck.SetActive(true);
                ScriptPoint.SetActive(true);
                Destroy(backTrigger);
                PlayerIn = true;
            }
        }
    }
}
