using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Board_Manager : MonoBehaviour
{
    Ch6_SceneChange b_director;

    AudioSource sound;

    //private float SceneTimer;
    private FirstPersonCamera player;
    private MouseLock mouse;

    PlayableAsset win;
    PlayableAsset defeat;

    public Camera cam;
    public Text Count; //횟수를 나타내기위한 ui(Text)변수
    public Text TimeRule; //시간을 나타내기위한 ui(Text)변수
    public Text Stage; //현재 스테이지를 나타내기위한 ui(Text)변수

    public bool isWhiteTurn = true; //백돌(플레이어)턴을 나타내기 위한 bool변수

    const float Tile_Size = 1.0f; //타일크기
    const float Tile_Offset = 0.5f; //타일정중앙을 맞추기위한 값 (타일 크기는 1.0이니까)

    //selection -1은 예외처리를 위한 값. 보드판위에 뭔가를 표시할려면 무조건 양수여야만한다. -가 되면 표시못하게 예외처리를 한것.
    int SelectionX = -1; //x좌표에 대한 변수  "Board_Coordinate"함수에서 처리한다.
    int SelectionY = -1; //z좌표에 대한 변수

    public List<GameObject> ChessPrefab; //프리팹을 담을 리스트
    List<GameObject> ActiveChessman; //생성되는 GameObject들(말)을 담는 리스트
    List<GameObject> ActiveEnemy; //생성되는 적들(검은색 Pawn)을 담는 리스트
    //ActiveChessman 리스트를 원래 SpawnAllChessmans 함수안에 동적 생성 해놨었지만 전역에 선언해도 크게 문제가 없다(?)/문제있다(ㅇ)
    //SpawnAllChessmans 함수안에서 생성해야 말이 지워질때 리스트안에서도 올바르게 지워지기때문에 전역에다가 선언은 하지말자

    //{ set; get; } 이란 Property(프로퍼티)로써 private로 선언된걸 간단하게 반환(get)과 할당(set)을 하기 위함이다
    public static Board_Manager Instance { set; get; } //싱글톤개념

    //움직일 수 있는 위치를 나타내기위한 배열
    private bool[,] AllowedMoves { set; get; } //bool형 이중배열을 만들었다 말그대로 bool형의 변수들을 배열에다가 담을 수 있는것이다.

    //현재 위치를 나타내기위한 배열
    public Chessman[,] Chessmans { set; get; } //Chessman형 이중배열
    private Chessman SelectedChessman; //Chessman형 변수 (선택한말의 값을 넣는 변수)

    private Material previousMat;
    public Material selectedMat; //선택된 말의 material을 바꾸기위한 변수

    public int Clear = 1; //다음스테이지로 가기위한변수 and 현재 스테이지를 나타내는 변수
    public int Number = 10; //횟수
    public float timer = 120.0f; //각 스테이지마다 주어지는 시간제한

    Quaternion rotation = Quaternion.Euler(0, 90, 0); //Knight에 회전값을 주기위함.


    private void Start()
    {
        Debug.Log("Board_Manager Start()");
        sound = GetComponent<AudioSource>();

        b_director = GameObject.Find("Ch6_BossRoom_TimeLine").GetComponent<Ch6_SceneChange>();

        win = GameObject.Find("Ch6_BossRoom_TimeLine").GetComponent<Ch6_SceneChange>().time1;
        defeat = GameObject.Find("Ch6_BossRoom_TimeLine").GetComponent<Ch6_SceneChange>().time2;

        mouse = GameManager.gm.GetComponent<MouseLock>();

        //Clear = 1; //다음스테이지로 가기위한변수 and 현재 스테이지를 나타내는 변수
        //Number = 10; //횟수
        //timer = 120.0f; //각 스테이지마다 주어지는 시간제한
        //Instance = this; //this로 선언 시켜줘야 말을 선택할 수 있다.
        //Stage1(); //말들을 소환시키는 SpawnAllChessmans함수를 부른다.
    }

    private void OnEnable()
    {
        Clear = 1; //다음스테이지로 가기위한변수 and 현재 스테이지를 나타내는 변수
        Number = 10; //횟수
        timer = 120.0f; //각 스테이지마다 주어지는 시간제한
        Instance = this; //this로 선언 시켜줘야 말을 선택할 수 있다.
        Stage1(); //말들을 소환시키는 SpawnAllChessmans함수를 부른다.
    }

    void Update()
    {
        // 체스 게임 중일 때(체스 게임 씬이 살아있는 동안)는 무조건 마우스 언락.
        mouse.ChangeMouseLock(false);

        Count.text = "Count :" + Number.ToString(); //게임화면에 횟수를 나타내기위함
        TimeRule.text = string.Format("Time :{0:N2}", timer); //게임화면에 시간제한을 나타내기위함
        Stage.text = "Stage : " + Clear.ToString(); //게임화면에 현재 스테이지를 나타내기위함
        timer -= Time.deltaTime; //게임시작과함께 시간이 줄어든다.

        Board_Line(); //바닥을 DrawLine으로 그리는 함수
        Board_Coordinate(); //바닥 좌표를 받아온다.

        if (Input.GetMouseButtonDown(0)) //마우스 왼쪽을 눌렀을때
        {
            if (GameManager.gm.panelOpen == false)
            {
                if (SelectionX >= 0 && SelectionY >= 0) //selectionX와 Y가 0이상일때
                {
                    if (SelectedChessman == null) //selectedChessman이 null값이라면 (선택하지않았다면)
                    {
                        // Select the chessman
                        SelectChessman(SelectionX, SelectionY); //마우스로 찍은 좌표값을 SelectChessman함수로 보낸다.
                    }
                    else //null값이 아니라면 (선택되어져있다면)
                    {
                        // Move the chessman
                        MoveChessman(SelectionX, SelectionY); //마우스로 찍은 좌표값을 MoveChessman함수로 보낸다.
                    }
                }
            }
        }

        if (timer <= 0) //시간이 0보다 작거나 같으면
        {
            Defeat_ChangeScene();
        }
    }

    //말을 선택한다
    void SelectChessman(int x, int y)
    {
        //Chessman c = Chessmans[x, y]; //이걸 안넣으면 3스테이지에서 안먹어지는 버그가 일어난다 (이유는 나도 잘..)

        if (Chessmans[x, y] == null) //Chessmans배열이 null값이라면
            return;

        if (Chessmans[x, y].isWhite == false)
            return;

        bool hasAtleastOneMove = false; //false로 선언
        AllowedMoves = Chessmans[x, y].PossibleMove(); //반환받은 동적인 bool형이중배열을 bool형이중배열인 allowedMoves에 넣는다.
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (AllowedMoves[i, j])
                    hasAtleastOneMove = true; //움직일 수 있는 위치가 존재한다면 true가 된다.

        if (!hasAtleastOneMove) //false 일때
        {
            Debug.Log(hasAtleastOneMove);
            return; //밑으로 안내려가고 밖으로 벗어난다.
        }

        SelectedChessman = Chessmans[x, y]; //좌표값을 넣은 배열을 selectedChessman변수에 넣는다.
        previousMat = SelectedChessman.GetComponent<MeshRenderer>().material; //선택한 말의 material을 previousMat변수에 넣는다
        selectedMat.mainTexture = previousMat.mainTexture; //selectedMat의 텍스쳐를 previousMat의 텍스쳐(selectedChessman의 텍스쳐)로 한다.
        SelectedChessman.GetComponent<MeshRenderer>().material = selectedMat; //selectedChessman의 material을 지정해둔 selectedMat의 material로 한다.
        BoardHighlights.Instance.HighlightAllowedMoves(AllowedMoves); //움직일 수 있는 좌표값을 HighlightAllowedMoves함수에 보낸다
    }

    //말을 움직인다 (상대 말을 없애는 것도 있음)
    void MoveChessman(int x, int y)
    {
        if (AllowedMoves[x, y]) //배열안에 값이 있다면
        {
            Number--; //횟수를 깍는다
            Chessman c = Chessmans[x, y]; //Chessmans배열을(위치를) 넣은 Chessman형 변수 선언
            sound.Play();

            if (c != null && c.isWhite != isWhiteTurn)//c가 null값이 아니고 isWhite와 isWhiteTurn이 서로 다를때(흑-흑이나 백-백이아닌 상황일때)
            {   //이동한곳에 c가 있다면(흑 - 백 / 백 - 흑) 이런 상황 // isWhite는 도달한곳에 있는 말이 흑인지 백인지를 나타낸다.

                ActiveEnemy.Remove(c.gameObject); // 해당gameObject가 들어가있는 activeChessman리스트를 지운다.
                Destroy(c.gameObject); //해당gameObject를 없앤다.
                Debug.Log(ActiveEnemy.Count);
                if (Number > -1) //number가 -1보다 클때
                {
                    if (ActiveEnemy.Count == 0) //ActiveEnemy리스트안에 아무것도 없다면(체스판에 검은돌이 없다면)
                    {
                        Clear++; //Clear + 1 올린다.
                        Number = 10; //횟수를 10으로 초기화시킨다.(다음 스테이지를 위해)
                        if (Clear == 2) //1스테이지를 깼을때
                        {
                            EndGame1(); //2스테이지를 불러오는 함수를 부른다.
                            //Debug.Log(ActiveChessman.Count);
                        }
                        if (Clear == 3) //2스테이지를 깼을때
                        {
                            EndGame2(); //3스테이지를 불러오는 함수를 부른다.
                        }
                        if (Clear == 4) //3스테이지를 깼을때
                        {
                            Vic_ChangeScene();
                        }

                    }
                   
                }
            }
            if (Number == 0)
            {
                if (ActiveEnemy.Count > 0)
                {
                    Defeat_ChangeScene();
                }
            }

            Chessmans[SelectedChessman.CurrentX, SelectedChessman.CurrentY] = null; //다른 위치로 옮길 것이기 때문에 현재위치를 null로 바꿈
            SelectedChessman.transform.position = GetTileCenter(x, y); //체스말을 놓을 좌표를 잡아주는 함수에다가 이동시킬 좌표를 넣는다
            SelectedChessman.SetPosition(x, y); //선택한 말에 현재위치값을 바꿔준다(SetPosition 함수에서 바꿔준다)
            Chessmans[x, y] = SelectedChessman; //움직인 말의 현재위치를 Chessmans배열(8 x 8)에 넣는다
            //isWhiteTurn = !isWhiteTurn; //말을 움직이면 턴이 바뀐다.
        }

        SelectedChessman.GetComponent<MeshRenderer>().material = previousMat; //선택한 material을 기존걸로 바꾼다.
        BoardHighlights.Instance.Hidehighlights(); //하이라이트를 숨기는 함수를 부른다.
        SelectedChessman = null; //턴이 바뀌고 다시 null값이 된다.
    }

    //바닥을 DrawLine으로 그림
    void Board_Line()
    {
        //가로8개 세로8개
        Vector3 Width_Line = Vector3.right * 8; //가로로 길이8의 줄을 만듬
        Vector3 Height_Line = Vector3.forward * 8; //세로로 길이8의 줄을 만듬

        //for문으로 체스판을 그린다.
        for (int i = 0; i <= 8; i++) //9번돈다.
        {
            Vector3 start = Vector3.forward * i; //시작점을 z축으로 길이1마다 9개만듬
            Debug.DrawLine(start, start + Width_Line); //시작점에 가로줄을 더함.

            for (int j = 0; j <= 8; j++) //9번돈다.
            {
                start = Vector3.right * j; //시작점을 y축으로 길이1마다 9개만듬
                Debug.DrawLine(start, start + Height_Line); //시작점에 세로줄을 더함.
            }
        }

        //타일에 x표시를 DrawLine으로 나타나게함. // selection을 Draw한다.
        if (SelectionX >= 0 && SelectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * SelectionY + Vector3.right * SelectionX, Vector3.forward * (SelectionY + 1) + Vector3.right * (SelectionX + 1));
            Debug.DrawLine(Vector3.forward * (SelectionY + 1) + Vector3.right * SelectionX, Vector3.forward * SelectionY + Vector3.right * (SelectionX + 1));
        }
    }

    //타일의 좌표를 받아오는 함수(업데이트한다)
    void Board_Coordinate()
    {
        if (!cam)
            return;

        RaycastHit hit;

        //마우스 포인트를 갖다대는곳에 raycast정보를 받는다. LayerMask로 "ChessBoard"만 충돌하여 인식한다.
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessBoard")))
        {
            //raycast로 불러오는 정보중 x좌표와 z좌표를 선언해둔 selectionX와 Y변수에 넣는다.
            SelectionX = (int)hit.point.x; //SelectionX와 Y는 int형이고 Raycast로 불러오는 x값과 y값은 float형이라 int형으로 "형 변환" 필요
            SelectionY = (int)hit.point.z; //y값이 아니라 z값을 넣어주는건 3D상에서 y값은 우리가 알던 y값과는 달라 z값이 우리가 알던 
                                           //y값과 가깝다.
        }
        else //안들어오면 예외처리 한다
        {
            SelectionX = -1;
            SelectionY = -1;
        }
    }

    // Player 체스말을 생성하는 함수
    void SpawnChessman(int index, int x, int y)
    {
        //                생성한다   리스트의 번호에 맞게  위치를받아서       회전값은 고정    GameObject형태로
        GameObject go = Instantiate(ChessPrefab[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform); //transform을 이용하면 부모로 접근이 가능히다.//없어도 작동은 잘된다?
        Chessmans[x, y] = go.GetComponent<Chessman>(); //go변수를 Chessman형으로 바꿔서 2차원 배열인Chessmans에 넣기위함(?)
        Chessmans[x, y].SetPosition(x, y);
        ActiveChessman.Add(go); //생성되는 체스말을 리스트에 넣는다.
    }

    //Knight만 따로 생성하는 함수 (회전값을주기위해)
    //흰돌
    void Spawn_Knight_W(int index, int x, int y)
    {
        GameObject go = Instantiate(ChessPrefab[index], GetTileCenter(x, y), rotation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        ActiveChessman.Add(go);
    }

    // Enemy
    void SpawnEnemy(int index, int x, int y)
    {
        GameObject go = Instantiate(ChessPrefab[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform); //transform을 이용하면 부모로 접근이 가능히다.//없어도 작동은 잘된다?
        Chessmans[x, y] = go.GetComponent<Chessman>(); //go변수를 Chessman형으로 바꿔서 2차원 배열인Chessmans에 넣기위함(?)
        Chessmans[x, y].SetPosition(x, y);
        ActiveEnemy.Add(go); //생성되는 체스말을 리스트에 넣는다.
    }

    void Stage1()
    {
        ActiveEnemy = new List<GameObject>(); //플레이어가 어차피 다먹어야 다음 스테이지로 넘어가니까 따로 초기화
        ActiveChessman = new List<GameObject>(); //스테이지클리어후 말을 제거하기위해서 필요하다.
        Chessmans = new Chessman[8, 8]; //동적으로 생성하지않으면 말을 생성할 수 가없다.

        //Player(흰말)//
        //Bishop
        SpawnChessman(3, 2, 0);

        //Rook
        SpawnChessman(2, 0, 0);


        //Com(흑말)//
        //Pawn1
        SpawnEnemy(11, 1, 1);

        //Pawn2
        SpawnEnemy(11, 0, 2);

        //Pawn3
        SpawnEnemy(11, 4, 2);

        //Pawn4
        SpawnEnemy(11, 0, 5);

        //Pawn5
        SpawnEnemy(11, 2, 4);

        //Pawn6
        SpawnEnemy(11, 4, 4);

        //Pawn7
        SpawnEnemy(11, 5, 5);

        //Pawn8
        SpawnEnemy(11, 5, 7);

    }

    void Stage2()
    {
        timer = 120.0f; //다음 스테이지로 넘어가면 시간을 초기화 해야하기때문에 120초로 초기화한다
        ActiveEnemy = new List<GameObject>();
        ActiveChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        //Player(흰말)//
        //Queen1
        SpawnChessman(1, 3, 3);

        //Queen2
        SpawnChessman(1, 4, 3);


        //Com(흑말)//
        //Pawn1
        SpawnEnemy(11, 5, 0);

        //Pawn2
        SpawnEnemy(11, 2, 1);

        //Pawn3
        SpawnEnemy(11, 1, 2);

        //Pawn4
        SpawnEnemy(11, 7, 2);

        //Pawn5
        SpawnEnemy(11, 5, 3);

        //Pawn6
        SpawnEnemy(11, 7, 6);

        //Pawn7
        SpawnEnemy(11, 2, 7);

        //Pawn8
        SpawnEnemy(11, 4, 7);

        //Debug.Log(ActiveChessman.Count);
    }

    void Stage3()
    {
        timer = 120.0f;
        ActiveEnemy = new List<GameObject>();
        ActiveChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        //Player(흰말)//
        //Knight1
        Spawn_Knight_W(4, 1, 0);

        //Knight2
        Spawn_Knight_W(4, 6, 0);


        //Com(흑말)//
        //Pawn1
        SpawnEnemy(11, 4, 1);

        //Pawn2
        SpawnEnemy(11, 2, 2);

        //Pawn3
        SpawnEnemy(11, 7, 2);

        //Pawn4
        SpawnEnemy(11, 3, 3);

        //Pawn5
        SpawnEnemy(11, 5, 2); //안먹힌다. y측 3으로 바꾸면 안먹히는 버그가 생겨서 2로 바꿈.

        //Pawn6
        SpawnEnemy(11, 3, 4);

        //Pawn7
        SpawnEnemy(11, 4, 5);

        //Pawn8
        SpawnEnemy(11, 5, 5);

        //Debug.Log(ActiveChessman.Count);
    }

    //체스말을 놓을 좌표를 잡아준다.
    private Vector3 GetTileCenter(int x, int y) //SpawnAllChessmans함수안에서 주는 인자값을 받는다.
    {
        Vector3 origin = Vector3.zero; //0으로 초기화.

        //타일의 정중앙에 말을 놓아야하기때문에 TILE_OFFSET(0.5f) 값을 더한다.
        origin.x += (Tile_Size * x) + Tile_Offset;
        origin.z += (Tile_Size * y) + Tile_Offset;

        return origin;
    }

    private void EndGame1()
    {
        foreach (GameObject chess in ActiveChessman) //activeChessman리스트안에서 모든 GameObject의 이름을 임시로 go라고 정한다.
            Destroy(chess); //모든 GameObject를 지운다.
        BoardHighlights.Instance.Hidehighlights(); //하이라이트를 숨기는 함수를 부른다.

        Stage2();
    }

    private void EndGame2()
    {
        foreach (GameObject chess in ActiveChessman) //activeChessman리스트안에서 모든 GameObject의 이름을 임시로 go라고 정한다.
            Destroy(chess); //모든 GameObject를 지운다.
        BoardHighlights.Instance.Hidehighlights(); //하이라이트를 숨기는 함수를 부른다.

        Stage3();
    }

    private void Vic_ChangeScene()
    {
        b_director.director.Play(win);
        b_director.listener.enabled = true;
        GameObject.Find("Ch6_BossRoom_TimeLine").GetComponent<Ch6_SceneChange>().Chess_Check_End = true;
        GameObject.Find("Ch6_BossRoom_TimeLine").GetComponent<Ch6_SceneChange>()._vitory = true;
        SceneManager.UnloadSceneAsync("ChessGame");
    }

    private void Defeat_ChangeScene()
    {
        b_director.director.Play(defeat);
        b_director.listener.enabled = true;
        GameObject.Find("Ch6_BossRoom_TimeLine").GetComponent<Ch6_SceneChange>()._defeat = true;
        SceneManager.UnloadSceneAsync("ChessGame");
    }
}
