using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Navi : MonoBehaviour
{
    // 적이 순찰 도는 지점을 리스트에 저장
    public List<Transform> point;

    // 리스트 번호
    public int index;

    // 적이 순찰 도는 지점을 모아놓은 상위 오브젝트
    GameObject group;

    //돼지 오브젝트를 지우기위한 Transform형 변수
    public Transform finish;

    //네비를 이용하기위한 NavMeshAgent형 변수
    NavMeshAgent nav;

    bool attack = false; //돌에 맞았는지를 나타내는 bool형 변수
    bool looknjoy = false; //돼지와 내가 가까워졌음을 나타내는 bool형 변수
    public bool OnlyOne = false; //애니메이션 동작인 "Joy"를 단 한번 실행시키기위한 bool형 변수
                                 //한번만 실행시킴으로서 HitnRun함수를 한번만 부른다.
    //Animator와 관련된 기능을 부르기위한 변수
    Animator anim; 

    //플레이어의 위치를 부르기 위한 변수 
    public Transform Player;

    public float timer; //Pig_Canvas를 부르기위한 시간변수
    public float timer_run; //애니메이션 동작인 "MotionChange"를 자연스럽게 부르기위한 시간변수

    // 플레이어가 적의 눈에 들어오는 거리 
    float chaseDistance = 7.0f;

    // 돼지가 도망간 후 등장할 석판
    public GameObject circle;

    void Start()
    {
        
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //플레이어위치를 불러온다

        anim = GetComponent<Animator>(); //Animator를 불러온다

        group = GameObject.Find("Pig_Point"); //돼지가 움직일 경로지점을 모아둔 부모오브젝트를 불러온다

        finish = GameObject.FindGameObjectWithTag("Finish").transform; //빈오브젝트인"Finish"의 위치를 가져온다.

        nav = GetComponent<NavMeshAgent>(); //NavMeshAgent를 불러온다.

        if (group != null) //group이 존재한다면 (null값이라는게 거짓이라면)
        {
            group.GetComponentsInChildren<Transform>(point); //group(부모오브젝트)의 자식들을 point리스트에 넣는다.
            point.RemoveAt(0); //0번째를 제거하지않으면 처음에 전혀 다른위치로 이동하게되니 꼭 제거해주자.
        }
        MovePoint(); //경로대로 움직이기위한 MovePoint함수를 부른다.

        OnlyOne = true; //한번만 실행시키기위해 true로 선언한다.

        circle.SetActive(false); // 석판은 처음에 보이지 않는다.
    }

    //경로대로 움직이기위한 MovePoint함수
    void MovePoint()
    {
        nav.destination = point[index].position; //nav의 목적지를 point[index]의 position으로 한다.
    }

    void Update()
    {
        // sqrMagnitude -> 벡터의 길이에 제곱한 값을 반환
        // remainingDistance -> 목적지까지 남은 거리 반환
        // NavMeshAgent가 이동하고 있고 목적지에 도착했는지 계산
        if (nav.velocity.sqrMagnitude >= 0.2f * 0.2f && nav.remainingDistance <= 0.5f)
        {
            // 몇번째 배열 값에 접근할 것인지 판단
            // 1씩 증가시키고 마지막 배열을 참조하면 다시 첫 번째 배열 값을 가리키도록 나머지 연산을 사용해 순환
            index = ++index % point.Count;
            
            MovePoint(); //계속 부른다.
        }

        //돼지가 놀라거나 돌에 맞았을때
        if (GetComponent<BoxCollider>().isTrigger == true)
        {
            timer += Time.deltaTime;
            //timer가 2초지났다면
            if (timer >= 2.0f)
            {
                //canvas를 켜서 대사를 불러온다
                GameObject.Find("Pig_Script_Zone").GetComponent<Pig_Script>().Pig_Canvas.SetActive(true);
                //시간을 초기화한다.
                timer = 0.0f;
                // 석판을 보이게 한다.
                circle.SetActive(true); 
            }
        }

        if (anim.GetBool("Hit") == true) //애니메이션동작"Hit"(bool형)가(돌에 맞는 모션) 동작했다면
        {
            HitnRun(); //도망가는 모션인 HitnRun함수를 부른다.
        }

        if (anim.GetBool("Joy") == true) //애니메이션동작"Joy"(bool형)가(깜짝놀라는 모션) 동작했다면
        {
            HitnRun();
        }

        Distance(); //플레이어와 돼지의 거리를 구하는함수
    }

    //돌에 맞는걸 구현하기위한 함수
    private void OnCollisionEnter(Collision collision)
    {
        //부딪힌 대상이(가해자가) "Item_Stone" 이라면
        if (collision.gameObject.name == "Item_Stone")
        {
            //down_stone이 GameManager_Ch3(돌 던지는 코드가있음)에서 true가 되고 거기서 iscorrect변수에 넣는다(따로 변수에 안넣으면 작동안됨)
            if (GameObject.Find("GameManager_Ch3").GetComponent<GameManager_Ch3>().iscorrect == true)
            {
                SoundManager.instance.Stone_Shot();
                GameObject.Find("GameManager_Ch3").GetComponent<GameManager_Ch3>().iscorrect = false; //예외처리
                anim.SetBool("Hit", true); //애니메이션"Hit"(bool형)를 true로 바꾼다
                attack = true; //맞았는지를 확인하는 bool형 attack을 true로 바꾼다.
                SoundManager.instance.Pig_BeShot();
            }
        }
    }

    //돼지가 목적지에 도달해서 사라지게 하기위한 함수
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish") //도달한 오브젝트의 태그가 "Finish"라면
        {
            Destroy(gameObject, 1.0f); // 1초후에 삭제한다. 
        }
    }

    //플레이어와 돼지가 가까워지면 돼지가 놀라는 동작을 하게 하기위한 함수
    public float Distance()
    {
        //돼지의 현재 위치와 플레이어의 현재위치를 더한 값을뺀 나머지 길이를 나타낸다
        float distance = Vector3.Distance(transform.position, Player.position);
        // 플레이어와 돼지의 거리를 나타낸다고 생각하면된다

        //돼지와 플레이어간의 거리가 미리 설정해둔 값(7.0f) 보다 적다면
        if (distance < chaseDistance)
        {
            looknjoy = true; //가까워졌는지를 나타내는 bool형 변수
            if (OnlyOne == true) //start함수에서 true로 해뒀기때문에 바로 들어온다.
            {
                transform.LookAt(Player); //돼지가 플레이어를 바라본다.
                anim.SetBool("Joy", true); //깜짝놀라는 동작을 취한다.
                OnlyOne = false; //다시 부르지않기위해 예외처리를 한다.
                SoundManager.instance.Pig_BeShot();
            }
        }

        return distance; //distance값을 반환한다.
    }

    //돌에 맞거나 깜짝놀란후에 목적지까지 도망가게 하기위한 함수
    public void HitnRun()
    {
        timer_run += Time.deltaTime; //"MotionChange"를 자연스럽게 부르기위한 시간변수 
        nav.enabled = false; //nav를 끊다, nav를 안끄면 nav에서 지정해둔 경로로 계속 이동하면서 동작들을(깜짝놀라거나, 맞는모션)
                             //보여주기때문에 이상하게 보인다.
        //동작을 자연스럽게 보여주기위한 1.2초
        if (timer_run >= 1.2f) //1.2초보다 커지면
        {
            nav.enabled = true; //nav를 다시 켠다
            anim.SetTrigger("MotionChange"); //도망가는 동작인 "MotionChange"를 SetTrigger로 부른다
            timer_run = 0.0f; //예외처리를 위한 초기화
            SoundManager.instance.Pig_Run();

            if (attack == true || looknjoy == true) //둘중 하나가 true라면
            {
                //업데이트함수에서 HitnRun를 부르지않기위해
                if (anim.GetBool("Hit") == true)
                {
                    anim.SetBool("Hit", false); //false로 바꿈
                }
                if (anim.GetBool("Joy") == true)
                {
                    anim.SetBool("Joy", false); //false로 바꿈
                }

                nav.SetDestination(finish.transform.position); //목적지로 보내기위한 경로설정
                nav.speed = 4f; //도망가는 것이기 때문에 속도를 4로 맞춘다
                GetComponent<BoxCollider>().isTrigger = true; //언덕으로 올라가기위해서 isTrigger를 true로 바꿔줘야한다
            }   // isTrigger를 체크안하면 collider가 terrain에 걸려서 언덕을 못올라간다.         
        }
    }
}
