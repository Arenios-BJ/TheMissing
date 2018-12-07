using UnityEngine;

// 보스의 집에서 필수 아이템을 먹었는지 체크하기 위한 스크립트
// 모두 먹었다면 나갈 수 있을 것이고, 그렇지 않다면 나가지 못함
// 사용한 방법 : Find / Destroy / OnCollisionEnter / SetActive

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
