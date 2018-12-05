using UnityEngine;

public class Attack_Door : MonoBehaviour {

    public AudioSource Audio;

	void Start () {
        Audio = GetComponent<AudioSource>();
        Audio.Play();
	}

    public void PlaySound ()
    {
        Audio.Play();
    }
}
