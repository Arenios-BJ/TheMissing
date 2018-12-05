using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScriptTrigger : MonoBehaviour {
    private Collider coll;

    private void OnEnable()
    {
        coll = GetComponent<Collider>();
        coll.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("Ch2_Story").GetComponent<Ch2_Script>().Ch2Script.SetActive(true);
            Destroy(gameObject);
        }
    }
}
