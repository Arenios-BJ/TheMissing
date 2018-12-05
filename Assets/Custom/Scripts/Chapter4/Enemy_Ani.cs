using UnityEngine;

public class Enemy_Ani : MonoBehaviour {

    public Animator enemy_ani;

    void Start () {

        enemy_ani = GetComponent<Animator>();

        enemy_ani.SetBool("IsMove", true);
    }
}
