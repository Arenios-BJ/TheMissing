using UnityEngine;

public class SoundManager : MonoBehaviour {

    public Safe safe;
    public Picture framemove;
    public Attack_Ani broken;

    private FirstPersonCamera player;

    public AudioSource Audio;

    public AudioClip Flash_Switch_s;
    public AudioClip Blue_Key_s;
    public AudioClip Message_s;
    public AudioClip Circle_s;
    public AudioClip Folder_s;
    public AudioClip Can_s;
    public AudioClip Special_Item_s;
    public AudioClip Paper_Touch_s;
    public AudioClip Cup_Touch_s;
    public AudioClip Bottle_Touch_s;
    public AudioClip Smoke_Touch_s;
    public AudioClip Key_Lock_s;
    public AudioClip Trash_Touch_s;
    public AudioClip Radio_Touch_s;
    public AudioClip Lighter_Touch_s;
    public AudioClip ChainDoor_Touch_s;
    public AudioClip OpenDoor_w_s;
    public AudioClip ChainDoor_Open_s;
    public AudioClip Frame_Move_s;
    public AudioClip Broken_Door_s;
    public AudioClip Button_Click_s;
    public AudioClip Puzzle_Select_s;
    public AudioClip Stone_BeShot_s;
    public AudioClip Pig_BeShot_s;
    public AudioClip Pig_Run_s;
    public AudioClip Ch2_Close_Door_s;
    public AudioClip Ch2_Window_Close_s;
    public AudioClip Dodg_Fail_s;
    public AudioClip Mouse_Click_s;
    public AudioClip KeyBoard_Touch_s;
    public AudioClip Eat_s;
    public AudioClip Glass_s;
    public AudioClip Shake_s;
    public AudioClip Paper_s;
    public AudioClip Dishes_s;
    public AudioClip Drawer_Open_s;
    public AudioClip Drawer_Close_s;
    public AudioClip Drawer_Door_s;
    public AudioClip Open_Mis_Image_s;
    public AudioClip Stove_s;
    public AudioClip Fridge_s;
    public AudioClip Knife_s;
    public AudioClip Button_s;
    public AudioClip OpenSafe_s;
    public AudioClip Shoes_s;
    public AudioClip OpenWindow_s;
    public AudioClip Brick_s;
    public AudioClip Stone_s;
    public AudioClip EMF_s;
    public AudioClip TV_s;
    public AudioClip Beam_s;
    public AudioClip RockHit_s;
    public AudioClip Fire_s;
    public AudioClip Ch5_Rock_s;
    public AudioClip Power_s;
    public AudioClip Pull_s;
    public AudioClip Inven_s;

    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Audio = GetComponent<AudioSource>();
        player = FirstPersonCamera.player;

        if (safe)
        {
            safe = GameObject.Find("Button").GetComponent<Safe>();
        }
        if (framemove)
        {
            framemove = GameObject.Find("Puzzle").GetComponent<Picture>();
        }
        if (broken)
        {
            broken = GameObject.Find("Player").GetComponent<Attack_Ani>();
        }
        
    }

    private void Update()
    {
        if (safe)
        {
            if (safe.check1 == true && safe.check2 == true && safe.check3 == true && safe.check4 == true && safe.Okcheck == true)
            {
                Audio.PlayOneShot(ChainDoor_Open_s);
            }
        }
    }

    public void Flash_Button()
    {
        Audio.PlayOneShot(Flash_Switch_s);
    }

    public void Chess_Move()
    {
        Audio.PlayOneShot(Circle_s);
    }

    public void Move_Pannel()
    {
        Audio.PlayOneShot(Puzzle_Select_s);
    }

    public void FrameMove()
    {
        Audio.PlayOneShot(Frame_Move_s);
    }

    public void Button_Click()
    {
        Audio.PlayOneShot(Button_Click_s);
    }

    public void ChainDoor_Open()
    {
        Audio.PlayOneShot(ChainDoor_Open_s);
    }

    public void Broken_Door()
    {
        Audio.PlayOneShot(Broken_Door_s);
    }

    public void Dodg_Fail()
    {
        Audio.PlayOneShot(Dodg_Fail_s);
    }

    public void Mouse_Click()
    {
        Audio.PlayOneShot(Mouse_Click_s);
    }

    public void KeyBoard_Touch()
    {
        Audio.PlayOneShot(KeyBoard_Touch_s);
    }

    public void Paper_Touch()
    {
        Audio.PlayOneShot(Paper_Touch_s);
    }

    public void Stone_Shot()
    {
        Audio.PlayOneShot(Stone_BeShot_s);
    }

    public void Pig_BeShot()
    {
        Audio.PlayOneShot(Pig_BeShot_s);
    }

    public void Pig_Run()
    {
        Audio.PlayOneShot(Pig_Run_s);
    }

    public void ColorS()
    {
        Audio.PlayOneShot(OpenSafe_s);
    }

    public void BrickS()
    {
        Audio.PlayOneShot(Brick_s);
    }

    public void StoneS()
    {
        Audio.PlayOneShot(Stone_s);
    }

    public void DrawerCloseS()
    {
        Audio.PlayOneShot(Drawer_Close_s);
    }

    public void MisImageS()
    {
        Audio.PlayOneShot(Open_Mis_Image_s);
    }

    public void Ch5RockS()
    {
        Audio.PlayOneShot(Ch5_Rock_s);
    }

    public void RockS()
    {
        Audio.PlayOneShot(RockHit_s);
    }

    public void InvenS()
    {
        Audio.PlayOneShot(Inven_s);
    }

    public void SelectSound(string name)
    {
        switch(name)
        {
            case "Item_PowerKey":
            case "Item_CarKey":
            case "Item_BlueKey":
                Audio.PlayOneShot(Blue_Key_s);
                break;
            case "Item_Hint":
            case "Item_Battery":
            case "Item_Axe":
            case "Item_Flashlight":
            case "Item_Compass":
            case "Item_Stone":
                Audio.PlayOneShot(Message_s);
                break;
            case "Item_CircleElectric":
            case "Item_CircleSub":
            case "Item_CircleBoss":
            case "Item_CircleCCTV":
                Audio.PlayOneShot(Circle_s);
                break;
            case "folder":
            case "book_0001a (1)":
            case "book_0001b (1)":
            case "BrownBook":
            case "Police_Pinboard":
            case "DoctorBook":
                Audio.PlayOneShot(Folder_s);
                break;
            case "S_Paper6":
            case "stone_totem (1)":
            case "stone_totem (2)":
            case "stone_totem (3)":
            case "stone_totem (4)":
            case "stone_totem (5)":
            case "stone_totem (6)":
            case "stone_totem (7)":
            case "colorfull":
            case "card":
                Audio.PlayOneShot(Special_Item_s);
                break;
            case "S_Paper":
            case "S_Paper (7)":
            case "Item_Map":
                Audio.PlayOneShot(Paper_Touch_s);
                break;
            case "Can_3 (1)":
            case "Bottle3":
            case "Topor_lod0":
                Audio.PlayOneShot(Bottle_Touch_s);
                break;
            case "Cigarette (3)":
            case "pPlane3":
                Audio.PlayOneShot(Smoke_Touch_s);
                break;
            case "old_wooden_door":
                if (!GameObject.Find("Inventory").GetComponent<Inventory>().BlueKey)
                {
                    Audio.PlayOneShot(Key_Lock_s);
                }
                else
                {
                    Audio.PlayOneShot(OpenDoor_w_s);
                }
                break;
            case "jail_door":
                if (!player.gameObject.GetComponent<Attack_Ani>().AxeCheck)
                {
                    Audio.PlayOneShot(Key_Lock_s);
                }
                else
                {

                }
                break;
            case "kos":
            case "binbag 8":
                Audio.PlayOneShot(Trash_Touch_s);
                break;
            case "radio_low_poly (2)":
                Audio.PlayOneShot(Radio_Touch_s);
                break;
            case "Lighter":
                Audio.PlayOneShot(Lighter_Touch_s);
                break;
            case "DoorLeftGrp":
            case "DoorRightGrp":
            case "door":
                Audio.PlayOneShot(ChainDoor_Touch_s);
                break;
            case "S_Paper8":
                Audio.PlayOneShot(Special_Item_s);
                break;
            case "bread_01":
                Audio.PlayOneShot(Eat_s);
                break;
            case "TeapotBase_LOD0":
            case "Teacup_LOD0":
            case "Mug_LOD0":
            case "kitchendraw02":
                Audio.PlayOneShot(Glass_s);
                break;
            case "chukies_02":
                Audio.PlayOneShot(Shake_s);
                break;
            case "S_Paper (17)":
            case "S_Paper (16)":
            case "S_Paper (18)":
            case "S_Paper1818":
            case "S_Paper (13)":
            case "S_Paper (14)":
            case "S_Paper (22)":
            case "S_Paper (19)":
            case "S_Paper (20)":
            case "S_Paper (21)":
                Audio.PlayOneShot(Paper_s);
                break;
            case "kitchendraw01":
                Audio.PlayOneShot(Dishes_s);
                break;
            case "kitchendraw04":
            case "kitchendraw03":
            case "Beddraw01":
            case "Beddraw02":
            case "Beddraw03":
            case "Beddraw04":
            case "Beddraw05":
            case "Beddraw06":
            case "Beddraw07":
            case "Tabledraw1":
            case "Tabledraw2":
            case "cover":
                Audio.PlayOneShot(Drawer_Open_s);
                break;
            case "door01":
            case "door02":
            case "door03":
            case "door04":
            case "door05":
                Audio.PlayOneShot(Drawer_Door_s);
                break;
            case "PFB_Stove":
                Audio.PlayOneShot(Stove_s);
                break;
            case "PFB_Fridge":
            case "PantryL":
            case "PantryR":
            case "tvStandDoor_L":
            case "tvStandDoor_R":
            case "BathroomdoorL":
            case "BathroomdoorR":
            case "BedroomDoorL":
            case "BedroomDoorR":
                Audio.PlayOneShot(Fridge_s);
                break;
            case "knife_01":
            case "M9_Knife":
                Audio.PlayOneShot(Knife_s);
                break;
            case "PowerLock":
            case "PowerLock1":
            case "PowerLock2":
                Audio.PlayOneShot(Button_s);
                break;
            case "shoes":
                Audio.PlayOneShot(Shoes_s);
                break;
            case "Window_Left":
                Audio.PlayOneShot(OpenWindow_s);
                break;
            case "Circle_mold":
                Audio.PlayOneShot(Brick_s);
                break;
            case "panel3":
            case "panel1":
                Audio.PlayOneShot(EMF_s);
                break;
            case "tv":
                Audio.PlayOneShot(TV_s);
                break;
            case "brocken projector":
                Audio.PlayOneShot(Beam_s);
                break;
            case "disc 4":
            case "projector 4":
            case "RedDisc":
            case "BlueDisc":
            case "BlackDisc":
                Audio.PlayOneShot(RockHit_s);
                break;
            case "fireext":
                Audio.PlayOneShot(Fire_s);
                break;
            case "SubstationBig":
                Audio.PlayOneShot(Power_s);
                break;
            case "cctvdoor":
                Audio.PlayOneShot(Ch2_Close_Door_s);
                break;
            case "pencere2":
            case "pencere3":
                Audio.PlayOneShot(Ch2_Window_Close_s);
                break;
            case "Bucket":
                Audio.PlayOneShot(Pull_s);
                break;
        }
    }
}
