using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ITHandSanitizer : Item
{
    public UnityEvent OnUse;

    public override bool UseItem()
    {
        if (!base.UseItem()) return false;
            
        OnUse.Invoke();

        return true;
    }
}
