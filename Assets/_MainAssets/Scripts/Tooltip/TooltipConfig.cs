using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TooltipType
{
    text,
    action,
    toggle 
}

public class TooltipConfig : MonoBehaviour
{
    public TooltipType Type;
    public string objectName;
    [Header("IF TEXT")]
    public string text;
    [Header("IF TOGGLE")]
    public ToggleModule ToggleModule;
    public string toggleTrue;
    public string toggleFalse;

    public string GetTooltipText()
    {
        if(Type == TooltipType.text)
        {
            return text;
        }
        else if(Type == TooltipType.toggle)
        {
            if (ToggleModule.toggleStatus)
            {
                return toggleTrue;
            }
            else if (!ToggleModule.toggleStatus)
            {
                return toggleFalse;
            }
            else
            {
                return toggleFalse;
            }
        }
        else
        {
            return text;
        }
    }
}
