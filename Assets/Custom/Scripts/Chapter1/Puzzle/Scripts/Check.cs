using UnityEngine;

public class Check : MonoBehaviour
{
    private GameObject OnePanel;
    private GameObject TwoPanel;
    private GameObject ThreePanel;
    private GameObject FourPanel;
    private GameObject FivePanel;
    private GameObject SixPanel;
    private GameObject SevenPanel;
    private GameObject EightPanel;
    private GameObject NinePanel;
    private GameObject TenPanel;

    bool check = false;
    bool check2 = false;
    bool check3 = false;
    bool check4 = false;
    bool check5 = false;
    bool check6 = false;
    bool check7 = false;
    bool check8 = false;
    bool check9 = false;
    bool check10 = false;

    public GameObject canvas;

    private MouseLock mouse;

    void Start()
    {
        OnePanel = GameObject.Find("OnePanel");
        TwoPanel = GameObject.Find("TwoPanel");
        ThreePanel = GameObject.Find("ThreePanel");
        FourPanel = GameObject.Find("FourPanel");
        FivePanel = GameObject.Find("FivePanel");
        SixPanel = GameObject.Find("SixPanel");
        SevenPanel = GameObject.Find("SevenPanel");
        EightPanel = GameObject.Find("EightPanel");
        NinePanel = GameObject.Find("NinePanel");
        TenPanel = GameObject.Find("TenPanel");

        mouse = GameManager.gm.GetComponent<MouseLock>();
    }

    void C_Check()
    {
        if (OnePanel.transform.childCount == 0)
        {
            check = false;
        }
        else if (OnePanel.transform.childCount == 1)
        {
            if (OnePanel.transform.GetChild(0).name == "One")
            {
                check = true;
            }
        }

        if (TwoPanel.transform.childCount == 0)
        {
            check2 = false;
        }
        else if (TwoPanel.transform.childCount == 1)
        {
            if (TwoPanel.transform.GetChild(0).name == "Three")
            {
                check2 = true;
            }
        }

        if (ThreePanel.transform.childCount == 0)
        {
            check3 = false;
        }
        else if (ThreePanel.transform.childCount == 1)
        {
            if (ThreePanel.transform.GetChild(0).name == "Five")
            {
                check3 = true;
            }
        }

        if (FourPanel.transform.childCount == 0)
        {
            check4 = false;
        }
        else if (FourPanel.transform.childCount == 1)
        {
            if (FourPanel.transform.GetChild(0).name == "Seven")
            {
                check4 = true;
            }
        }

        if (FivePanel.transform.childCount == 0)
        {
            check5 = false;
        }
        else if (FivePanel.transform.childCount == 1)
        {
            if (FivePanel.transform.GetChild(0).name == "Nine")
            {
                check5 = true;
            }
        }

        if (SixPanel.transform.childCount == 0)
        {
            check6 = false;
        }
        else if (SixPanel.transform.childCount == 1)
        {
            if (SixPanel.transform.GetChild(0).name == "Ten")
            {
                check6 = true;
            }
        }

        if (SevenPanel.transform.childCount == 0)
        {
            check7 = false;
        }
        else if (SevenPanel.transform.childCount == 1)
        {
            if (SevenPanel.transform.GetChild(0).name == "Eight")
            {
                check7 = true;
            }
        }

        if (EightPanel.transform.childCount == 0)
        {
            check8 = false;
        }
        else if (EightPanel.transform.childCount == 1)
        {
            if (EightPanel.transform.GetChild(0).name == "Six")
            {
                check8 = true;
            }
        }

        if (NinePanel.transform.childCount == 0)
        {
            check9 = false;
        }
        else if (NinePanel.transform.childCount == 1)
        {
            if (NinePanel.transform.GetChild(0).name == "Four")
            {
                check9 = true;
            }
        }

        if (TenPanel.transform.childCount == 0)
        {
            check10 = false;
        }
        else if (TenPanel.transform.childCount == 1)
        {
            if (TenPanel.transform.GetChild(0).name == "Two")
            {
                check10 = true;
            }
        }
    }

    void Update()
    {
        C_Check();

        if (check == true && check2 == true && check3 == true && check4 == true && check5 == true && check6 == true && check7 == true && check8 == true && check9 == true && check10 == true)
        {
            Destroy(canvas);
            Time.timeScale = 1.0f;
            mouse.ChangeMouseLock(true);
        }
    }
}
