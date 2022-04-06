using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggler : ToggleModule
{
    public List<ToggleModule> Modules = new List<ToggleModule>();

    public void ToggleModules()
    {
        foreach(ToggleModule tm in Modules)
        {
            tm.Toggle();
        }
        toggleStatus = GetStatus();
    }

    public void ToggleFalse()
    {
        if (toggleStatus)
        {
            ToggleModules();
        }
    }

    public void Start()
    {
        toggleStatus = GetStatus();
    }

    public bool GetStatus()
    {
        bool trueState = false;
        foreach (ToggleModule tm in Modules)
        {
            if (tm.toggleStatus == true)
            {
                trueState = true;
            }
        }
        return trueState;
    }
}
