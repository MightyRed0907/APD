using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMValidation : GPhaseModule
{
    public List<ValidationModule> ValidationModules = new List<ValidationModule>();

    public void UpdateVModules(List<ValidationModule> vMod)
    {
        ValidationModules.Clear();
        foreach(ValidationModule newVMod in vMod)
        {
            ValidationModules.Add(newVMod);
        }
        CheckValidationStatus();
    }

    public void CheckValidationStatus()
    {
        IsFinished = CheckModuleCompletion();
        if (!IsFinished) return;
        BSetModuleStatus();
        if (phaseParent.IsSequential && phaseParent.stageParent.IsSequential)
        {
            if (IsFinished)
            {
                phaseParent.UnlockNextModule(this);
                DoSetState(IsFinished);
                OnFinish.Invoke();
            }
        }
    }

    public bool CheckModuleCompletion()
    {
        bool modComplete = true;

        foreach(ValidationModule vM in ValidationModules)
        {
            if (!IsValidationComplete(vM))
            {
                modComplete = false;
            }
        }

        return modComplete;
    }

    public void CheckBaseOnVModCompletion(ValidationModule vMod)
    {
        IsFinished = IsValidationComplete(vMod);
        BSetModuleStatus();
        if (phaseParent.IsSequential && phaseParent.stageParent.IsSequential)
        {
            if (IsFinished)
            {
                phaseParent.UnlockNextModule(this);
                DoSetState(IsFinished);
                OnFinish.Invoke();
            }
        }
    }

    public bool IsValidationComplete(ValidationModule vMod)
    {
        bool isComplete = true;

        foreach(VMField vF in vMod.Fields)
        {
            if (!vF.GetComponent<Distractor>())
            {
                if (vF.ValidationStatus == ValidationStatus.unvalidated)
                {
                    isComplete = false;
                }
            }

            
            if (vF.GetComponent<Distractor>())
            {
                if (vF.ValidationStatus != ValidationStatus.unvalidated)
                {
                    if (!DistractorsActivated.Contains(vF.GetComponent<Distractor>()))
                    {
                        DistractorsActivated.Add(vF.GetComponent<Distractor>());
                    }
                } 
            }
            
        }

        return isComplete;
    }

    public override int GetScorePoints()
    {
        int score = 0;

        foreach (ValidationModule vMod in ValidationModules)
        {
            foreach (VMField vF in vMod.Fields)
            {
                if (!vF.GetComponent<Distractor>())
                {
                    if (vF.ValidationStatus != ValidationStatus.unvalidated)
                    {
                        if (vF.GetValidation() == vF.IsValid) 
                        {
                            score += 1;
                        }
                        
                    }
                }
            }
        }

        return score;
    } 
    
    public override int GetPossiblePoints()
    {
        int score = 0;

        foreach (ValidationModule vMod in ValidationModules)
        {
            foreach (VMField vF in vMod.Fields)
            {
                if (!vF.GetComponent<Distractor>())
                {
                    score += 1;
                }
            }
        }

        Debug.Log("Possible Points for " + gameObject.name + " : " + score);

        return score;
    }


}
