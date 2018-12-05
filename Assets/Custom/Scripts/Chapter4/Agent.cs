using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour {

    // 적이 순찰 도는 지점을 리스트에 저장
    public List<Transform> wayPoints;

    // 리스트 번호
    public int nextIdx;

    // 적이 순찰 도는 지점을 모아놓은 상위 오브젝트
    private GameObject group;

    private NavMeshAgent agent;

    // 플레이어 위치
    public Transform playerTr;

    // 적과 플레이어 사이의 거리
    public float distance;

    // 플레이어가 적의 눈에 들어오는 거리 
    float chaseDistance = 10.0f;

    // 플레이어가 벗어나는 거리
    float reChaseDistance = 7.0f;

    float moveSpeed = 5.0f;

    // 적의 위치
    public Transform EnemyTr;

    void Start () {

        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); // 플레이어의 위치를 playerTr에 담아주고

        agent = GetComponent<NavMeshAgent>();
        group = GameObject.Find("_4th/Point");

        if (group != null)
        {
            // Point폴더 아래에 있는 모든 컴포넌트를 추출
            // wayPoints에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);
        }
        MovePoint();
    }

	void Update () {

        // sqrMagnitude -> 벡터의 길이에 제곱한 값을 반환
        // remainingDistance -> 목적지까지 남은 거리 반환
        // NavMeshAgent가 이동하고 있고 목적지에 도착했는지 계산
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
           && agent.remainingDistance <= 0.5f)
        {
            // 몇번째 배열 값에 접근할 것인지 판단
            // 1씩 증가시키고 마지막 배열을 참조하면 다시 첫 번째 배열 값을 가리키도록 나머지 연산을 사용해 순환
            nextIdx = ++nextIdx % wayPoints.Count;
            MovePoint();
        }

        GetDistanceFromPlayer();
    }

    public void MovePoint()
    {
        // wayPoints(적이 순찰 다닐 곳) [nextIdx] 순찰할 지점
        // 순잘할 지점의 위치를 넘겨주면 적이 그 곳에 순찰을 다닌다.
        agent.destination = wayPoints[nextIdx].position; // 포인트 위치를 넘겨준다.

        //true설정하면 NavMesh 에이전트의 이동이 현재 경로에서 중지된다. false로 설정하면 현재 경로를 따라 이동을 다시 시작.
        agent.isStopped = false;
    }

    public float GetDistanceFromPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerTr.position); // 적과 플레이어의 거리를 구해서 distance에 넣는다.

        // 바로 위에서 계산한 거리가 chaseDistance(10.0f)보다 작다면
        // (플레이어가 추적 거리 계산 안으로 들어왔다면)
        if (distance < chaseDistance)
        {
            // 플레이어의 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, playerTr.position, moveSpeed * Time.deltaTime);

            // 적 오브젝트의 애니메이션 상태 변경 (flase = run상태)
            //GameObject.Find("Enemy").GetComponent<Enemy_Ani>().enemy_ani.SetBool("IsMove", false);

            // 가야할 방향을 플레이어의 위치로 변경
            agent.destination = playerTr.position;

            // desiredVelocity 속성은 NavMashAgent가 이동할 때 다음 목적지로 향하는 속도를 의미
            Quaternion qut = Quaternion.LookRotation(agent.desiredVelocity);

            //Slerp를 쓰면 몸을 서서히 돌리는 것처럼 보인다
            EnemyTr.rotation = Quaternion.Slerp(EnemyTr.rotation, qut, Time.deltaTime * 3f);
        }

        if (distance > reChaseDistance) // 플레이어가 추적 가능한 거리 밖으로 나가면
        {
            // 적 오브젝트의 애니메이션 상태 변경 (true = walk상태)
            //GameObject.Find("Enemy").GetComponent<Enemy_Ani>().enemy_ani.SetBool("IsMove", true);
            agent.destination = wayPoints[nextIdx].position; // 포인트 위치를 넘겨준다.
        }

        return distance; // 구한거리를 되돌려준다.
    }

}
