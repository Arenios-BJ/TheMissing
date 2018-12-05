using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    // 드롭 이벤트
    public void OnDrop(PointerEventData eventData)
    {
        // 자식의 갯수가 0이라면
        if (transform.childCount == 0)
        {
            // 현재 드래그 중인 아이템 (Drag.draggingItem) 을 Drop한 위치에 놓는다.
            Drag.draggingItem.transform.SetParent(this.transform);
        }
    }
}
