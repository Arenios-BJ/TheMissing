using UnityEngine;

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
