using UnityEngine;

// 첫 번째 오두막에서 퍼즐 캔버스 -> 닫기를 누르면 캔버스가 꺼진다.
// 사용한 방법 : Button / Find / Time.timeScale

public class Close_Puz : MonoBehaviour
{

    private MouseLock mouse;

    void Start()
    {
        mouse = GameManager.gm.GetComponent<MouseLock>();
    }

    public void OnClick()
    {
        GameObject.Find("Puzzle").GetComponent<Puz_Canvas_Manager>().Puzcanvas.SetActive(false);
        Time.timeScale = 1.0f;
        mouse.ChangeMouseLock(true);
    }
}
