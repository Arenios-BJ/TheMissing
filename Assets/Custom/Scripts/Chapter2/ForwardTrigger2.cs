using UnityEngine;

public class ForwardTrigger2 : MonoBehaviour {

    [SerializeField] private GameObject AfterBookTrigger;
    private Transform parent;

    private void Start()
    {
        parent = GameObject.Find("Img_ItemList").transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameObject.Find("Item_Map").transform.parent == parent
                && GameObject.Find("Item_CircleCCTV").transform.parent == parent)
                if (GameObject.Find("DoctorBook"))
                    if (GameObject.Find("DoctorBook").GetComponent<DoctorBook>().isOpened)
                    {
                        AfterBookTrigger.SetActive(true);
                        Destroy(gameObject);
                        return;
                    }

            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().SetScript("무언가를 놓친 것같아. 집을 다시 뒤져보자.");
        }
    }
}
