using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ITPaperTowel : Item
{
    public PMDryHands CurrentDryHandModule;
    public bool IsRequiringHandDry;
    public bool PaperTowelUsed;
    public UnityEvent OnUse;

    public void RequireHandDry()
    {
        IsRequiringHandDry = true;
        PaperTowelUsed = false;
    }

    public override bool UseItem()
    {
        if (!base.UseItem()) return false;

        if (IsRequiringHandDry)
        {
            OnUse.Invoke();
            IsRequiringHandDry = false;
            PaperTowelUsed = true;
        }

        return true;
    }

    public void SetDHModule(PMDryHands dhMod)
    {
        CurrentDryHandModule = dhMod;
    }

    public void SetDryHandStatus()
    {
        if (!CurrentDryHandModule) return;
        CurrentDryHandModule.IsFinished = true;
        CurrentDryHandModule.BSetModuleStatus();
        if (CurrentDryHandModule.phaseParent.IsSequential && CurrentDryHandModule.phaseParent.stageParent.IsSequential)
        {
            CurrentDryHandModule.phaseParent.UnlockNextModule(CurrentDryHandModule);
            CurrentDryHandModule.DoSetState(true);
        }
        CurrentDryHandModule = null;
    }
 
}
