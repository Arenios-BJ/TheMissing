using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMenu : MonoBehaviour {

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ExitMenuButton(bool isExit)
    {
        if (isExit) // 플레이하던 게임을 종료하고 처음 화면으로 돌아감.
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }
        else    // 게임 계속 진행.    
        {
            if (GameManager.gm.isPlayerMoved) // 기존에 timeScale이 1f 이었을 때.
                Time.timeScale = 1f;

            gameObject.SetActive(false);
            GameManager.gm.panelOpen = false;

            if (GameManager.gm.isMouseLocked) // 기존에 마우스 상태가 lock이었을 때.
                GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(true);
        }
    }
}
