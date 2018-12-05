using UnityEngine;

public class INCheck : MonoBehaviour {

    private bool PlayerIn;

    public GameObject OutCheck;

    public GameObject ScriptPoint;

    public GameObject backTrigger;

    void Start () {

        PlayerIn = false;
        
        OutCheck.SetActive(false);
        ScriptPoint.SetActive(false);
    }


    void OnTriggerEnter(Collider col)
    {
        if(PlayerIn == false)
        {
            if (col.gameObject.name == "Player")
            {
                OutCheck.SetActive(true);
                ScriptPoint.SetActive(true);
                Destroy(backTrigger);
                PlayerIn = true;
            }
        }
    }
}
