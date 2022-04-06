using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuManager : MonoBehaviour
{
    public Transform RadialMenu;
    public RMF_RadialMenu rMenu;
    public RMConfig currentRMConfig;

    public void AddRMElement(RMConfig rmConf, RMF_RadialMenuElement rmE)
    {
        if (!rmConf) return;
        if (!rmConf.RMElements.Contains(rmE))
        {
            rmConf.RMElements.Add(rmE);
        }
    }

    public void ShowRadialMenu(RMConfig rmConf)
    {
        if (!rmConf) return;

        RadialMenu.gameObject.SetActive(true);

        foreach(RMF_RadialMenuElement e in rmConf.RMElements)
        {
            e.gameObject.SetActive(true);
        }
    }
    
    public void HideRadialMenu(RMConfig rmConf)
    {
        if (!rmConf) return;

        if (rMenu)
        {
            foreach (RMF_RadialMenuElement rmE in rMenu.elements)
            {
                rmE.gameObject.SetActive(false);
            }
        }

        RadialMenu.gameObject.SetActive(false);
    }

    public void HideRadialGUI()
    {
        foreach (RMF_RadialMenuElement rmE in rMenu.elements)
        {
            rmE.gameObject.SetActive(false);
        }
        RadialMenu.gameObject.SetActive(false);
    }

    public void RefreshRadialMenu(RMConfig rmConf)
    {
        HideRadialGUI();
        ShowRadialMenu(rmConf);
    }

}
