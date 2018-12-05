using System.Collections;
using UnityEngine;

public class Stone : MonoBehaviour {

    public bool drop_stone; //돌이 던져졌는지를 나타내는 bool변수
    public bool down_stone; //돌이 바닥에 먼저 맞은건지를 나타내는 bool변수

    float timer;

    private void Start()
    {
        drop_stone = false; //필드에 있는 돌이 못움직이게 하기위해 bool로 막아둔다.
        StartCoroutine(Delay()); //첫 생성되는 돌이 겹쳐지지않아야 하기때문에 바로 코루틴을 호출한다.
    }
  
    private void Update()
    {
       
        if (drop_stone == true) //GameManager스크립트에서 돌을던지면 true로 바뀜
        {
            timer += Time.deltaTime; //timer변수를 만든다(매초마다 값이 올라간다)
            if (timer >= 5.0f) //5초가 지나면
            {
                //랜덤으로 위치를 바꿔주기 위해 변수를 선언한다.
                var maxX = 333.33;
                var minX = 340;
                var Y = 108;
                var maxZ = 200;
                var minZ = 198;

                transform.position = new Vector3(Random.Range(minX, (float)maxX), (float)Y, Random.Range(minZ, maxZ)); //위치를 바꿔줌
                timer = 0.0f; //timer는 다시 초기화된다.
                drop_stone = false; //false로 바꿔준다.
                down_stone = false; //false로 바꿔준다.
                StartCoroutine(Delay()); //재생성할때도 호출한다.
            }
        }
    }

    //돌이 겹쳐지지않게 하기위한 코루틴 함수
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.8f); //0.8초의 시간을 주는건 돌이 자연스럽게 흩어지는 시간을 주기위함
        GetComponent<Rigidbody>().isKinematic = true; //다시 필드에 재생성되기에 isKinematic을 줘서 움직임을 막는다.
    }

    //바닥에 굴러다니는 돌의 피격판정을 없애기위한 함수
    public void OnCollisionEnter(Collision collision)
    {
        //부딪힌 대상이 "Floor"(지면)이고 down_stone이 true이면(던져진 상태)
        if (collision.gameObject.tag == "Floor" && GetComponent<Stone>().down_stone == true)
        {
            //던져진 돌의 down_stone을 false로 바꿔서 구르는돌의 피격판정을 막는다.
            GameObject.Find("GameManager_Ch3").GetComponent<GameManager_Ch3>().iscorrect = false;
        }
    }
}
