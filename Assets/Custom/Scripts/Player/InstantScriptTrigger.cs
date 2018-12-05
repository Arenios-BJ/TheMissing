#define ISTRIGGER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InstantScriptTrigger : MonoBehaviour {

    [Tooltip("화면에 출력할 스크립트(string)")]
    [SerializeField] private string text = "";
    [Tooltip("물리적 콜라이더인지, 트리거 콜라이더인지.")]
    [SerializeField] private bool isTrigger = false;
    [Tooltip("Scene 로드 시, 활성화 상태 유무.")]
    [SerializeField] private bool isStarted = false;
    [Tooltip("이 트리거를 1회용으로 사용할 것인가? True = 사용 후 제거")]
    [SerializeField] private bool isTemporary = false;

    private Collider coll;
    private ObjectScripts scripts;

    private void Start()
    {
        scripts = GameObject.Find("ScriptsManager").GetComponent<ObjectScripts>();
        coll = GetComponent<Collider>();
        if (isTrigger)
            coll.isTrigger = true;
        else
            coll.isTrigger = false;

        if (isStarted)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            scripts.SetScript(text);

        if (isTemporary)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            scripts.SetScript(text);

        if (isTemporary)
            Destroy(gameObject);
    }
}
