using UnityEngine;

public class Check_hit_door : MonoBehaviour {

    public float count = 0;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Axe")
        {
            count++;
        }

        if (count == 3)
        {
            Destroy(gameObject);
        }
    }
}
