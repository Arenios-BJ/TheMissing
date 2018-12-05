/* CameraMovement.cs / Last modified date : 2018-08-10 / Last modifier : Yang Jae Hyeok
 * 
 *** Script Description
 * CCTV 카메라의 자동 회전 및 플레이어 추적 AI 스크립트.
 * 게임 내에서 CCTV로 사용할 Camera 오브젝트에 컴포넌트로 붙여 사용한다.
 * 자식 오브젝트의 MeshCollider의 충돌 이벤트에서 본 스크립트 public 함수를 호출해 카메라 회전 코루틴을 조작할 수 있다.
 * 카메라의 시야(충돌 판단은 자식 오브젝트에 붙은 Collider로)에 플레이어가 감지되었을 경우 플레이어 위치를 따라 추적 회전한다.
 * 최소, 최대 회전각 이상으로는 추적이 불가능하며 시야에서 놓치면(충돌 해제) 자동 회전 상태로 복귀한다.
 * 
 *** 참고 사항
 * Transform 컴포넌트의 rotation(Quaternion 구조체) 변수의 eulerAngles(Vector3 구조체) 변수의
 * x, y, z축 각각의 회전값(float)는 범위가 0 ~ 359f이다. 0f에서 반시계로 1f 회전하면 359f가 된다.
 * 회전에 오차 범위값을 적용할 때 rotateSpeed를 그대로 쓰는 이유는 eulerAngle의 값이 미세하게 오차가 발생하기 때문.(좀 더 넓은 오차 범위 적용)
 * 플레이어 추적 시 최소, 최대각 이탈 방지를 위한 예외 처리로 인해 플레이어 추적 경우의 실제 카메라 최소, 최대각이 기본 설정보다 미세하게 작다.
 * 
 * 완성.(제발 버그 없어라...)
*/

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]      // 필요 컴포넌트. '카메라'
public class CameraMovement : MonoBehaviour {
    [SerializeField] private Transform parent;  // Camera 오브젝트를 자식으로 하는 CCTV 모델 오브젝트의 Transform.
    [SerializeField] private float rotateSpeed = 0.1f;  // 회전 속도. 양수값 : 시계방향 / 음수값 : 반시계방향.
    [SerializeField] private float rotateRange = 45f;   // 초기 카메라 위치를 기준으로 좌,우 회전 각도 범위.
    [SerializeField] private float chaseSpeed = 0.2f;   // 카메라가 플레이어 감지 시 추적하여 회전하는 속도.
    [SerializeField] private float minChaseDist = 1f;   // 카메라가 플레이어를 추적하는 최소 거리 차이.

    private Camera cam;         // 플레이어의 스크린 좌표를 구하기 위한 현재 Camera 인스턴스.
    private Vector3 originTargetPos;

    private float maxRotateDeg; // 시계 방향으로 회전 시 최대 각도 값.
    private float minRotateDeg; // 반시계 방향으로 회전 시 최소 각도 값.

    private IEnumerator rotateCoroutine = null; // coroutine 변수.
    private bool IsRotateCoroutine;             // 코루틴 동작 여부.


    // 내부 로직 함수들.
    private void Start () {
        cam = GetComponent<Camera>();
        originTargetPos.y = -10f;

        if (parent == null) // 유니티 에디터에서 지정을 안 해줬을 경우.
            parent = gameObject.transform.parent;   // 컴포넌트 오브젝트의 부모 오브젝트 할당.

        maxRotateDeg = parent.rotation.eulerAngles.y + rotateRange;
        if (maxRotateDeg >= 360f)   // 현재 각도값에 회전 범위를 더했을 때 그 값이 360보다 클 때.
            maxRotateDeg -= 360f;   // 위 참고 사항에 말한대로 0 ~ 359의 범위로 변경. 360 == 0

        minRotateDeg = parent.rotation.eulerAngles.y - rotateRange;
        if (minRotateDeg < 0f)      // maxRotateDeg와 동일.
            minRotateDeg += 360f;

        if (minRotateDeg > maxRotateDeg)    // 회전 범위가 359 - 0 을 넘어가면 min이 max보다 커지는 경우가 발생.
            maxRotateDeg += 360f;

        rotateCoroutine = CameraRotate();   // 코루틴(IEnumerator) 함수를 변수(IEnumerator)에 대입.
        if (IsRotateCoroutine != true)
        {
            StartCoroutine(rotateCoroutine);    // 카메라 회전 코루틴 시작.
            IsRotateCoroutine = true;
        }
    }

    private void OnEnable()
    {
        if (rotateCoroutine != null)
        {
            StartCoroutine(rotateCoroutine);    // 카메라 회전 코루틴 시작.
            IsRotateCoroutine = true;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(rotateCoroutine);    // 카메라 회전 코루틴 정지.
        IsRotateCoroutine = false;
    }

    private IEnumerator CameraRotate()
    {
        float rotY;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();   // while(true)문 안의 yield return 구문마다 new를 해주면 별로라길래...
        while (true)
        {
            rotY = getCurrentRotationY();   // 현재 회전값(부모의)을 가져온다.

            // rotY(현재 회전 값)이 최대, 최소 회전 각도 값의 오차 범위(+-rotateSpeed)안에 들어왔을 때 회전 방향을 전환.
            if ((maxRotateDeg - rotateSpeed <= rotY && rotY < maxRotateDeg + rotateSpeed)
                // rotateSpeed의 값이 음수라서 오차 범위 계산 부호도 반대로 적용.
                || (minRotateDeg + rotateSpeed <= rotY && rotY < minRotateDeg - rotateSpeed))
                // 회전 값을 반대로 주기 위해 값의 부호만 바꿔줌.
                rotateSpeed *= -1f;

            parent.Rotate(Vector3.up, rotateSpeed, Space.World);    // World 좌표계상의 up벡터(Y축)을 기준으로 회전.

            yield return wait;
        }
    }

    private float getCurrentRotationY()
    {
        float rotY = parent.rotation.eulerAngles.y;   // 현재 회전 값을 받아옴.
        // maxRotateDeg 값이 360 이상이라는 것은 회전 범위가 0을 기준으로 좌우에 포진되어있다는 것을 의미한다.
        // rotY값이 사분면에서 Y축 기준으로 좌측일 때는 min과 max 사이의 정상값이기 때문에 예외 처리.
        if (maxRotateDeg > 360f && (0f <= rotY && rotY < maxRotateDeg - 360f))
            rotY += 360f;
        rotY = Mathf.Clamp(rotY, minRotateDeg, maxRotateDeg);   // rotY 값을 min과 max 값의 범위로 제한한 값을 반환.

        return rotY;
    }

    // 외부 접근(컨트롤) 함수.
    public void ChangeCoroutineState()  // CameraRotate를 조작하는 함수.
    {
        if (IsRotateCoroutine)          // 코루틴이 동작중일 때. => 멈춘다.
        {
            StopCoroutine(rotateCoroutine); // 동작중인 코루틴이 있든가, 말든가, 중지 요청.
                                            // 동작중인 코루틴이 없어도 에러 발생하지 않음.
        }
        else                            // 코루틴이 동작중이 아닐 때. => 동작시킨다.
        {
            StopCoroutine(rotateCoroutine);   // 기존의 동작중인 CameraRotate() 코루틴이 있다면 중지.
            StartCoroutine(rotateCoroutine);  // 후 재시작.
        }
        IsRotateCoroutine = !IsRotateCoroutine;
    }

    public void PlayerChase(Transform target)   // 플레이어 추적 함수.
    {
        if (target.position == originTargetPos)
            return; // 플레이어의 위치가 변하지 않았다면 굳이 계산할 필요가 있을까?

        float rotY = getCurrentRotationY();

        if ((minRotateDeg + chaseSpeed) < rotY && rotY < (maxRotateDeg - chaseSpeed))   // 플레이어가 카메라 회전 각도 사이에 있을 경우에만 추적.
        {
            Vector3 screenPos = cam.WorldToScreenPoint(target.position);    // target의 카메라 스크린 상의 좌표. z 좌표값은 무시하자.
            float dist = (cam.pixelWidth / 2f) - screenPos.x;   // 카메라 스크린의 중앙 위치에서 타겟의 위치를 뺀다..

            if (Mathf.Abs(dist) > minChaseDist) // 최소 추적 거리보다 멀어야 됨.
            {
                rotY = 0f;      // rotY 변수 재활용.
                if (dist < 0f)  // dist가 음수라면 타겟은 중앙(스크린 상의 Y축)보다 우측에 위치.
                    rotY = 0 + chaseSpeed;
                else            // 양수라면 타겟은 기준보다 좌측에 위치.
                    rotY = 0 - chaseSpeed;

                parent.Rotate(Vector3.up, rotY, Space.World);   // 회전~
            }
            else
                originTargetPos = target.position;  // 현재 프로세스 단계의 타겟의 위치값을 저장.

            rotY = getCurrentRotationY();   // 재활용.
            if (rotY < minRotateDeg + chaseSpeed)   // 카메라의 회전값이 최대(최소)값을 벗어나면 반대방향으로 역회전을 걸어줌으로써 이탈 방지.
            {
                parent.Rotate(Vector3.up, 0 + chaseSpeed, Space.World);
                originTargetPos = target.position;
            }
            else if (maxRotateDeg - chaseSpeed < rotY)
            {
                parent.Rotate(Vector3.up, 0 - chaseSpeed, Space.World);
                originTargetPos = target.position;
            }
        }
    }
}
