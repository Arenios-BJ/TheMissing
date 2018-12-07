using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마지막 퍼즐과 관련된 스크립트 -> 밟았을 때 석판의 색깔 변화와 위치이동, 그리고 리셋되었을 때 석판의 변화와 플레이어의 변화를 나타냄
// 사용한 방법 : bool / FindWithTag / Color32 / SpriteRenderer / transform.position / transform.localPosition / transform.Translate

public class LastPuz : MonoBehaviour {

    private SpriteRenderer sprite;

    bool UpCheck;

    private LastPuzManager PuzPlayer = null;

    void Start () {


        sprite = GetComponent<SpriteRenderer>();

        UpCheck = true;

        if (GameObject.FindWithTag("Player"))
            PuzPlayer = GameObject.FindWithTag("Player").GetComponent<LastPuzManager>();

    }
	
	void Update () {

        // 플레이어가 석판을 제대로 밟지 못했다면, 석판이 다시 흰색으로 돌아가고 위치도 돌아간다.
        if (PuzPlayer)
        {
            if (GameObject.FindWithTag("Player").GetComponent<LastPuzManager>().count == 1)
            {
                sprite.color = new Color32(255, 255, 255, 255);

                if (transform.position.y > -9)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, -9f, transform.localPosition.z);
                }

                UpCheck = true;

            }
        }
    }

    // 플레이어가 석판을 밟았다면 색깔이 노란색으로 변하고 위로 올라옴
    void OnCollisionEnter(Collision col)
    {
        if (UpCheck == true)
        {
            if (col.gameObject.tag == "Player")
            {
                sprite.color = new Color32(255, 255, 150, 255);
                transform.Translate(Vector3.up * 1, Space.World);
                SoundManager.instance.BrickS();
                UpCheck = false;
            }
        }
    }
}
