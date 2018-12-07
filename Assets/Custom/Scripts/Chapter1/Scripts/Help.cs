using UnityEngine;
using UnityEngine.UI;

// 도움말 스크립트 (첫 번째 오두막에서만 뜬다)
// 사용한 방법 : Text / SetActive / RaycastHit / CompareTag / activeSelf / GetKeyUp

public class Help : MonoBehaviour {

    public GameObject HelpCanvas;

    public Text HelpText;

    private FirstPersonCamera player;

    private GameObject hint1;
    private GameObject inven;

    void Start () {

        HelpCanvas.SetActive(false);

        player = FirstPersonCamera.player;

        hint1 = GameObject.Find("Hint");

        inven = GameObject.Find("Inventory");

    }

    void Update()
    {

        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (GameManager.gm.panelOpen == false)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // 아이템을 먹었으면 I를 눌러 확인이라는 도움말이 뜬다.
                    if (hit.transform.CompareTag("Item"))
                    {
                        HelpText.text = "I 를 눌러 확인";
                        HelpCanvas.SetActive(true);
                    }

                }
            }
        }

        ActivSelfCheck();

    }

    // UI 활성화 체크
    private void ActivSelfCheck()
    {

        // hint1(퍼즐 그림 원본)이 켜져있는 상태라면
        if (hint1.GetComponent<Hint>().hint1.activeSelf == true || hint1.GetComponent<Hint>().hint2.activeSelf == true)
        {
            HelpText.text = "Tab을 눌러 닫음";
            HelpCanvas.SetActive(true);
        }
        
        // 인벤토리가 켜져있는 상태라면
        if (inven.GetComponent<Inventory>().IsOpened == true)
        {
            if (GameManager.gm.panelOpen == false)
            {
                if (Input.GetKeyUp(KeyCode.I))
                {
                    HelpText.text = "I 또는 Tab키를 눌러 닫음";
                    HelpCanvas.SetActive(true);

                }
            }
        }
        else if(inven.GetComponent<Inventory>().IsOpened == false)
        {
            if (GameManager.gm.panelOpen == false)
            {
                if (Input.GetKeyUp(KeyCode.I))
                {
                    HelpCanvas.SetActive(false);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (GameManager.gm.panelOpen == false)
            {
                HelpCanvas.SetActive(false);
            }
        }
    }
}
