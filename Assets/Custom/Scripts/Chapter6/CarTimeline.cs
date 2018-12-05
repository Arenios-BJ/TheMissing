using UnityEngine;
using UnityEngine.Playables;


public class CarTimeline : MonoBehaviour {

    private Inventory inven;
    private FirstPersonCamera player;
    private GameObject car;
    private GameObject FakeCar;

    public PlayableDirector playerdirector;

    public GameObject End;
    public GameObject End2;

    float _time;

    void Start () {

        player = FirstPersonCamera.player;
        inven = GameObject.Find("Inventory").GetComponent<Inventory>();
        FakeCar = GameObject.Find("Car2");
        car = GameObject.Find("Car");
        car.SetActive(false);

        End.SetActive(false);
        End2.SetActive(false);
    }
	
	void Update () {

        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (inven.CarKey == true)
                    {
                        if (hit.transform.name == "Classic_16_Door_L")   //
                        {
                            Destroy(FakeCar);
                            car.SetActive(true);
                            inven.items.Remove(GameObject.Find("Item_CarKey"));
                            Destroy(GameObject.Find("Item_CarKey"));
                            GameObject.Find("Ch6_Story").GetComponent<Ch6_Script>().Ch6Script.SetActive(true);

                            GetComponent<DeadAniManager>().Ani();

                            Destroy(GameObject.Find("Wall"));
                        }
                    }
                }
            }
        }

        if(playerdirector.time > 18)
        {
            End.SetActive(true);

            _time += Time.deltaTime;
        }

        if (_time > 2)
        {
            End2.SetActive(true);
            GameObject.Find("_Environments").GetComponent<BackSound>().back.enabled = false;
            _time = 0;
        }
    }
}
