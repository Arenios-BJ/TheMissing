using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour {

    public Image But;
    public GameObject loding;
    public AudioSource StartBut;
    public GameObject renew;

    void Start()
    {
        Time.timeScale = 1f;
        loding.SetActive(false);
        renew.SetActive(false);
        StartCoroutine("StartButton");
        StartBut.enabled = false;
    }

    // 'Start' 버튼을 눌렀을 때.
    public void StartGame()
    {
        StartBut.enabled = true;
		int idx = PlayerPrefs.GetInt("Chapter");
        if (idx == 0 || idx == 1)
        {
            loding.SetActive(true);
            GameManager.gm.LoadSavedScene();
        }
        else
        {
            renew.SetActive(true);
        }
    }

    // 기존의 데이터가 존재하여 'RenewPanel'의 버튼을 눌렀을 때.
    public void RenewButton(bool isLoad)
    {
        if (isLoad) // 기존의 플레이 데이터를 불러올 때.
        {
            int idx = PlayerPrefs.GetInt("Chapter");
            loding.SetActive(true);
            GameManager.gm.LoadSavedScene(idx);
        }
        else    // 새로이 게임을 시작할 때.
        {
            loding.SetActive(true);
            GameManager.gm.LoadSavedScene();
        }
    }

    // 'Start' 버튼 깜박임 효과.
    IEnumerator StartButton()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        bool alpha = true;
        while (true)
        {
            if (alpha)
                But.color = new Color32(255, 255, 255, 255);
            else
                But.color = new Color32(255, 255, 255, 0);

            alpha = !alpha;
            yield return wait;
        }
    }
}
