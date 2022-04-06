using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class ValidationModule : MonoBehaviour
{
    public bool isActive = true;
    public Transform Object;
    public List<VMField> Fields = new List<VMField>();
    public UnityEvent OnAnyValidation;
    private ValidationManager VManager;

    public virtual bool EnableValidationArea()
    {
        if (!isActive) return false;

        EnableVMFields();

        return true;
    }
    
    public virtual bool DisableValidationArea()
    {
        if (!isActive) return false;

        DisableVMFields();

        return true;
    }

    public void DisableVMFields()
    {
        foreach (VMField vF in Fields)
        {
            if (vF.mark)
            {
                vF.mark.gameObject.SetActive(false);
            }
            vF.isActive = false;
        }
    }  
    
    public void EnableVMFields()
    {
        foreach (VMField vF in Fields)
        {
            if (vF.mark)
            {
                if(vF.ValidationStatus != ValidationStatus.unvalidated)
                {
                    vF.mark.gameObject.SetActive(true);
                }
            }
            vF.isActive = true;
        }
    }

    public void AutocompleteValidation(bool state)
    {
        if (!VManager) return;
        foreach(VMField field in Fields)
        {
            if (!field.GetComponent<Distractor>())
            {
                VManager.VMFieldSetState(this, field, state);
            }
        }
    }

    public void Start()
    {
        if (!Object)
        {
            Object = GetComponent<Transform>();
        }

        VManager = FindObjectOfType<ValidationManager>();
    }
}
