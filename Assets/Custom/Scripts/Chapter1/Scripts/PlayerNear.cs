using UnityEngine;

// 첫 번째 오두막에서 나가는 문을 부술 때 플레이어는 특정 위치 안에 들어왔을 때만 문과의 상호작용이 가능하다.
// 그렇게 하지 않으면 저~ 멀리에서 문을 클릭해도 문이 부서지기 때문에 만들었음
// 사용한 방법 : bool / FindWithTag / OnTriggerEnter / OnTriggerExit

public class PlayerNear : MonoBehaviour {

    private Attack_Ani PlayerAxe;
    public bool near;

	void Start () {

        PlayerAxe = GameObject.FindWithTag("Player").GetComponent<Attack_Ani>();
        near = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Player")
        {
            if(PlayerAxe.AxeCheck == true)
            {
                near = true;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {

        near = false;
        
    }
}
