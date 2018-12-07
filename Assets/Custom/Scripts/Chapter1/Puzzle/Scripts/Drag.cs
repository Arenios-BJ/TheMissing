using UnityEngine;
using UnityEngine.EventSystems;

// 첫 번째 오두막 퍼즐 Drag 스크립트
// 사용한 방법 : Find / OnBeginDrag / OnDrag / OnEndDrag / SetParent

// Drag 기능을 쓰려면 IDragHandler, IBeginDragHandler, IEndDragHandler 상속 받아야 한다.
public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // 선택한 퍼즐의 위치를 담기 위함
    private Transform ItemTr;
    // 유저가 채워야 할 곳
    private Transform ItemList;
    // 처음에 퍼즐이 놓여져 있는 곳
    private Transform GameBoardList;
   
    // 내가 어떤 아이템을 드래그 했는지 알기 위함
    public static GameObject draggingItem;

    void Start()
    {
        ItemTr = GetComponent<Transform>();
        ItemList = GameObject.Find("PuzBoard").GetComponent<Transform>();
        GameBoardList = GameObject.Find("gameboard").GetComponent<Transform>();
    }

    // 드래그를 시작할 때 한 번 호출되는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그한 아이템은, 선택한 그 오브젝트가 된다.
        draggingItem = this.gameObject;
        // 처음 아이템의 부모는 gameboard이다.
        this.transform.SetParent(GameBoardList);
        SoundManager.instance.Move_Pannel();
    }

    // 드래그 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 이벤트가 발생하면 아이템의 위치를 마우스 커서의 위치로 변경
        ItemTr.position = Input.mousePosition;
    }

    // 드래그가 끝났을 때
    public void OnEndDrag(PointerEventData eventData) //여기다가 음악넣기
    {
        SoundManager.instance.Move_Pannel();
        // 선택한 아이템의 부모가 gameboard라면
        if (ItemTr.parent == GameBoardList)
        {
            // 선택한 아이템의 부모를 PuzBoard로 바꾼다.
            ItemTr.SetParent(ItemList.transform);
        }

        // 아이템의 부모가 PuzBoard라면
        if (ItemTr.parent == ItemList)
        {
            // 아이템의 부모를 gameboard로 바꾼다.
            ItemTr.SetParent(GameBoardList.transform);
        }
    }
}
