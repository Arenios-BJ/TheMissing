using UnityEngine;
using UnityEngine.UI;

public class DodgePlayerMove : MonoBehaviour {
    
    public float speed = 5f;
    Vector2 move;
    Vector2 scale;
    Rect canvasSize;

    void Start()
    {
        scale.Set(gameObject.transform.root.Find("Cnv_Dodge").transform.localScale.x, gameObject.transform.root.Find("Cnv_Dodge").transform.localScale.y);
        canvasSize = (gameObject.transform.parent as RectTransform).rect;
    }

    void Update()
    {
        if (GameManager.gm.panelOpen == false)
        {
            CheckInput(); //사용자 입력감지

            move *= speed;
            move *= scale;
            gameObject.transform.Translate(move.x, move.y, 0, gameObject.transform.parent);

            CheckOutOfScreen(); //플레이어의 화면이탈 방지
        }
    }

    void CheckInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        move = new Vector2(xInput, yInput).normalized; // 현재 벡터를 유지하고, 변경시키고 싶지 않은 경우에 사용
    }

    void CheckOutOfScreen()
    {
        float nextX = Mathf.Clamp(transform.localPosition.x, canvasSize.xMin, canvasSize.xMax);
        float nextY = Mathf.Clamp(transform.localPosition.y, canvasSize.yMin, canvasSize.yMax);
        transform.localPosition = new Vector3(nextX, nextY, 0f);
    }

    // 충돌 판정
    void OnTriggerEnter(Collider coll)
    {
        // Bullet과 부딪혔다면
        if (coll.gameObject.tag == "Enemy")
        {
            // 플레이어 오브젝트를 비활성화.
            gameObject.SetActive(false);
            coll.gameObject.GetComponent<RawImage>().enabled = true;
            SoundManager.instance.Dodg_Fail();
        }
    }
}
