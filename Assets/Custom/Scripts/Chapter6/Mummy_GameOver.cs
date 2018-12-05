using UnityEngine;

public class Mummy_GameOver : MonoBehaviour {

    private bool Check;

	void Start () {

        Check = false;

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            if (Check == false)
            {
                GameObject.Find("Post_VinetteManager").GetComponent<Post_Vinet>().GameOver();
                GameObject.Find("Post_VinetteManager").GetComponent<Post_Vinet>().text.SetActive(true);
                Check = true;
            }

        }
    }
}
