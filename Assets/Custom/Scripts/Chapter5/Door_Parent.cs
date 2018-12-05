using UnityEngine;

public class Door_Parent : MonoBehaviour {

    public bool D_1 = false;
    public bool D_2 = false;
    public bool D_3 = false;
    public bool D_4 = false;
    public bool D_5 = false;
    public bool D_6 = false;
    public bool D_7 = false;
    public bool D_8 = false;
    public bool D_9 = false;
    public bool D_10 = false;
    
    public bool DX_1 = false;
    public bool DX_2 = false;
    public bool DX_3 = false;
    public bool DX_4 = false;
    public bool DX_5 = false;
    public bool DX_6 = false;

    public void ColorChange(GameObject target)
    {
        if (target.GetComponent<Door_Child>().isPushed)
        {
            Vector3 pos = target.transform.position;
            pos.x = pos.x + 0.05f;
            target.transform.position = pos;
            target.GetComponent<Door_Child>().isPushed = false;
        }
        else    // isClicked = false. 이 패드가 눌러져있지 않을 때.
        {
            Vector3 pos = target.transform.position;
            pos.x = pos.x - 0.05f;
            target.transform.position = pos;
            target.GetComponent<Door_Child>().isPushed = true;
        }

        if (target.name == "Cube")
        {
            D_1 = !D_1;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (1)")
        {
            D_2 = !D_2;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (2)")
        {
            D_3 = !D_3;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (3)")
        {
            D_4 = !D_4;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (4)")
        {
            DX_1 = !DX_1;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (5)")
        {
            DX_2 = !DX_2;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (6)")
        {
            D_5 = !D_5;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (7)")
        {
            DX_3 = !DX_3;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (8)")
        {
            DX_4 = !DX_4;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (9)")
        {
            D_6 = !D_6;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (10)")
        {
            DX_5 = !DX_5;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (11)")
        {
            DX_6 = !DX_6;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (12)")
        {
            D_7 = !D_7;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (13)")
        {
            D_8 = !D_8;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (14)")
        {
            D_9 = !D_9;
            SoundManager.instance.BrickS();
        }
        else if (target.name == "Cube (15)")
        {
            D_10 = !D_10;
            SoundManager.instance.BrickS();
        }
        else
            return;
    }

    public void removeChildrenScript()
    {
        Door_Child[] children = GetComponentsInChildren<Door_Child>();

        for (int i = 0; i < children.Length; i++)
        {
            Destroy(children[i].GetComponent<Collider>());
            Destroy(children[i]);
        }
    }
}
