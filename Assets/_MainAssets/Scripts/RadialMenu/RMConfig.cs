using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMConfig : MonoBehaviour
{
    public List<RMF_RadialMenuElement> RMElements = new List<RMF_RadialMenuElement>();

    public void AddRMElement(RMF_RadialMenuElement rmE)
    {
        if (!RMElements.Contains(rmE))
        {
            RMElements.Add(rmE);
        }
    }
    
    public void RemoveRMElement(RMF_RadialMenuElement rmE)
    {
        if (RMElements.Contains(rmE))
        {
            RMElements.Remove(rmE);
        }
    }
}
