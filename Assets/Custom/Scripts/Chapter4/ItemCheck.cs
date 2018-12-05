using UnityEngine;

public class ItemCheck : MonoBehaviour {

    private Inventory Key;

    void Start () {

        Key = GameObject.Find("Inventory").GetComponent<Inventory>();

    }
	
	void Update () {
		
        if(Key.CarKey == true && Key.PowerKey == true && Key.Hint == true && Key.Circle3 == true)
        {
            Destroy(this.gameObject);
        }
	}

    void OnCollisionEnter(Collision col)
    {
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().Story.SetActive(true);
        GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().ChangeText.text = "집을 좀 더 찾아봐야겠어.";
    }
}
