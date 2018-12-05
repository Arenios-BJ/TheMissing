using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StopGenerator : MonoBehaviour {

    [Header("Obejcts")]
    [SerializeField] private GameObject boxCover;
    [SerializeField] private GameObject redLine, blueLine, greenLine, yellowLine;
    [Header("Answers")]
    [SerializeField] private bool bRed;
    [SerializeField] private bool bBlue;
    [SerializeField] private bool bGreen;
    [SerializeField] private bool bYellow;
    [Header("Timeline")]
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject TimelineCamera;
    [SerializeField] private GameObject lastAnimTrigger;
    [SerializeField] private GameObject entranceDoor;
    [SerializeField] private ForTimelineScript scripts;
    [SerializeField] private TimelineAsset nuclear;

    public bool red, blue, green, yellow;
    private FirstPersonCamera player;

    /// <summary>
    /// 0: 상호작용 불가 상태 /
    /// 1: 전력차단기 오픈 가능 상태 /
    /// 2: 전력선 절단 가능 상태 /
    /// 8: 전력선 오절단 상태 /
    /// 9: 상호작용 완료 상태
    /// </summary>
    public int isPossible = 0;

    private void Start()
    {
        //lastAnimTrigger.SetActive(false);
        player = FirstPersonCamera.player;
        director = GetComponent<PlayableDirector>();
        TimelineAsset asset = director.playableAsset as TimelineAsset;
        director.SetGenericBinding(asset.GetOutputTrack(0), Camera.main.gameObject);
    }

    private void Update()
    {
        if (isPossible == 0)
            return;
        else if (isPossible == 8)
        {
            NuclearFire();
            isPossible = 0;
            return;
        }
        else if (isPossible == 9)
        {
            // 발전기 멈춤 타임라인 재생 및 마지막 타임라인 트리거 활성화.
            lastAnimTrigger.SetActive(true);
            isPossible = 0;
            GeneratorStop();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.gm.panelOpen == false)
            {
                RaycastHit hit = player.getRaycastHit();
                if (hit.transform == null)
                    return;

                if (hit.transform.tag == "Untagged")
                    return;

                switch (isPossible)
                {
                    case 1:
                        {
                            if (hit.transform.gameObject == boxCover)
                            {
                                boxCover.GetComponent<TransformInteraction>().Active();
                                isPossible = 2; // 전력선과 상호작용 상태로 변경.
                            }
                            break;
                        }
                    case 2:
                        {
                            if (hit.transform.gameObject == redLine)
                            {
                                hit.transform.tag = "Untagged";
                                Destroy(redLine.GetComponent<QuickOutline>(), 0.5f);
                                TransformInteraction[] action = redLine.GetComponentsInChildren<TransformInteraction>();
                                for (int i = 0; i < action.Length; i++)
                                    action[i].Active();
                                red = true;
                                SoundManager.instance.RockS();
                            }
                            else if (hit.transform.gameObject == blueLine)
                            {
                                hit.transform.tag = "Untagged";
                                Destroy(blueLine.GetComponent<QuickOutline>(), 0.5f);
                                TransformInteraction[] action = blueLine.GetComponentsInChildren<TransformInteraction>();
                                for (int i = 0; i < action.Length; i++)
                                    action[i].Active();
                                blue = true;
                                SoundManager.instance.RockS();
                            }
                            else if (hit.transform.gameObject == greenLine)
                            {
                                hit.transform.tag = "Untagged";
                                Destroy(greenLine.GetComponent<QuickOutline>(), 0.5f);
                                TransformInteraction[] action = greenLine.GetComponentsInChildren<TransformInteraction>();
                                for (int i = 0; i < action.Length; i++)
                                    action[i].Active();
                                green = true;
                                SoundManager.instance.RockS();
                            }
                            else if (hit.transform.gameObject == yellowLine)
                            {
                                hit.transform.tag = "Untagged";
                                Destroy(yellowLine.GetComponent<QuickOutline>(), 0.5f);
                                TransformInteraction[] action = yellowLine.GetComponentsInChildren<TransformInteraction>();
                                for (int i = 0; i < action.Length; i++)
                                    action[i].Active();
                                yellow = true;
                                SoundManager.instance.RockS();
                            }

                            // 클리어 조건에 부합하면 isPossible 상태 변경.
                            if (bRed == red && bBlue == blue && bGreen == green && bYellow == yellow)
                                isPossible = 9;
                            // 하나라도 잘못 입력하면.
                            else if ((!bRed && red) || (!bBlue && blue) || (!bGreen && green) || (!bYellow && yellow))
                                isPossible = 8;

                            break;
                        }
                    default:
                        break;
                } // switch(isPossible) END.
            }
        } // if(GetMouseButtonDown(0)) END.
    }

    private void NuclearFire()
    {
        scripts.SetScript(3);
        player.ChangeMoveRotaState(false);
        director.Play(nuclear);
        director.SetGenericBinding(nuclear.GetOutputTrack(0), Camera.main.gameObject);
        Invoke("gameover", 2.5f);
    }

    private void GeneratorStop()
    {
        scripts.SetScript(1);
        director.played += TimelinePlayed;
        player.ChangeMoveRotaState(false);
        director.Play();
    }

    private void TimelinePlayed(PlayableDirector obj)
    {
        StartCoroutine(Timer());

        // 챕터5 마지막 타임라인 연출을 위해 전력실 진입문의 회전값을 원복.
        Destroy(entranceDoor.GetComponent<Animator>());
        Vector3 rot = entranceDoor.transform.localRotation.eulerAngles;
        rot.y = 180f;
        entranceDoor.transform.localRotation = Quaternion.Euler(rot);
        // Outline 표시를 위해 태그 변경.
        Transform door = entranceDoor.transform.GetChild(0);
        door.tag = "Handle";
        // 트리거 콜라이더 활성화.
        BoxCollider[] col = door.GetComponents<BoxCollider>();
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].isTrigger == true)
                col[i].enabled = true;
        }
    }

    private IEnumerator Timer()
    {
        director.played -= TimelinePlayed;
        float t = 0;
        float d = (float)director.playableAsset.duration;
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        while (t < d)
        {
            t += Time.deltaTime;
            yield return wait;
        }
        director.Stop();
        player.transform.GetChild(0).gameObject.SetActive(true);
        TimelineCamera.SetActive(false);
        player.ChangeMoveRotaState(true);
    }

    void gameover()
    {
        Post_Vinet.Vinet.GameOver();
    }
}
