using UnityEngine;

public class Attack_Axe_Manager : MonoBehaviour {

    // 내가 손에 쥐고 공격할 도끼의 이름
    public GameObject Attack_Axe;

    // Use this for initialization
    void Start () {
        Attack_Axe.SetActive(false);
    }
}
