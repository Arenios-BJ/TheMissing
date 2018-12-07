using UnityEngine;

// 첫 번째 오두막에서 파란 열쇠를 먹었을 때만 문을 열 수 있음. 파란 열쇠와 관련된 문 스크립트
// 사용한 방법 : Animator / enabled / Find / RaycastHit / Destroy

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
                        // 파란 열쇠-문을 열었다면 아이템 리스트에서도 파란 열쇠를 삭제 해준다!
                        inven.items.Remove(GameObject.Find("Item_BlueKey"));
                        Destroy(GameObject.Find("Item_BlueKey"));
                        BlueKeyCheck.GetComponent<Inventory>().BlueKey = false;
                    }
                }
            }
        }
    }
}
