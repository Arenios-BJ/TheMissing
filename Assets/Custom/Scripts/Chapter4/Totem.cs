using UnityEngine;

public class Totem : MonoBehaviour {

    private FirstPersonCamera player;
    public GameObject Missing1;
    public GameObject Missing2;
    public GameObject Missing3;
    public GameObject Missing4;
    public GameObject Missing5;
    public GameObject Missing6;
    public GameObject Missing7;

    private GameObject MissingImage;

    private bool MissingCheck;

    private float count;

    private bool totemState;

    void Start () {

        player = FirstPersonCamera.player;
        Missing1.SetActive(false);
        Missing2.SetActive(false);
        Missing3.SetActive(false);
        Missing4.SetActive(false);
        Missing5.SetActive(false);
        Missing6.SetActive(false);
        Missing7.SetActive(false);

        MissingImage = GameObject.Find("MissingImage");

        count = 0;

        totemState = true;

        MissingCheck = true;
    }

    void Update()
    {

        RaycastHit hit = player.getRaycastHit();    // 플레이어 객체에서 RaycastHit 정보를 받아온다.
        if (hit.transform != null)                  // 레이가 무언가와 충돌했다면.
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (hit.transform.tag == "Totem")   // 
                    {
                        if (totemState == true)
                        {
                            SoundManager.instance.SelectSound(hit.transform.name);
                            count++;
                            Destroy(hit.collider);
                            hit.transform.GetComponent<Animator>().enabled = false;

                        }
                    }
                }
            }
        }

        if (count == 1)
        {
            Missing1.SetActive(true);
        }

        if (count == 2)
        {
            Missing2.SetActive(true);
        }

        if (count == 3)
        {
            Missing3.SetActive(true);
        }

        if (count == 4)
        {
            Missing4.SetActive(true);
        }

        if (count == 5)
        {
            Missing5.SetActive(true);
        }

        if (count == 6)
        {
            Missing6.SetActive(true);
        }

        if (count == 7)
        {
            Missing7.SetActive(true);
            MissingCheck = false;
        }

        if (MissingCheck == false)
        {
            MissingImage.transform.Translate(Vector3.up * Time.deltaTime, Space.World);
            count = 8;
            SoundManager.instance.MisImageS();

            if (MissingImage.transform.position.y >= 107.5f)
            {
                MissingCheck = true;
            }
        }
    }
}
