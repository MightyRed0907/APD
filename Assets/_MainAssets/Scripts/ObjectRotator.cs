using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    private Transform obj;
    [SerializeField]
    private bool lerping;


    private void Start()
    {
        obj = GetComponent<Transform>();
    }

    public IEnumerator ILerpRotation(Quaternion endRot, float duration)
    {
        while (lerping) yield return new WaitForSeconds(0.1f);
        lerping = true;
        Quaternion startRot = obj.transform.rotation;
        Quaternion endRotation = endRot;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float smooth = t / duration;
            smooth = smooth * smooth * (3f - 2f * smooth);
            obj.transform.rotation = Quaternion.Lerp(startRot, endRotation, smooth);
            yield return null;
        }
        obj.transform.rotation = endRot;
        lerping = false;
        yield break;
    }
    
    public IEnumerator ILocalLerpRotation(Quaternion endRot, float duration)
    {
        while (lerping) yield return new WaitForSeconds(0.1f);
        lerping = true;
        Quaternion startRot = obj.transform.localRotation;
        Quaternion endRotation = endRot;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float smooth = t / duration;
            smooth = smooth * smooth * (3f - 2f * smooth);
            obj.transform.localRotation = Quaternion.Lerp(startRot, endRotation, smooth);
            yield return null;
        }
        obj.transform.localRotation = endRot;
        lerping = false;
        yield break;
    }

    public void LocalLerpRotation(Quaternion endRot, float duration)
    {
        StartCoroutine(ILocalLerpRotation(endRot, duration));
    }
    public void LerpRotation(Quaternion endRot, float duration)
    {
        StartCoroutine(ILerpRotation(endRot, duration));
    }

    public bool IsCurrentlyLerping()
    {
        return lerping;
    }

    public void StopRotLerp()
    {
        lerping = false;
    }
}
