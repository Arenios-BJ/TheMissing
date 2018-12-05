using UnityEngine;

// 범인의 집 순찰

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

        if (DistanceCheck == false)
        {
            if (Point1Check == true)
            {
                enemyTr.position = Vector3.MoveTowards(enemyTr.position, Point1.position, 1.5f * Time.deltaTime);
                enemyTr.LookAt(Point1);
                EnemyAni.SetBool("PointWalk", true);

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
                    if (EnemyAni.GetCurrentAnimatorStateInfo(0).IsName("unarmed_idle_looking_ver_1"))
                    {
                        time += Time.deltaTime;

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
        float distance = Vector3.Distance(enemyTr.position, playerTr.position);

        if (distance < chaseDistance)
        {
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
            DistanceCheck = false;
            EnemyAni.SetBool("EnemyRun", false);
        }
    }
}
