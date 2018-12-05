using UnityEngine;

public class Arms : MonoBehaviour
{
    public GameObject arms;

    private bool AniCheck;
    public bool CompassUseCheck;

    void Start()
    {
        arms.SetActive(false);

        AniCheck = false;
        CompassUseCheck = false;
    }

    void Update()
    {
        ArmsCheck();
    }

    void ArmsCheck()
    {
        if (CompassUseCheck == true)
        {
            if (AniCheck == false)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    arms.SetActive(true);
                    AniCheck = true;
                }
            }
            else if (AniCheck == true)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    arms.SetActive(false);
                    AniCheck = false;
                }
            }
        }
    }
}
