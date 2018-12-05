/* cctvController.cs / Last modified date : 2018-08-06 / Last modifier : Yang Jae Hyeok
 * 
 *** Script Description
 * CCTV 카메라의 화면을 확인하는 오브젝트에 컴포넌트로 붙여 사용한다.
 * 특정 키를 이용해 CCTV 카메라들의 화면(시야)를 전환하여 각 CCTV 카메라의 화면을 확인하는 용도이다.
 * 
 *** 참고 사항
 * RenderTexture 변수 currentRT는 유니티 에디터에서 에셋 폴더 내부의 RenderTexture 파일을 끌어다 대입해주어야 한다.
 * 
 * 완성.(버그가 없다면...)
*/

using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider))]
public class cctvController : MonoBehaviour {
    private CapsuleCollider Coll;
    [SerializeField] private Camera[] cameras;          // CCTV 카메라 배열.
    [SerializeField] private RenderTexture currentRT;   // assets 폴더 내부의 RenderTexture. CCTV 카메라들의 targetTexture로 사용.
    private int currentIdx; // 현재 화면을 띄우는 CCTV 카메라의 Index.
    private int maxIdx;     // 카메라 배열의 사이즈.

    private FirstPersonCamera player;
    [SerializeField] private Transform gamePos;
    [SerializeField] private Transform gameCanvas;
    private bool IsGaming;
    public bool IsCleared;

    private PlayableDirector director;
    [SerializeField] private GameObject cabinet;    // 타임라인을 가지고 있는 캐비닛 GO(GameObject).
    [SerializeField] private ForTimelineScript scripts; // 타임라인용 스크립트 클래스.

    private void Start()
    {
        if (GameObject.Find("CCTVs"))
            cameras = GameObject.Find("CCTVs").transform.GetComponentsInChildren<Camera>(true);
        
        Coll = GetComponent<CapsuleCollider>();
        Coll.isTrigger = true;

        currentIdx = 0; // 기본값 0.
        maxIdx = cameras.Length;    // 배열의 길이.
        IsGaming = false;
        IsCleared = false;
        player = FirstPersonCamera.player;

        if (cameras.Length != 0)
        {
            Camera cam = cameras[0];
            if (cam.transform.parent.parent.name == "Tree_with_CCTV")
                cam.transform.parent.parent.gameObject.SetActive(true);
            else if (cam.transform.parent.name == "Tree_with_CCTV")
                cam.transform.parent.gameObject.SetActive(true);
            cam.targetTexture = currentRT;  // 기본값 지정.
        }
    }

    private void OnTriggerStay(Collider other)  // cctv Controller 콜라이더 반경 안에 있을 때.
    {
        if (!IsGaming)  // cctv 컨트롤 중이 아니라면.
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (player.getRaycastHit().collider != null && player.getRaycastHit().collider.gameObject == gameObject)
                    {
                        // cctv 컨트롤을 위한 위치 세팅.
                        IsGaming = true;
                        gameObject.tag = "Untagged";
                        player.ChangeMoveRotaState(false);
                        player.transform.SetPositionAndRotation(gamePos.position, Quaternion.AngleAxis(90f, Vector3.up));
                        player.GetComponentInChildren<Camera>().transform.localRotation = gamePos.rotation;

                        if (!IsCleared) // 미니게임을 클리어하지 않았다면.
                        {
                            SoundManager.instance.Mouse_Click();
                            gameCanvas.gameObject.SetActive(true);
                            GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>().SetScript("오우야. 이런건 내 전문이지.");
                        }
                    }
                }
            }
        }
        else
        {
            if (IsCleared)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (GameManager.gm.panelOpen == false)
                    {
                        SoundManager.instance.KeyBoard_Touch();
                        cameras[currentIdx].targetTexture = null;   // currentRT를 사용하는 기존의 카메라의 targetTexture를 제거.
                        if (cameras[currentIdx].transform.parent.parent.name == "Tree_with_CCTV")
                            cameras[currentIdx].transform.parent.parent.gameObject.SetActive(false);
                        else if (cameras[currentIdx].transform.parent.name == "Tree_with_CCTV")
                            cameras[currentIdx].transform.parent.gameObject.SetActive(false);

                        if (currentIdx == 0)
                            currentIdx = maxIdx - 1;
                        else
                            currentIdx--;

                        cameras[currentIdx].targetTexture = currentRT;  // idx 변경 후의 카메라의 targetTexture에 currentRT를 할당.
                        if (cameras[currentIdx].transform.parent.parent.name == "Tree_with_CCTV")
                            cameras[currentIdx].transform.parent.parent.gameObject.SetActive(true);
                        else if (cameras[currentIdx].transform.parent.name == "Tree_with_CCTV")
                            cameras[currentIdx].transform.parent.gameObject.SetActive(true);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    if (GameManager.gm.panelOpen == false)
                    {
                        SoundManager.instance.KeyBoard_Touch();
                        cameras[currentIdx].targetTexture = null;   // currentRT를 사용하는 기존의 카메라의 targetTexture를 제거.
                        if (cameras[currentIdx].transform.parent.parent.name == "Tree_with_CCTV")
                            cameras[currentIdx].transform.parent.parent.gameObject.SetActive(false);
                        else if (cameras[currentIdx].transform.parent.name == "Tree_with_CCTV")
                            cameras[currentIdx].transform.parent.gameObject.SetActive(false);

                        if (currentIdx == maxIdx - 1)
                            currentIdx = 0;
                        else
                            currentIdx++;

                        cameras[currentIdx].targetTexture = currentRT;  // idx 변경 후의 카메라의 targetTexture에 currentRT를 할당.
                        if (cameras[currentIdx].transform.parent.parent.name == "Tree_with_CCTV")
                            cameras[currentIdx].transform.parent.parent.gameObject.SetActive(true);
                        else if (cameras[currentIdx].transform.parent.name == "Tree_with_CCTV")
                            cameras[currentIdx].transform.parent.gameObject.SetActive(true);
                    }
                }
            } // if (IsCleared). END

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (GameManager.gm.panelOpen == false)
                {
                    if (IsCleared)
                    {
                        if (gameCanvas)
                        {
                            SoundManager.instance.KeyBoard_Touch();

                            Destroy(gameCanvas.gameObject);
                            gameCanvas = null;

                            // 게임이 클리어 되었을 때 창문 및 캐비닛과 상호작용이 가능하게끔 트리거 콜라이더 활성화.
                            if (GameObject.Find("Handle_Window"))
                            {
                                GameObject tmp = GameObject.Find("Handle_Window");
                                if (tmp.GetComponent<CapsuleCollider>())
                                    tmp.GetComponent<CapsuleCollider>().enabled = true;
                            }
                            if (cabinet)
                            {
                                cabinet.tag = "Handle";
                                if (cabinet.GetComponent<SphereCollider>())
                                    cabinet.GetComponent<SphereCollider>().enabled = true;
                            }

                            scripts.SetScript(1);
                            director = GetComponent<PlayableDirector>();
                            director.played += TimelinePlayed;
                            director.stopped += TimelineStopped;
                            director.Play();
                        }
                    }
                    else
                    {
                        gameCanvas.GetComponentInChildren<Bullet>().OnClick();
                        gameCanvas.gameObject.SetActive(false);
                    }

                    IsGaming = false;
                    gameObject.tag = "Handle";
                    player.ChangeMoveRotaState(true);
                    GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(true);
                }
            }
        }
    } // OnTriggerStay. END

    private void TimelinePlayed(PlayableDirector obj)
    {
        StartCoroutine(Timer(director));
        // 플레이어 객체의 첫번째 자식 객체(= 메인카메라)를 비활성화.
        // 2 audio 문제 때문에.
        Camera.main.GetComponent<AudioListener>().enabled = false;
        director.played -= TimelinePlayed;
    }

    private void TimelineStopped(PlayableDirector obj)
    {
        Camera.main.GetComponent<AudioListener>().enabled = true;
        director.stopped -= TimelineStopped;
    }

    private IEnumerator Timer(PlayableDirector director)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        float t = 0;
        float d = (float)director.playableAsset.duration;
        while (t < d)
        {
            t += Time.deltaTime;
            yield return wait;
        }
        director.Stop();
    }
}
