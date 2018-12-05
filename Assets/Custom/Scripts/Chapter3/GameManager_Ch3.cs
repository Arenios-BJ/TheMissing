using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Ch3 : MonoBehaviour {

    public Stone stone; //Stone스크립트형 변수선언

    public Inventory throwStone; //Inventory스크립트형 변수선언 //Items리스트안에 있는 아이템을 불러오기위함
    public Transform Stone_Spawn; //Transform형 변수선언 //돌이 던져질 위치를 가지고 있는 빈오브젝트를 부르기위함
    public GameObject Stone_Prefab; //CreatePool함수에서 Stone프리팹을 불러오기위한 변수 Inspector에서 드래그로 넣어줄것이다.
    int MaxPool = 10; //CreatePool함수에서 돌을 생성할때 최대수치를 정해놓은 변수
    public List<GameObject> StonePool = new List<GameObject>(); //바닥에 깔아놓을 돌을 생성하기위한 오브젝트풀 리스트
    public List<GameObject> MyStone = new List<GameObject>(); //생성된 돌을 먹게되면 MyStone리스트에 들어온다

    public bool stone_button = false; //ItemFunctions 스크립트에서 true로 바꿔준다(인벤토리창에서 사용하기를 누르면 true로 바뀐다)
    float power = 2000.0f; //돌에 힘을 주기위한 변수

    public bool iscorrect; //Enemy_Navi에서 예외처리로 이용하기위한 변수


    private void Awake()
    {
        CreatePool(); //돌을 생성하기위한 함수
        Stone_Spawn = GameObject.Find("Stone_Spawn").GetComponent<Transform>(); //돌이 던져질 위치를 가지고 있는 빈오브젝트를 불러옴
        throwStone = GameObject.Find("Inventory").GetComponent<Inventory>(); //Inventory스크립트를 불러온다.
    }

    void CreatePool()
    {
        GameObject objectpools = new GameObject("objectPools"); //생성된 돌을 담기위한 빈 오브젝트
        for (int i = 0; i < MaxPool; i++) //MaxPool수치만큼 i를 돌린다.
        {
            //일정 범위안에 랜덤으로 돌을 생성하기위한 변수들이다.
            var maxX = 333.33;
            var minX = 340;
            var Y = 108.5;
            var maxZ = 200;
            var minZ = 198;

            //         생성함      자료형     생성할원본    objectpools부모에게 상속시킨다
            var obj = Instantiate<GameObject>(Stone_Prefab, objectpools.transform);
            obj.transform.position = new Vector3(Random.Range(minX, (float)maxX), (float)Y, Random.Range(minZ, maxZ)); //생성될 위치를 정해둔 수치사이에 
            obj.name = "Item_Stone";//생성될 돌의 name을 정한다                                         //랜덤으로 생성시킨다.
                                    //obj.SetActive(true); 
                                    //obj.GetComponent<Rigidbody>().isKinematic = true; //생성될때 돌이 멋대로 움직이는것을 막기위한것이다.//isKinematic을 true로 하면 힘 충돌 물리에 관한것들이 강체(Rigidbody)에 영향을 못준다.

            StonePool.Add(obj); //생성된 돌을 StonePool리스트에 넣는다 //이 리스트가 없어도 작동이 잘된다???
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1)) //마우스 오른쪽을 누르면
        {
            if (GameManager.gm.panelOpen == false && GameObject.Find("Inventory").GetComponent<Inventory>().IsOpened == false)
            {
                if (stone_button == true) //사용하기를 눌렀으면 true가 되어있을것이다.
                {
                    //Inventory스크립트에서 필드에 있는 돌을 먹으면 해당 돌을 MyStone 리스트에 집어 넣는다
                    if (MyStone.Count > 0) //MyStone리스트안에 돌의 갯수가 0보다 크면
                    {
                        MyStone[0].GetComponent<Rigidbody>().isKinematic = false; //true상태로 유지해놓으면 공이 스폰위치에 생성된 상태에서 움직이질않는다
                        MyStone[0].SetActive(true); //MyStone 0번째(첫번째)의 돌이 활성화된다.
                        MyStone[0].transform.position = Stone_Spawn.position; //돌의 위치를 Stone_Spawn 위치로 바꾼다
                        MyStone[0].transform.rotation = Stone_Spawn.rotation; //돌의 회전을 Stone_Spawn 회전으로 바꾼다.
                        Vector3 velocity = (Camera.main.transform.forward) * power; //돌에 AddForce를 하기위한 Vector3 변수를 만든다.
                        MyStone[0].GetComponent<Rigidbody>().AddForce(velocity); //velocity를 AddForce에 넣어 MyStone[0]번째 돌에 적용시킨다.
                        MyStone[0].GetComponent<Stone>().drop_stone = true; //각각의 돌에는 Stone스크립트가 들어있기에 해당 돌의 Stone스크립트를 불러와 bool을 바꿔준다.
                        MyStone[0].GetComponent<Stone>().down_stone = true; //구르고있는 돌에 대한 피격판정을 막을 bool변수
                        iscorrect = MyStone[0].GetComponent<Stone>().down_stone; //Enemy_Navi에서 예외처리로 이용하기위해 iscorrect변수에 넣어준다.(이렇게안하면 제대로 적용못함)
                        Debug.Log(iscorrect);
                        StonePool.Add(MyStone[0]); //던진 돌을 StonePool리스트에 넣는다(그전에 돌을 먹으면 Inventory스크립트에서 remove를 해놓기 때문에 다시 추가하는것)//하지만 이 리스트가 없어도 작동이 잘된다??
                        MyStone.Remove(MyStone[0]); //MyStone[0]에서 돌을 던졌기때문에 MyStone에서 Remove시킨다.

                        //인벤토리 스크립트에 있는 items리스트중에서 GameObject형을 모두 임시로 item이라고 이름을 정한다.
                        foreach (GameObject item in throwStone.items.ToArray()) //리스트를 ToArray를 이용해 배열로 만들어 오류를 수정함
                        {
                            if (item.gameObject.name == "Item_Stone") //item의 이름이 Item_Stone이라면
                            {
                                int count = int.Parse(item.transform.Find("Text").GetComponent<Text>().text); //해당 아이템의 갯수를 불러온다.
                                if (count > 0) //갯수가 0보다 크면
                                {
                                    count--; //카운트 -1시켜준다.
                                    item.transform.Find("Text").GetComponent<Text>().text = count.ToString(); //count를 갱신화시켜준다.
                                }
                                if (count <= 0) //0이 더 크다면
                                {
                                    throwStone.items.Remove(item); //리스트에서 지운다
                                    Destroy(item); //아이콘을 지운다.
                                }
                            }
                        }
                    }

                    if (MyStone.Count == 0) //MyStone리스트의 갯수가 0이라면
                    {
                        stone_button = false;
                    }
                }
            }
        }
    }
}
