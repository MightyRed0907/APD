using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMObjOpening : GPhaseModule
{
    public Openable OpenableObj;

    public void UpdateOpenStatus()
    {
        if (!OpenableObj) { Debug.Log("No openable object assigned!"); return; }
        if (IsFinished) return;
        IsFinished = OpenableObj;
        UpdatePhaseParent();
        if (phaseParent.IsSequential && phaseParent.stageParent.IsSequential)
        {

            if (IsFinished)
            {
                phaseParent.UnlockNextModule(this);
            }
            if (phaseParent.stageParent.IsSequential)
            {
                DoSetState(IsFinished);
            }
        }
    }
}
