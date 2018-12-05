using UnityEngine;

public class DoorInteraction : MonoBehaviour {
    public Animator doorAnimator;

    private void Start()
    {
        if (doorAnimator)
            doorAnimator.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (doorAnimator)
        {
            if (other.CompareTag("Player"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameManager.gm.panelOpen == false)
                    {
                        RaycastHit hit = other.GetComponent<FirstPersonCamera>().getRaycastHit();
                        if (hit.collider != null)
                        {
                            if (hit.collider.CompareTag("Handle"))
                            {
                                doorAnimator.enabled = true;
                                hit.collider.tag = "Untagged";
                                Destroy(GetComponent<Collider>());
                                Destroy(this, 1f);
                                Animator ani = Camera.main.GetComponent<Animator>();
                                ani.runtimeAnimatorController = null;
                                SoundManager.instance.ChainDoor_Open();

                                if (hit.collider.name == "Ch5ExitDoor")
                                {
                                    GameObject.Find("Inventory").GetComponent<Inventory>().items.Remove(GameObject.Find("Item_PowerKey"));
                                    Destroy(GameObject.Find("Item_PowerKey"));
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Destroy(GetComponent<BoxCollider>());
            Destroy(this, 1f);
        }
    }
}
