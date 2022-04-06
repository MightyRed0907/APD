using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public Image Tooltip;
    public TMP_Text TooltipText;
    public Vector3 PosOffset;
    public bool isDisplayed;

    public void Update()
    {
        if (Tooltip)
        {
            if (isDisplayed)
            {
                if (!Tooltip.gameObject.activeSelf)
                {
                    Tooltip.gameObject.SetActive(true);
                }
                Tooltip.transform.position = new Vector3(Input.mousePosition.x + PosOffset.x, Input.mousePosition.y + PosOffset.y);
            }
        }
    }

    public void ShowTooltip(TooltipConfig tC)
    {
        isDisplayed = true;
        TooltipText.text = tC.GetTooltipText();
    }

    public void HideTooltip()
    {
        isDisplayed = false;
        Tooltip.gameObject.SetActive(false);

    }
}
