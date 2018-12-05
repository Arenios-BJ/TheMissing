using UnityEngine;

public class Door_Child : MonoBehaviour {

    [SerializeField] Door_Parent parent;
    [SerializeField] FirstPersonCamera player;
    public bool isPushed = false;

	void Start () {
        parent = gameObject.GetComponentInParent<Door_Parent>();
        player = FirstPersonCamera.player;
    }
	
	void Update () {
        opendoor();
	}

    void opendoor()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (GameManager.gm.panelOpen == false)
            {
                if (player.getRaycastHit().collider != null)
                {
                    if (player.getRaycastHit().collider.gameObject == this.gameObject)
                    {
                        parent.ColorChange(gameObject);
                    }
                }
            }
        }
    }
}
