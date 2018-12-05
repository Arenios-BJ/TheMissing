using UnityEngine;

public class HandleControll : MonoBehaviour {
    private FirstPersonCamera player;
    [SerializeField] private Transform target;
    public Vector3 moveToPos = Vector3.zero;

    private void Start()
    {
        if (GameObject.FindWithTag("Player"))
            player = FirstPersonCamera.player;
    }

    private void Update()
    {
        if (player.getRaycastHit().transform != null && player.getRaycastHit().transform == transform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (target)
                {
                    target.localPosition = moveToPos;
                    Destroy(this, 1f);
                }
            }
        }
    }
}
