using UnityEngine;

// 게임을 시작할때 UI가 띄워져있지 않도록 하기 위함
// 사용한 방법 : SetActive

public class Puz_Canvas_Manager : MonoBehaviour {

    public GameObject Puzcanvas;

    void Start () {
        Puzcanvas.SetActive(false);
    }
	
}
