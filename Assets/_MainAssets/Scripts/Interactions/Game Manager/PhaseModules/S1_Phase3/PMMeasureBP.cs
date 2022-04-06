using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMMeasureBP : GPhaseModule
{
    public ITBPMonitor bpMonitor;

    public void CheckBPModuleStatus()
    {
        if (IsFinished) return;
        IsFinished = true;
        BSetModuleStatus();
        if (phaseParent.IsSequential && phaseParent.stageParent.IsSequential)
        {
            phaseParent.UnlockNextModule(this); 
            DoSetState(IsFinished);
        }
        
    }
}
