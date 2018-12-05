using UnityEngine;

public class StroyPoint : MonoBehaviour {

    private bool Check;

	void Start () {

        Check = false;

    }
	

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Check == false)
            {
                GameObject.Find("Ch6_Story").GetComponent<Ch6_Script>().Ch6Script.SetActive(true);
                Check = true;
                GameObject.Find("MunJaPoint").GetComponent<MunJaPoint>().Hole.enabled = false;
            }
        }
    }
}
