using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pig_item_trigger : MonoBehaviour {
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (GameObject.Find("Inventory").GetComponent<Inventory>().Circle2 == false)
            {
                GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().SetScript("돼지가 뭔가 떨어트렸어. 확인해보자.");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
