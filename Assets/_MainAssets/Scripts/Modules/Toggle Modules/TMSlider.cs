using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectLerper))]
public class TMSlider : ToggleModule
{
    [SerializeField]
    private float drawSpeed;
    [SerializeField]
    private Vector3 openedPos;
    [SerializeField]
    private Vector3 closedPos;

    private Transform obj;
    private ObjectLerper oLerper;

    
    

    private void Start()
    {
        obj = GetComponent<Transform>();
        oLerper = GetComponent<ObjectLerper>();
        if (closedPos == Vector3.zero)
        {
            closedPos = GetComponent<Transform>().position;
        }

        if (transform.localPosition == openedPos)
        {
            toggleStatus = true;
        }
        else if (transform.localPosition == closedPos)
        {
            toggleStatus = false;
        }
        else
        {
            toggleStatus = false;
        }
    }

    public override bool Toggle()
    {
        if (!base.Toggle()) return false;

        DoSlide();

        return true;
    }
    
    public void DoSlide() 
    {
        if (oLerper) { if (oLerper.IsCurrentlyLerping()) return; }

        PlaySfx();

        if (!toggleStatus)
        {
            oLerper.LocalLerpTowards(openedPos, drawSpeed);
            toggleStatus = true;
        }
        else
        {
            oLerper.LocalLerpTowards(closedPos, drawSpeed);
            toggleStatus = false;
        }
    }

    public void SlideClose()
    {
        if (toggleStatus)
        {
            PlaySfx();
            oLerper.LocalLerpTowards(closedPos, drawSpeed);
            toggleStatus = false;
        }
    }
}
