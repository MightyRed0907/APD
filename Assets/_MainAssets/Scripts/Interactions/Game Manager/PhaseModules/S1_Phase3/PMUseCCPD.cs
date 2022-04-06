using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMUseCCPD : GPhaseModule
{
    public void CheckCCPDModuleStatus()
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
