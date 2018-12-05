using UnityEngine;

public class FenceAni : MonoBehaviour {

    public Animator ani;
    public AudioSource FenceSound;

	void Start () {

        ani.enabled = false;
        FenceSound.enabled = false;

    }
	
}
