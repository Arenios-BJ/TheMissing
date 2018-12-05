using System.Collections;
using UnityEngine;

public class TransformInteraction : MonoBehaviour {

    [SerializeField] private bool isRotate = false;
    [SerializeField] private bool isUseParentTransform = true;
    [SerializeField] private bool isInteractedSelf = false;
    [Header("Interaction Axis")]
    [SerializeField] private bool XAxis = true;
    [SerializeField] private bool YAxis = false;
    [SerializeField] private bool ZAxis = false;
    [Header("Interaction Values")]
    [SerializeField] private float effectValue = 0f;
    [SerializeField] private float actionTime = 0f;

    private Transform parent;

    private void Start()
    {
        if (isUseParentTransform)
            parent = transform.parent;
        else
            parent = null;
    }

    // 외부에서도 사용하기 위해서 public 함수로 작동 관련 구문을 분리.
    public void Active()
    {
        gameObject.tag = "Untagged";
        if (GetComponent<QuickOutline>())
            Destroy(GetComponent<QuickOutline>());
        if (isRotate)
            StartCoroutine(Rotate());
        else
            StartCoroutine(Translate());
    }

    private IEnumerator Rotate()
    {
        Vector3 axis;
        if (XAxis)
        {
            if (YAxis)
            {
                if (ZAxis)
                    axis = Vector3.right + Vector3.up + Vector3.forward;
                else
                    axis = Vector3.right + Vector3.up;
            }
            else
            {
                if (ZAxis)
                    axis = Vector3.right + Vector3.forward;
                else
                    axis = Vector3.right;
            }
        }
        else if (YAxis)
        {
            if (ZAxis)
                axis = Vector3.up + Vector3.forward;
            else
                axis = Vector3.up;
        }
        else    // ZAxis
            axis = Vector3.forward;

        Space rotateMode;
        if (isInteractedSelf)
            rotateMode = Space.Self;
        else
            rotateMode = Space.World;

        float timer = 0f;
        WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
        while (true)
        {
            float rotatePerFrame = effectValue / (actionTime / Time.deltaTime);
            if (isUseParentTransform)
                parent.Rotate(axis, rotatePerFrame, rotateMode);
            else
                transform.Rotate(axis, rotatePerFrame, rotateMode);

            timer += Time.deltaTime;
            if (timer > actionTime)
                break;

            yield return endOfFrame;
        }
        Destroy(this);
    }

    private IEnumerator Translate()
    {
        Vector3 axis;
        if (XAxis)
        {
            if (YAxis)
            {
                if (ZAxis)
                    axis = Vector3.right + Vector3.up + Vector3.forward;
                else
                    axis = Vector3.right + Vector3.up;
            }
            else
            {
                if (ZAxis)
                    axis = Vector3.right + Vector3.forward;
                else
                    axis = Vector3.right;
            }
        }
        else if (YAxis)
        {
            if (ZAxis)
                axis = Vector3.up + Vector3.forward;
            else
                axis = Vector3.up;
        }
        else    // ZAxis
            axis = Vector3.forward;

        Space translateMode;
        if (isInteractedSelf)
            translateMode = Space.Self;
        else
            translateMode = Space.World;

        float timer = 0f;
        WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
        while (true)
        {
            float translatePerFrame = (effectValue * Time.deltaTime) / actionTime;
            if (isUseParentTransform)
                parent.Translate(axis * translatePerFrame, translateMode);
            else
                transform.Translate(axis * translatePerFrame, translateMode);

            timer += Time.deltaTime;
            if (timer > actionTime)
                break;

            yield return endOfFrame;
        }
        Destroy(this);
    }
}
