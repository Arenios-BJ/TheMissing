using UnityEngine;

public class WoodenDoor : MonoBehaviour
{

    private Animator wooden_door;
    private GameObject BlueKeyCheck;

    private FirstPersonCamera player;

    private Inventory inven;

    void Start()
    {

        wooden_door = GameObject.Find("old_wooden_door").GetComponent<Animator>();

        wooden_door.enabled = false;

        BlueKeyCheck = GameObject.Find("Inventory");

        player = FirstPersonCamera.player;

        inven = GameObject.Find("Inventory").GetComponent<Inventory>();

    }

    void Update()
    {

        FAFA();

    }

    void FAFA()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (GameManager.gm.panelOpen == false)
            {
                if (BlueKeyCheck.GetComponent<Inventory>().BlueKey == true)
                {
                    RaycastHit hit = player.getRaycastHit();
                    if (hit.transform.name == "old_wooden_door")
                    {
                        wooden_door.enabled = true;
                        wooden_door.tag = "Untagged";
                        Destroy(gameObject, 1f);
                        inven.items.Remove(GameObject.Find("Item_BlueKey"));
                        Destroy(GameObject.Find("Item_BlueKey"));
                        BlueKeyCheck.GetComponent<Inventory>().BlueKey = false;
                    }
                }
            }
        }
    }
}
