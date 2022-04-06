using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMLight : ToggleModule
{
    public GameObject lightSource;

    public override bool Toggle()
    {
        if (!isModuleActive) return false;

        ToggleLights();

        return true;
    }

    public void ToggleLights()
    {
        if (toggleStatus)
        {
            toggleStatus = false;
            lightSource.gameObject.SetActive(false);
        }
        else
        {
            toggleStatus = true;
            lightSource.gameObject.SetActive(true);
        }
    }
}
