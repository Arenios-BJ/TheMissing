using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

// PostProcessing, 비네트와 관련된 스크립트
// 플레이어가 죽었을 때 효과를 위함
// 사용한 방법 : Post_Vinet / PostProcessingBehaviour / VignetteModel.Settings / Light / bool / Find / enabled / SetActive / color

public class Post_Vinet : MonoBehaviour {

    public static Post_Vinet Vinet;
    private PostProcessingBehaviour beHavior;
    private VignetteModel.Settings vig;

    public GameObject text;
    public Light worldRight;
    public GameObject loading;

    private bool PostCheck;

	void Start () {
        Vinet = this;
        beHavior = GameObject.Find("MainCamera").GetComponent<PostProcessingBehaviour>();
        
        beHavior.profile.vignette.enabled = false;

        text.SetActive(false);
        loading.SetActive(false);

        vig.center.x = 0.5f;
        vig.center.y = 0.5f;

        vig.smoothness = 1;
        vig.roundness = 1;

        vig.color = new Color32(29, 4, 4, 255);

        PostCheck = false;
    }
	
	void Update () {

        if (PostCheck == false)
        {
            if (beHavior.profile.vignette.enabled == true)
            {
                vig.intensity += Time.deltaTime/2;
                beHavior.profile.vignette.settings = vig;
            }

            if(vig.intensity >= 1)
            {
                PostCheck = true;
                Time.timeScale = 0f;
                StartCoroutine(Timer());
            }
        }
	}

    public void GameOver()
    {
        beHavior.profile.vignette.enabled = true;
        text.SetActive(true);
        worldRight.intensity = 2f;
    }

    // 비네트가 완전히 활성화되고 2초 후 씬 전환.
    private IEnumerator Timer()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(2f);

        yield return wait;
        wait = null;

        Time.timeScale = 1f;
        text.SetActive(false);
        loading.SetActive(true);
        beHavior.profile.vignette.enabled = false;
        GameManager.gm.GetComponent<MouseLock>().ChangeMouseLock(false);
        GameManager.gm.LoadSavedScene(PlayerPrefs.GetInt("Chapter"));
    }
}
