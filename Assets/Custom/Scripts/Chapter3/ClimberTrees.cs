/* ClimberTrees.cs / Last modified date : 2018-08-24 / Last modifier : Yang Jae Hyeok
 * 
 *** Script Description
 * CCTV가 달려있는 나무 오브젝트에 붙여서 사용한다.
 * 콜라이더(트리거)를 통해 플레이어 충돌 및 키 입력을 검사하는 프로세스를 담당한다.
 * 
 *** 참고 사항
 * 주가 되는 TreeClimberGame 스크립트는 DontDestroyObject로 작동된다.
 * 
 * 완성.(버그가 없... 딱히 짧아서 없을거야.)
*/

using UnityEngine;
using UnityEngine.EventSystems;

public class ClimberTrees : MonoBehaviour {
    [SerializeField] private TreeClimberGame gameScript;

	void Start () {
        if (gameScript == null) // 주 스크립트가 없으면 안되니까 직접 할당 해드립니다.
        {
            if (GameObject.Find("TreeClimberGame") != null) 
            {
                gameScript = GameObject.Find("TreeClimberGame").GetComponent<TreeClimberGame>();
                return;
            }
            else    // 없을 수도 있으니까 예외 처리.
            {
                if (GameObject.Find("Pnl_ClimberGame") != null)
                    GameObject.Find("Pnl_ClimberGame").gameObject.SetActive(false); // '나무 타기 게임' 패널 비활성화.
                gameObject.GetComponent<CapsuleCollider>().enabled = false; // 이벤트 발생 안되게 트리거 콜라이더 비활성화.
                gameObject.tag = "Untagged";    // QuickOutline 안되게 태그 변경.
            }
        }
	}

    private void OnTriggerStay(Collider other)  // 콜라이더가 충돌하고 있을 때만 체크. (지속 검사가 필요하기에)
    {
        if (GameManager.gm.panelOpen == false)
        {
            if (gameScript.IsGaming() == false && Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current.IsPointerOverGameObject() == false)
                    if (other.CompareTag("Player"))
                    {
                        gameScript.SetupBeforeMiniGame(other.gameObject, gameObject.transform.parent.Find("CCTV").gameObject);
                    }
            }
            else if (Input.GetKeyUp(KeyCode.Escape) && gameScript.IsGaming() == true)
            {
                if (other.CompareTag("Player"))
                {
                    gameScript.SetupAfterMiniGame(other.gameObject);
                }
            }
        }
    }
}
