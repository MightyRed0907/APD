using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMTurnOnAPDM : GPhaseModule
{ 
    public void UpdateModuleStatus()
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
