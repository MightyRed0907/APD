using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMUIElement : ToggleModule
{
    public override bool Toggle()
    {
        if (!base.Toggle()) return false;



        return true;
    }


}
