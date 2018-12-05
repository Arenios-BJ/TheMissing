using UnityEngine;

public class PlayerNear : MonoBehaviour {

    private Attack_Ani PlayerAxe;
    public bool near;

	void Start () {

        PlayerAxe = GameObject.FindWithTag("Player").GetComponent<Attack_Ani>();
        near = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Player")
        {
            if(PlayerAxe.AxeCheck == true)
            {
                near = true;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {

        near = false;
        
    }
}
