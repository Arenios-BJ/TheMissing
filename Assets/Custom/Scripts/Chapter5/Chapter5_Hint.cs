using UnityEngine;

public class Chapter5_Hint : MonoBehaviour {

    public GameObject HintCanvas1;
    public GameObject HintCanvas2;
    public GameObject HintCanvas3;
    public GameObject HintCanvas4;

    private FirstPersonCamera player;

    void Start () {

        HintCanvas1.SetActive(false);
        HintCanvas2.SetActive(false);
        HintCanvas3.SetActive(false);
        HintCanvas4.SetActive(false);

        player = FirstPersonCamera.player;

    }
	
	void Update () {

        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    switch (hit.transform.name)
                    {
                        case "S_Paper (22)":
                            SoundManager.instance.SelectSound(hit.transform.name);
                            HintCanvas1.SetActive(true);
                            Time.timeScale = 0.0f;
                            break;
                        case "S_Paper (19)":
                            SoundManager.instance.SelectSound(hit.transform.name);
                            HintCanvas2.SetActive(true);
                            Time.timeScale = 0.0f;
                            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
                            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "근처에서 본 것 같아";
                            break;
                        case "S_Paper (20)":
                            SoundManager.instance.SelectSound(hit.transform.name);
                            HintCanvas3.SetActive(true);
                            Time.timeScale = 0.0f;
                            break;
                        case "S_Paper (21)":
                            SoundManager.instance.SelectSound(hit.transform.name);
                            HintCanvas4.SetActive(true);
                            Time.timeScale = 0.0f;
                            break;

                    }
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.gm.panelOpen == false)
            {
                HintCanvas1.SetActive(false);
                HintCanvas2.SetActive(false);
                HintCanvas3.SetActive(false);
                HintCanvas4.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }

    }
}
