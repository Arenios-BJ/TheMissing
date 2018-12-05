// 상호작용이 가능한 오브젝트를 클릭 시 오브젝트를 카메라 앞에 위치시켜 회전이 가능하게 하는 스크립트.
// 지금은 쓰지 않는다.

using UnityEngine;

public class ObjectPickUp : MonoBehaviour {
    private Vector3 originPos;
    private Quaternion originRot;
    private Transform originPapa;
    private FirstPersonCamera player;
    [SerializeField] private GameObject pickedObject;
    [SerializeField] private float distance = 0.6f;
    [SerializeField] private float sensitivity = 3f;
    private bool isPicked = false;

    private void OnEnable()
    {
        if (!player)
            player = FirstPersonCamera.player;
    }

    private void Update()
    {
        if (!isPicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (player.getRaycastHit().collider != null)
                {
                    if (player.getRaycastHit().transform.CompareTag("Object"))
                    {
                        pickedObject = player.getRaycastHit().collider.gameObject;
                        originPos = pickedObject.transform.position;
                        originRot = pickedObject.transform.rotation;
                        originPapa = pickedObject.transform.parent;
                        pickedObject.transform.parent = player.GetComponentInChildren<Camera>().transform;
                        pickedObject.tag = "Untagged";

                        pickedObject.transform.localPosition = Vector3.forward * distance;
                        pickedObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                        player.ChangeMoveRotaState(false);
                        isPicked = true;
                        Time.timeScale = 0.0f;
                    }
                }
            }
        }
        else    // isPicked == true.
        {
            if (pickedObject)
            {
                if (Input.GetMouseButton(1))
                {
                    float h = Input.GetAxis("Mouse Y") * sensitivity;
                    float v = Input.GetAxis("Mouse X") * sensitivity;
                    pickedObject.transform.Rotate(h, v, 0f);
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    pickedObject.transform.position = originPos;
                    pickedObject.transform.rotation = originRot;
                    pickedObject.transform.parent = originPapa;
                    pickedObject.tag = "Object";

                    player.ChangeMoveRotaState(true);
                    isPicked = false;
                    pickedObject = null;
                    Time.timeScale = 1f;
                }
            }
            else
                isPicked = false;
        }
    }
}
