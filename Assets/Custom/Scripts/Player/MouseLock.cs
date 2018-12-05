/* MouseLock.cs / Last modified date : 2018-08-08 / Last modifier : Yang Jae Hyeok
 * 
 *** Script Description
 * 마우스 커서 고정 및 표시를 관리하는 스크립트.
 * FristPersonCamera 스크립트가 붙어있는 동일한 오브젝트에 컴포넌트로 붙여 사용한다.
 * 
 *** 참고 사항
 * FirstPersonCamera 스크립트에서 호출하여 동작한다.
 * 
 * 완성.(버그가 없다면...)
*/

using UnityEngine;

public class MouseLock : MonoBehaviour {
    [SerializeField] public bool m_IsLock;  // 마우스 커서의 잠금 상태.

    private void ChangeMouseState()
    {
        if (m_IsLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// 마우스 커서의 화면 중앙 Lock과 화면 표시를 설정.
    /// </summary>
    /// <param name="set">true : 잠금 / false : 해제</param>
    public void ChangeMouseLock(bool set)
    {
        if (set)
            m_IsLock = true;
        else
            m_IsLock = false;

        ChangeMouseState();
    }
}
