using UnityEngine;

public class Compass : MonoBehaviour {
    [SerializeField] private Transform playerTransform;
    private Transform basePlate;        // 나침반의 기판.
    private Transform needle;           // 나침반의 자침.
    private Vector3 dir = Vector3.zero; // 갱신용 벡터.
    private float beforeZ = 0f;         // 이전값 저장용.

    // OnEnable() 나침반을 꺼낼 때마다 방향 세팅을 위해서.
    private void OnEnable()
    {
        if (!playerTransform)
            playerTransform = GameObject.FindWithTag("Player").transform;

        if (!basePlate)
        {
            basePlate = gameObject.transform.Find("baseplate");
            dir.x = -90f;   // 모델링의 자체 회전값 때문에 보정.  (추후 모델링을 손 볼..)
            // 플레이어 회전값(Quarternion)을 오일러(Euler)값으로 변환한 값을 갱신용 벡터에 대입.
            // 각 객체에 따른 회전축에 대해서는... (고민 좀;;)
            dir.z = playerTransform.eulerAngles.y;
            basePlate.localEulerAngles = dir;   // 나침반 기판의 회전값을 갱신.
        }
        
        if (!needle)
            needle = gameObject.transform.Find("needle");
        if (needle)
        {
            Vector3 tmp = Vector3.zero;
            tmp.x = -90f;   // 자침도 모델링의 자체 회전값 때문에 마찬가지로 보정.
            needle.localEulerAngles = tmp; // 자침의 회전값은 항상 고정.
        }
    }

    private void Update()
    {
        // 플레이어의 회전값이 변경되었을 때만 갱신. (딱히 효율적인 측면에서는 별 차이가 없을 것 같다...)
        if (beforeZ != playerTransform.eulerAngles.y)
        {
            dir.z = -playerTransform.eulerAngles.y;
            basePlate.localEulerAngles = dir;
            beforeZ = -dir.z;
        }
    }
}
