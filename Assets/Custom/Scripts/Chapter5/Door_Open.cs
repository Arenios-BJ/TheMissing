using UnityEngine;

public class Door_Open : MonoBehaviour
{
    [SerializeField] Door_Parent parent;
    [SerializeField] AnimationClip openClip;
    Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        OpenDoor();
    }

    void OpenDoor()
    {
        if (parent.D_1 == true && parent.D_2 == true && parent.D_3 == true && parent.D_4 == true && parent.D_5 == true
            && parent.D_6 == true && parent.D_7 == true && parent.D_8 == true && parent.D_9 == true && parent.D_10 == true
            && parent.DX_1 == false && parent.DX_2 == false && parent.DX_3 == false && parent.DX_4 == false && parent.DX_5 == false && parent.DX_6 == false)
        {
            if (!anim.isPlaying)
            {
                // 문 열리는 애니메이션 클립.
                anim.clip = openClip;
                anim.Play();

                SoundManager.instance.ChainDoor_Open();

                // 관련된 일회성 스크립트들 삭제.
                parent.removeChildrenScript();
                gameObject.tag = "Untagged";
                Destroy(GetComponent<QuickOutline>());
                Destroy(parent);
                Destroy(this);
            }

        }
    }
}
