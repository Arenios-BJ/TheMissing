using UnityEngine;

public class ItemFunctions {
    public void ItemFunction(string itemName)
    {
        switch (itemName)
        {
            case "손전등":
                FirstPersonCamera.player.batteryImage.transform.parent.gameObject.SetActive(true);
                FirstPersonCamera.player.m_bHasFlashlight = true;
                if (FirstPersonCamera.player.m_fCur_Battery == 0f)
                {
                    if (GameObject.Find("Help"))
                    {
                        Help a = GameObject.Find("Help").GetComponent<Help>();
                        a.HelpText.text = "배터리가 없습니다. 배터리를 사용해주세요.";
                        a.HelpCanvas.SetActive(true);
                    }
                }
                break;
            case "도끼":
                //도끼활성화
                GameObject.FindWithTag("Player").GetComponent<Attack_Ani>().AxeCheck = true;
                break;
            case "건전지":
                if (FirstPersonCamera.player.m_bHasFlashlight == true)
                {
                    FirstPersonCamera.player.m_fCur_Battery = 1f;
                    FirstPersonCamera.player.batteryImage.fillAmount = 1;

                    if (GameObject.Find("Help"))
                    {
                        GameObject.Find("Help").GetComponent<Help>().HelpText.text = "F키를 눌러 사용";
                        GameObject.Find("Help").GetComponent<Help>().HelpCanvas.SetActive(true);
                    }
                }
                break;
            case "돌":
                GameObject.Find("GameManager_Ch3").GetComponent<GameManager_Ch3>().stone_button = true;
                break;
            case "나침반":
                GameObject.FindWithTag("Player").GetComponent<Arms>().CompassUseCheck = true;
                if (GameObject.Find("Help"))
                {
                    GameObject.Find("Help").GetComponent<Help>().HelpText.text = "R키를 눌러 사용";
                    GameObject.Find("Help").GetComponent<Help>().HelpCanvas.SetActive(true);
                }
                break;
            case "지도":
                GameObject.Find("Canvas").transform.Find("Img_Map").gameObject.SetActive(true);
                Time.timeScale = 0.0f;
                break;
            case "단서":
                GameObject.Find("Inventory").GetComponent<Inventory>().ImgHint.SetActive(true);
                Time.timeScale = 0.0f;
                break;
            default:
                break;
        }
    }
}
