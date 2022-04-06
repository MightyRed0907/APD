using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionButtonManager : MonoBehaviour
{
    public List<Transform> ActionButtons = new List<Transform>();

    public void ToggleActionMenu() //Sample radial menu enabler
    {
        foreach (Transform b in ActionButtons)
        {
            if (!b.gameObject.activeSelf)
            {
                b.gameObject.SetActive(true);
            }
            else
            {
                b.gameObject.SetActive(false);
            }
        }
    }
    
    public void ShowActionMenu() //Sample radial menu enabler
    {
        foreach (Transform b in ActionButtons)
        {
            b.gameObject.SetActive(true);
        }
    }

    public void HideActionMenu() //Sample radial menu disabler
    {
        foreach (Transform b in ActionButtons)
        {
            b.gameObject.SetActive(false);
        }
    }

}
