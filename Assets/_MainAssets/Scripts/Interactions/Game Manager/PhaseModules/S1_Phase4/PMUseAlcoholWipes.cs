using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMUseAlcoholWipes : GPhaseModule
{
    public ITAlcoholWipe alcWipe;

    public void UpdatePhaseModule()
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
