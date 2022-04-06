using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ITFacemask : Item
{
    public Transform obj;
    public Transform TargetParent;
    public float UseSpeed;
    public Vector3 UseLocalPos;
    public Quaternion UseRotation;
    public float ScaleFactor = 1;

    public UnityEvent OnUse;

    private ObjectLerper oLerper;
    private ObjectRotator oRotator;


    public void Start()
    {
        obj = GetComponent<Transform>();
        if (TargetParent)
        {
            TargetParent = Camera.main.transform;
        }
        oLerper = GetComponent<ObjectLerper>();
        oRotator = GetComponent<ObjectRotator>();
    }

    public void WearFacemask()
    {
        obj.SetParent(TargetParent);

        if (oLerper)
        {
            oLerper.LocalLerpTowards(UseLocalPos, UseSpeed);
        }

        if (oRotator)
        {
            oRotator.LocalLerpRotation(UseRotation, UseSpeed);
        }

        if (!sLerping)
        {
            StartCoroutine(ILerpScale(new Vector3(transform.localScale.x * ScaleFactor, transform.localScale.y * ScaleFactor, transform.localScale.z * ScaleFactor), UseSpeed));
        }

        obj.GetComponent<Collider>().enabled = false;

        if (obj.GetComponent<Interactable>())
        {
            obj.GetComponent<Interactable>().isInteractable = false;   
            obj.GetComponent<Interactable>().isHighlightable = false;   
        }


        OnUse.Invoke();

    }

    public void FaceMaskOn()
    {
        obj.SetParent(TargetParent);
        obj.localPosition = UseLocalPos;
        obj.localRotation = UseRotation;

        obj.GetComponent<Collider>().enabled = false;

        if (obj.GetComponent<Interactable>())
        {
            obj.GetComponent<Interactable>().isInteractable = false;
        }

        OnUse.Invoke();
    }

    public override bool UseItem()
    {
        if (!base.UseItem()) return false;

        WearFacemask();
        //FaceMaskOn();

        return true;
    }

    private bool sLerping;

    private IEnumerator ILerpScale(Vector3 eScale, float duration)
    {
        while (sLerping) yield return new WaitForSeconds(0.1f);
        sLerping = true;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = eScale;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float smooth = t / duration;
            smooth = smooth * smooth * (3f - 2f * smooth);
            transform.localScale = Vector3.Lerp(startScale, endScale, smooth);
            yield return null;
        }
        transform.localScale = endScale;
        sLerping = false;
        yield break;
    }
}
