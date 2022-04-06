using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSelfPrep : GStagePhase
{
    public void CloseDrawer(Transform drawer)
    {
        if (drawer.GetComponent<TMSlider>().toggleStatus)
        {
            
            drawer.GetComponent<Interactable>().OnInteract.Invoke();
            drawer.GetComponent<Interactable>().isInteractable = false;
            drawer.GetComponent<Interactable>().isSetSequentially = true;
            drawer.GetComponent<InteractionArea>().isAreaToggledOn = false;
            InteractionAreaManager iaM = FindObjectOfType<InteractionAreaManager>();
            if (iaM)
            {
                iaM.interactionAreasPassed.Clear();
            }
            foreach (Collider c in drawer.GetComponents<Collider>())
            {
                c.enabled = false;
            }

        }
        
    }
}
