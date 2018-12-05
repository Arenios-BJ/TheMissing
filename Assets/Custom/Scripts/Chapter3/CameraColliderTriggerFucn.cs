/* CameraColliderTriggerFucn.cs / Last modified date : 2018-08-09 / Last modifier : Yang Jae Hyeok
 * 
 *** Script Description
 * CCTV 카메라의 자식 오브젝트에 붙어있는 MeshCollider(카메라 시야 체크용)의 OnTriggerXXX 이벤트 스크립트.
 * CCTV 오브젝트의 Camera 오브젝트에 Empty 오브젝트를 자식으로 만들어 붙여 사용한다.
 * 
 *** 참고 사항
 * 이 스크립트에서는 충돌 검사에 따른 이벤트 실행만 하고 실질적인 동작은
 * 내부 변수로 외부 컴포넌트 및 스크립트를 받아와 그 내부 함수를 호출함으로써 동작한다.
 *
 * 완성.
*/

using UnityEngine;

[RequireComponent(typeof(MeshCollider))]    // 필요 컴포넌트. 충돌 이벤트 사용을 위한 카메라 Viewport와 유사한 MeshCollider.
public class CameraColliderTriggerFucn : MonoBehaviour {
    private CameraMovement parentsTrigger;

    private void Start()
    {
        if (transform.parent.GetComponent<CameraMovement>())
            parentsTrigger = transform.parent.GetComponent<CameraMovement>();
    }

    private void OnTriggerEnter(Collider other) // Enter. 다른 Collider 또는 RigidBody와 충돌한 처음 프레임.
    {
        if (other.CompareTag("Player"))
        {
            if (parentsTrigger != null)
                parentsTrigger.ChangeCoroutineState();  // 플레이어 충돌(=감지) 시 회전 코루틴 중지.
        }
    }

    private void OnTriggerStay(Collider other)  // Stay. Enter 이후 충돌 오브젝트가 지속적으로 충돌중일 때.
    {
        if (other.CompareTag("Player"))
        {
            if (parentsTrigger != null)
                parentsTrigger.PlayerChase(other.transform);    // 플레이어가 콜라이더 내부에 있을 경우(=지속) 위치 추적.
        }
    }

    private void OnTriggerExit(Collider other)  // Exit. Enter or Stay 이후 충돌 오브젝트가 충돌 범위(Collider 크기)를 벗어났을 때.
    {
        if (other.CompareTag("Player"))
        {
            if (parentsTrigger != null)
                parentsTrigger.ChangeCoroutineState();  // 플레이어 충돌 해제(=놓침) 시 회전 코루틴 시작.
        }
    }
}
