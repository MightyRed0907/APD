using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMLights : PMToggle
{
    public override void SetStartingStatus()
    {
        IsFinished = ToggleModule.toggleStatus;
        finishedState = true;
    }
    public override void SetStatus()
    {
        if (IsFinished) return;
        IsFinished = ToggleModule.toggleStatus;
        OnFinish.Invoke();
    }
}
