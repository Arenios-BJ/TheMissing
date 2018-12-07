using UnityEngine;

// 보스의 집을 순찰
// 사용한 방법 : Animator / bool / Find / FindWithTag / LookAt / SetBool / GetCurrentAnimatorStateInfo / MoveTowards / Slerp / Distance

public class MovePoint : MonoBehaviour {

    public Transform Point1;
    public Transform Point2;
    public GameObject Enemy;

    private Animator EnemyAni;

    public Transform enemyTr;
    public Transform playerTr;

    public float time;

    private bool Point1Check;
    private bool Point2Check;
    private bool IdleCheck;
    private bool DistanceCheck;

    float chaseDistance = 10.0f;

    void Start () {

        Point1Check = true;
        Point2Check = false;
        IdleCheck = false;
        DistanceCheck = false;
        EnemyAni = GameObject.Find("SkelMesh_Bodyguard_02").GetComponent<Animator>();
        playerTr = GameObject.FindWithTag("Player").transform;
    }
	
	void Update () {

        Distance();
        
        // 지정된 곳을 바라보며 이동
        if (DistanceCheck == false)
        {
            if (Point1Check == true)
            {
                enemyTr.position = Vector3.MoveTowards(enemyTr.position, Point1.position, 1.5f * Time.deltaTime);
                enemyTr.LookAt(Point1);
                EnemyAni.SetBool("PointWalk", true);

                // 지정된 곳에 도착했다면, 걷는 애니메이션을 중지한다.
                if (Enemy.transform.position == Point1.transform.position)
                {
                    EnemyAni.SetBool("PointWalk", false);
                    Point1Check = false;
                }
            }

            if (IdleCheck == false)
            {
                if (Point1Check == false)
                {
                    // 걷는 애니메이션이 중지된 후, 두리번 두리번 하는 애니메이션 실행
                    if (EnemyAni.GetCurrentAnimatorStateInfo(0).IsName("unarmed_idle_looking_ver_1"))
                    {
                        time += Time.deltaTime;

                        // 2초가 지나면 다시 걷는 애니메이션 실행
                        if (time >= 2.5f)
                        {
                            time = 0f;
                            Point2Check = true;
                            IdleCheck = true;
                        }
                    }
                }
            }

            if (Point2Check == true)
            {
                // 그리고 또 다시 지정된 위치를 바라보며 이동
                // 반복
                enemyTr.position = Vector3.MoveTowards(enemyTr.position, Point2.position, 1.5f * Time.deltaTime);
                enemyTr.LookAt(Point2);
                EnemyAni.SetBool("PointWalk", true);

                if (Enemy.transform.position == Point2.transform.position)
                {
                    EnemyAni.SetBool("PointWalk", false);
                    Point2Check = false;
                }
            }

            if (IdleCheck == true)
            {
                if (Point2Check == false)
                {
                    if (EnemyAni.GetCurrentAnimatorStateInfo(0).IsName("unarmed_idle_looking_ver_1"))
                    {
                        time += Time.deltaTime;

                        if (time >= 2.5f)
                        {
                            time = 0f;
                            Point1Check = true;
                            IdleCheck = false;
                        }
                    }
                }
            }
        }
    }

    void Distance()
    {
        // 플레이어의 위치와 적의 위치 거리를 계산해서
        float distance = Vector3.Distance(enemyTr.position, playerTr.position);

        // 플레이어가 추적 가능한 거리 안으로 들어온다면
        if (distance < chaseDistance)
        {
            // 적은 플레이어를 향해서 뛰어간다.
            DistanceCheck = true;
            enemyTr.position = Vector3.MoveTowards(enemyTr.position, new Vector3(playerTr.position.x, Enemy.transform.position.y, playerTr.position.z), 2.5f * Time.deltaTime);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, playerTr.rotation, Time.deltaTime * 3f);          
            enemyTr.LookAt(new Vector3(playerTr.position.x, Enemy.transform.position.y, playerTr.position.z));
            EnemyAni.SetBool("EnemyRun", true);

            if(distance < 3)
            {
                GameObject.Find("Post_VinetteManager").GetComponent<Post_Vinet>().GameOver();
                GameObject.Find("Post_VinetteManager").GetComponent<Post_Vinet>().text.SetActive(true);
            }
        }

        if (distance > chaseDistance) // 플레이어가 추적 가능한 거리 밖으로 나가면
        {
            // 원래의 위치로 돌아와서 다시 순찰
            DistanceCheck = false;
            EnemyAni.SetBool("EnemyRun", false);
        }
    }
}
