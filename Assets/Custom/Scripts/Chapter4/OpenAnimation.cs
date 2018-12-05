using UnityEngine;

// 플레이어가 서랍 같은 것을 선택했을 때, 열리는 애니메이션을 관리하는 스크립트

public class OpenAnimation : MonoBehaviour {

    private FirstPersonCamera player;
    private Animator TVLeftdoor;
    private Animator TVRightdoor;

    private Animator Kitchendoor1;
    private Animator Kitchendoor2;
    private Animator Kitchendoor3;
    private Animator Kitchendoor4;
    private Animator Kitchendoor5;

    private Animator Kitchendrawer1;
    private Animator Kitchendrawer2;
    private Animator Kitchendrawer3;
    private Animator Kitchendrawer4;

    private Animator PantryL;
    private Animator PantryR;

    private Animator BathroomDoorL;
    private Animator BathroomDoorR;

    private Animator Bedroomdarw1;
    private Animator Bedroomdarw2;
    private Animator Bedroomdarw3;
    private Animator Bedroomdarw4;
    private Animator Bedroomdarw5;
    private Animator Bedroomdarw6;
    private Animator Bedroomdarw7;

    private Animator BedroomDoorL;
    private Animator BedroomDoorR;

    private Animator TableDoorL;
    private Animator TableDoorR;

    // 싱크대 작은 서랍이 열려있니?
    private bool KitchenDrawerCheck1;
    private bool KitchenDrawerCheck2;
    private bool KitchenDrawerCheck3;
    private bool KitchenDrawerCheck4;

    // 싱크대 큰 서랍이 열려있니?
    private bool KitchenDoorCheck1;
    private bool KitchenDoorCheck2;
    private bool KitchenDoorCheck3;
    private bool KitchenDoorCheck4;
    private bool KitchenDoorCheck5;

    // 침대 방 서랍이 열려있니?
    private bool BedroomDrawerCheck1;
    private bool BedroomDrawerCheck2;
    private bool BedroomDrawerCheck3;
    private bool BedroomDrawerCheck4;
    private bool BedroomDrawerCheck5;
    private bool BedroomDrawerCheck6;
    private bool BedroomDrawerCheck7;



    void Start () {

        player = FirstPersonCamera.player;
        TVLeftdoor = GameObject.Find("tvStandDoor_L").GetComponent<Animator>();
        TVRightdoor = GameObject.Find("tvStandDoor_R").GetComponent<Animator>();

        Kitchendoor1 = GameObject.Find("door01").GetComponent<Animator>();
        Kitchendoor2 = GameObject.Find("door02").GetComponent<Animator>();
        Kitchendoor3 = GameObject.Find("door03").GetComponent<Animator>();
        Kitchendoor4 = GameObject.Find("door04").GetComponent<Animator>();
        Kitchendoor5 = GameObject.Find("door05").GetComponent<Animator>();

        Kitchendrawer1 = GameObject.Find("kitchendraw01").GetComponent<Animator>();
        Kitchendrawer2 = GameObject.Find("kitchendraw02").GetComponent<Animator>();
        Kitchendrawer3 = GameObject.Find("kitchendraw03").GetComponent<Animator>();
        Kitchendrawer4 = GameObject.Find("kitchendraw04").GetComponent<Animator>();

        PantryL = GameObject.Find("PantryL").GetComponent<Animator>();
        PantryR = GameObject.Find("PantryR").GetComponent<Animator>();

        BathroomDoorL = GameObject.Find("BathroomdoorL").GetComponent<Animator>();
        BathroomDoorR = GameObject.Find("BathroomdoorR").GetComponent<Animator>();

        Bedroomdarw1 = GameObject.Find("Beddraw01").GetComponent<Animator>();
        Bedroomdarw2 = GameObject.Find("Beddraw02").GetComponent<Animator>();
        Bedroomdarw3 = GameObject.Find("Beddraw03").GetComponent<Animator>();
        Bedroomdarw4 = GameObject.Find("Beddraw04").GetComponent<Animator>();
        Bedroomdarw5 = GameObject.Find("Beddraw05").GetComponent<Animator>();
        Bedroomdarw6 = GameObject.Find("Beddraw06").GetComponent<Animator>();
        Bedroomdarw7 = GameObject.Find("Beddraw07").GetComponent<Animator>();

        BedroomDoorL = GameObject.Find("BedroomDoorL").GetComponent<Animator>();
        BedroomDoorR = GameObject.Find("BedroomDoorR").GetComponent<Animator>();

        TableDoorL = GameObject.Find("Tabledraw1").GetComponent<Animator>();
        TableDoorR = GameObject.Find("Tabledraw2").GetComponent<Animator>();


        // TV아래 서랍 애니메이션
        TVLeftdoor.enabled = false;
        TVRightdoor.enabled = false;

        // 싱크대 큰 서랍 애니메이션
        Kitchendoor1.enabled = false;
        Kitchendoor2.enabled = false;
        Kitchendoor3.enabled = false;
        Kitchendoor4.enabled = false;
        Kitchendoor5.enabled = false;

        // 싱크대 작은 서랍 애니메이션
        Kitchendrawer1.enabled = false;
        Kitchendrawer2.enabled = false;
        Kitchendrawer3.enabled = false;
        Kitchendrawer4.enabled = false;

        // 붙박이장 애니메이션
        PantryL.enabled = false;
        PantryR.enabled = false;

        // 화장실 서랍 애니메이션
        BathroomDoorL.enabled = false;
        BathroomDoorR.enabled = false;

        // 침대방 서랍 애니메이션
        Bedroomdarw1.enabled = false;
        Bedroomdarw2.enabled = false;
        Bedroomdarw3.enabled = false;
        Bedroomdarw4.enabled = false;
        Bedroomdarw5.enabled = false;
        Bedroomdarw6.enabled = false;
        Bedroomdarw7.enabled = false;

        // 옷장 애니메이션
        BedroomDoorL.enabled = false;
        BedroomDoorR.enabled = false;

        TableDoorL.enabled = false;
        TableDoorR.enabled = false;

        KitchenDrawerCheck1 = false;
        KitchenDrawerCheck2 = false;
        KitchenDrawerCheck3 = false;
        KitchenDrawerCheck4 = false;

        KitchenDoorCheck1 = false;
        KitchenDoorCheck2 = false;
        KitchenDoorCheck3 = false;
        KitchenDoorCheck4 = false;
        KitchenDoorCheck5 = false;

        BedroomDrawerCheck1 = false;
        BedroomDrawerCheck2 = false;
        BedroomDrawerCheck3 = false;
        BedroomDrawerCheck4 = false;
        BedroomDrawerCheck5 = false;
        BedroomDrawerCheck6 = false;
        BedroomDrawerCheck7 = false;
    }
	
	void Update () {

        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    // TV아래 서랍
                    if (hit.transform.name == "tvStandDoor_L")   // 
                    {
                        TVLeftdoor.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }

                    if (hit.transform.name == "tvStandDoor_R")   // 
                    {
                        TVRightdoor.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }

                    // 싱크대 큰 서랍 
                    if (hit.transform.name == "door01")   // 
                    {
                        if (KitchenDoorCheck1 == false)
                        {
                            Kitchendoor1.enabled = true;
                            Kitchendoor1.SetBool("KitchenDoor_Open1", false);
                            KitchenDoorCheck1 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (KitchenDoorCheck1 == true)
                        {
                            Kitchendoor1.SetBool("KitchenDoor_Open1", true);
                            KitchenDoorCheck1 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "door02")   // 
                    {
                        if (KitchenDoorCheck2 == false)
                        {
                            Kitchendoor2.enabled = true;
                            Kitchendoor2.SetBool("KitchenDoor_Open2", false);
                            KitchenDoorCheck2 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (KitchenDoorCheck2 == true)
                        {
                            Kitchendoor2.SetBool("KitchenDoor_Open2", true);
                            KitchenDoorCheck2 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "door03")   // 
                    {
                        if (KitchenDoorCheck3 == false)
                        {
                            Kitchendoor3.enabled = true;
                            Kitchendoor3.SetBool("KitchenDoor_Open3", false);
                            KitchenDoorCheck3 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (KitchenDoorCheck3 == true)
                        {
                            Kitchendoor3.SetBool("KitchenDoor_Open3", true);
                            KitchenDoorCheck3 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "door04")   // 
                    {
                        if (KitchenDoorCheck4 == false)
                        {
                            Kitchendoor4.enabled = true;
                            Kitchendoor4.SetBool("KitchenDoor_Open4", false);
                            KitchenDoorCheck4 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (KitchenDoorCheck4 == true)
                        {
                            Kitchendoor4.SetBool("KitchenDoor_Open4", true);
                            KitchenDoorCheck4 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "door05")   // 
                    {
                        if (KitchenDoorCheck5 == false)
                        {
                            Kitchendoor5.enabled = true;
                            Kitchendoor5.SetBool("KitchenDoor_Open5", false);
                            KitchenDoorCheck5 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (KitchenDoorCheck5 == true)
                        {
                            Kitchendoor5.SetBool("KitchenDoor_Open5", true);
                            KitchenDoorCheck5 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    // 싱크대 작은 서랍

                    if (hit.transform.name == "kitchendraw01")   // 
                    {
                        if (KitchenDrawerCheck1 == false)
                        {
                            Kitchendrawer1.enabled = true;
                            Kitchendrawer1.SetBool("Kitchendrawer_Open", false);
                            KitchenDrawerCheck1 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }

                        else if (KitchenDrawerCheck1 == true)
                        {
                            Kitchendrawer1.SetBool("Kitchendrawer_Open", true);
                            KitchenDrawerCheck1 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "kitchendraw02")   // 
                    {
                        if (KitchenDrawerCheck2 == false)
                        {
                            Kitchendrawer2.enabled = true;
                            Kitchendrawer2.SetBool("Kitchendrawer_Open2", false);
                            KitchenDrawerCheck2 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }

                        else if (KitchenDrawerCheck2 == true)
                        {
                            Kitchendrawer2.SetBool("Kitchendrawer_Open2", true);
                            KitchenDrawerCheck2 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "kitchendraw03")   // 
                    {
                        if (KitchenDrawerCheck3 == false)
                        {
                            Kitchendrawer3.enabled = true;
                            Kitchendrawer3.SetBool("Kitchendrawer_Open3", false);
                            KitchenDrawerCheck3 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }

                        else if (KitchenDrawerCheck3 == true)
                        {
                            Kitchendrawer3.SetBool("Kitchendrawer_Open3", true);
                            KitchenDrawerCheck3 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "kitchendraw04")   // 
                    {
                        if (KitchenDrawerCheck4 == false)
                        {
                            Kitchendrawer4.enabled = true;
                            Kitchendrawer4.SetBool("Kitchendrawer_Open4", false);
                            KitchenDrawerCheck4 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }

                        else if (KitchenDrawerCheck4 == true)
                        {
                            Kitchendrawer4.SetBool("Kitchendrawer_Open4", true);
                            KitchenDrawerCheck4 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "PantryL")   // 
                    {
                        PantryL.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }

                    if (hit.transform.name == "PantryR")   // 
                    {
                        PantryR.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }

                    if (hit.transform.name == "BathroomdoorL")   // 
                    {
                        BathroomDoorL.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }

                    if (hit.transform.name == "BathroomdoorR")   // 
                    {
                        BathroomDoorR.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }

                    if (hit.transform.name == "Beddraw01")   // 
                    {
                        if (BedroomDrawerCheck1 == false)
                        {
                            Bedroomdarw1.enabled = true;
                            Bedroomdarw1.SetBool("BedroomDrawer_Open1", false);
                            BedroomDrawerCheck1 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (BedroomDrawerCheck1 == true)
                        {
                            Bedroomdarw1.SetBool("BedroomDrawer_Open1", true);
                            BedroomDrawerCheck1 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "Beddraw02")   // 
                    {
                        if (BedroomDrawerCheck2 == false)
                        {
                            Bedroomdarw2.enabled = true;
                            Bedroomdarw2.SetBool("BedroomDrawer_Open2", false);
                            BedroomDrawerCheck2 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (BedroomDrawerCheck2 == true)
                        {
                            Bedroomdarw2.SetBool("BedroomDrawer_Open2", true);
                            BedroomDrawerCheck2 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "Beddraw03")   // 
                    {
                        if (BedroomDrawerCheck3 == false)
                        {
                            Bedroomdarw3.enabled = true;
                            Bedroomdarw3.SetBool("BedroomDrawer_Open3", false);
                            BedroomDrawerCheck3 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (BedroomDrawerCheck3 == true)
                        {
                            Bedroomdarw3.SetBool("BedroomDrawer_Open3", true);
                            BedroomDrawerCheck3 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "Beddraw04")   // 
                    {
                        if (BedroomDrawerCheck4 == false)
                        {
                            Bedroomdarw4.enabled = true;
                            Bedroomdarw4.SetBool("BedroomDrawer_Open4", false);
                            BedroomDrawerCheck4 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (BedroomDrawerCheck4 == true)
                        {
                            Bedroomdarw4.SetBool("BedroomDrawer_Open4", true);
                            BedroomDrawerCheck4 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "Beddraw05")   // 
                    {
                        if (BedroomDrawerCheck5 == false)
                        {
                            Bedroomdarw5.enabled = true;
                            Bedroomdarw5.SetBool("BedroomDrawer_Open5", false);
                            BedroomDrawerCheck5 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (BedroomDrawerCheck5 == true)
                        {
                            Bedroomdarw5.SetBool("BedroomDrawer_Open5", true);
                            BedroomDrawerCheck5 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "Beddraw06")   // 
                    {
                        if (BedroomDrawerCheck6 == false)
                        {
                            Bedroomdarw6.enabled = true;
                            Bedroomdarw6.SetBool("BedroomDrawer_Open6", false);
                            BedroomDrawerCheck6 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (BedroomDrawerCheck6 == true)
                        {
                            Bedroomdarw6.SetBool("BedroomDrawer_Open6", true);
                            BedroomDrawerCheck6 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "Beddraw07")   // 
                    {
                        if (BedroomDrawerCheck7 == false)
                        {
                            Bedroomdarw7.enabled = true;
                            Bedroomdarw7.SetBool("BedroomDrawer_Open7", false);
                            BedroomDrawerCheck7 = true;
                            SoundManager.instance.SelectSound(hit.transform.name);
                        }
                        else if (BedroomDrawerCheck7 == true)
                        {
                            Bedroomdarw7.SetBool("BedroomDrawer_Open7", true);
                            BedroomDrawerCheck7 = false;
                            SoundManager.instance.DrawerCloseS();
                        }
                    }

                    if (hit.transform.name == "BedroomDoorL")   // 
                    {
                        BedroomDoorL.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }

                    if (hit.transform.name == "BedroomDoorR")   // 
                    {
                        BedroomDoorR.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }

                    if (hit.transform.name == "Tabledraw1")   // 
                    {
                        TableDoorL.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }

                    if (hit.transform.name == "Tabledraw2")   // 
                    {
                        TableDoorR.enabled = true;
                        SoundManager.instance.SelectSound(hit.transform.name);
                    }
                }
            }
        }
    }
}
