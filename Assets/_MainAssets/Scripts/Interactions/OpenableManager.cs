using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableManager : MonoBehaviour
{
    public InteractionManager InteractionManager;
    private Interactable CurrentInteractable;
    public List<Openable> OpenableList = new List<Openable>();
    public RMF_RadialMenuElement RMF_OpenableElement;


    public void Update()
    {
        if (InteractionManager)
        {
            if (InteractionManager.GetCurrentInteractable())
            {
                CurrentInteractable = InteractionManager.GetCurrentInteractable();
            }
        }
    }

    public void OpenObject()
    {
        if (CurrentInteractable.GetComponent<Openable>())
        {
            CurrentInteractable.GetComponent<Openable>().OpenObject();
        }
    }

    public void MassDisableOpenOption()
    {
        foreach (Openable o in OpenableList)
        {
            if (o.GetComponent<RMConfig>())
            {
                if (o.GetComponent<RMConfig>().RMElements.Contains(RMF_OpenableElement))
                {
                    o.GetComponent<RMConfig>().RMElements.Remove(RMF_OpenableElement);
                }
            }
        }
    }

    public void MassEnableOpenOption()
    {
        foreach(Openable o in OpenableList)
        {
            if (o.GetComponent<RMConfig>())
            {
                if (!o.GetComponent<RMConfig>().RMElements.Contains(RMF_OpenableElement))
                {
                    o.GetComponent<RMConfig>().RMElements.Add(RMF_OpenableElement);
                }
            }
        }
    }
}
