using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLerper : MonoBehaviour
{
    public bool isSmoothStep = true;
    private Transform obj;
    [SerializeField]
    private bool lerping;

    private void Start()
    {
        obj = GetComponent<Transform>();
    }

    public IEnumerator ILerpTowards(Vector3 endPos, float duration)
    {
        while (lerping) yield return new WaitForSeconds(0.1f);
        lerping = true;
        Vector3 startPosition = obj.transform.position;
        Vector3 endPosition = endPos;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float smooth = t / duration;
            smooth = smooth * smooth * (3f - 2f * smooth);
            obj.transform.position = Vector3.Lerp(startPosition, endPosition, smooth);
            yield return null;
        }
        obj.transform.position = endPos;
        lerping = false;
        yield break;
    }

    private IEnumerator ILocalLerpTowards(Vector3 endPos, float duration)
    {
        while (lerping) yield return new WaitForSeconds(0.1f);
        lerping = true;
        Vector3 startPosition = obj.transform.localPosition;
        Vector3 endPosition = endPos;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float smooth = t / duration;
            smooth = smooth * smooth * (3f - 2f * smooth);
            obj.transform.localPosition = Vector3.Lerp(startPosition, endPosition, smooth);
            yield return null;
        }
        obj.transform.localPosition = endPos;
        lerping = false;
        yield break;
    }

    public void LerpTowards(Vector3 endPos, float duration)
    {
        StartCoroutine(ILerpTowards(endPos, duration));
    }
    
    public void LocalLerpTowards(Vector3 endPos, float duration)
    {
        StartCoroutine(ILocalLerpTowards(endPos, duration));
    }

    public bool IsCurrentlyLerping()
    {
        return lerping;
    }

     public void StopMoveLerp()
    {
        lerping = false;
    }
}
