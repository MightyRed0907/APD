using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMToggle : GPhaseModule
{
    public bool DisableOnToggle;
    public ToggleModule ToggleModule;
    //[HideInInspector]
    public bool finishedState;

    public void Start()
    {
        SetStartingStatus();
    }

    public override bool BSetModuleStatus()
    {
        if (!base.BSetModuleStatus()) return false;
        SetStatus();
        if(phaseParent) 
        {
            phaseParent.CheckPhaseStatus();
            if (phaseParent.stageParent.IsSequential)
            {
                if (phaseParent.IsSequential || DisableOnToggle)
                {
                    SetInteractablesState(false);
                }
                //phaseParent.CheckPhaseStatus();
                phaseParent.UnlockNextModule(this);
            }
        }
        
        return true;
    }

    public virtual void SetStartingStatus()
    {
        IsFinished = ToggleModule.toggleStatus;
        finishedState = false;
    }

    public virtual void SetStatus()
    {
        if (IsFinished) return;
        IsFinished = !ToggleModule.toggleStatus;
        OnFinish.Invoke();
    }

    public override int GetScorePoints()
    {
        int sPoint = 0;

        if (ToggleModule.toggleStatus == finishedState)
        {
            sPoint = 1;
        }
        else
        {
            sPoint = 0;
        }

        return sPoint;
    }
}
