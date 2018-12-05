using UnityEngine;
using UnityEngine.UI;

public class DodgeMissile : MonoBehaviour {

    private bool isIn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isIn)
            if (other.transform.name == "Img_Background")
                Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Img_Background")
        {
            GetComponent<RawImage>().enabled = true;
            isIn = true;
        }
    }
}
