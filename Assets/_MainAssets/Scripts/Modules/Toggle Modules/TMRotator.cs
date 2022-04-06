using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectRotator))]
public class TMRotator : ToggleModule
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private Quaternion openedPos;
    [SerializeField]
    private Quaternion closedPos;
    [SerializeField]
    private bool useLocalRot;

    public bool playOnToggle = true;
    public bool playOnCall = false;

    private Transform obj;
    private ObjectRotator oRotator;

    private void Start()
    {
        obj = GetComponent<Transform>();
        oRotator = GetComponent<ObjectRotator>();

        if(transform.rotation == openedPos)
        {
            toggleStatus = true;
        }
        else if(transform.rotation == closedPos)
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

        ToggleObjectRotation();

        return true;
    }

    public void ToggleObjectRotation()
    {
        if (oRotator) { if (oRotator.IsCurrentlyLerping()) return; }

        if (playOnToggle)
        {
            PlaySfx();
        }

        if (!toggleStatus)
        {
            if (!useLocalRot)
            {
                oRotator.LerpRotation(openedPos, rotationSpeed);
            }
            else
            {
                oRotator.LocalLerpRotation(openedPos, rotationSpeed);
            }
            toggleStatus = true;
        }
        else
        {
            if (!useLocalRot)
            {
                oRotator.LerpRotation(closedPos, rotationSpeed);
            }
            else
            {
                oRotator.LocalLerpRotation(closedPos, rotationSpeed);
            }
            toggleStatus = false;
        }
    }

    public void DoToggle()
    {
        Toggle();
    }

    public void OnRotation()
    {
        if (oRotator.IsCurrentlyLerping()) return;
        if (!useLocalRot)
        {
            oRotator.LerpRotation(openedPos, rotationSpeed);
        }
        else
        {
            oRotator.LocalLerpRotation(openedPos, rotationSpeed);
        }

        toggleStatus = true;
        if (playOnCall)
        {
            PlaySfx();
        }
    }

    public void OffRotation() 
    {
        if (oRotator.IsCurrentlyLerping()) return;
        if (!useLocalRot)
        {
            oRotator.LerpRotation(closedPos, rotationSpeed);
        }
        else
        {
            oRotator.LocalLerpRotation(closedPos, rotationSpeed);
        }
        toggleStatus = false;
        if (playOnCall)
        {
            PlaySfx();
        }
    }
}
