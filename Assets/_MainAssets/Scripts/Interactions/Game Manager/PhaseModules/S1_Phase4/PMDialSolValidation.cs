using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMDialSolValidation : PMValidation
{
    public void UpdateSelectedDSol(GameManager gMan)
    {
        List<ValidationModule> newvMod = new List<ValidationModule>();
        foreach(Transform t in gMan.SelectedDSol)
        {
            if (t.GetComponent<ValidationModule>())
            {
                newvMod.Add(t.GetComponent<ValidationModule>());
            }
        }

        UpdateVModules(newvMod);
    }

    
}
