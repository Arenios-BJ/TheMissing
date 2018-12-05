using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
