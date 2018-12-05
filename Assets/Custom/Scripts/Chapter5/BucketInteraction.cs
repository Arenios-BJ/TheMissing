using UnityEngine;

public class BucketInteraction : MonoBehaviour {

    private FirstPersonCamera player;
    private Animation anim;
    private QuickOutline outline;

	void Start () {
        player = FirstPersonCamera.player;
        anim = GetComponent<Animation>();
        outline = GetComponent<QuickOutline>();
	}
	
	void Update () {
        if (outline.enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (player.getRaycastHit().transform == transform)
                    {
                        anim.Play();
                        tag = "Untagged";
                        Destroy(outline);
                        Destroy(this);
                    }
                }
            }
        }
	}
}
