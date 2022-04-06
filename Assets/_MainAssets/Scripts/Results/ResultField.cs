using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ResultField : MonoBehaviour
{
    public delegate void OnUpdate();
    public GPhaseModule Module;
    public Image Indicator;
    public TMP_Text Label;
    public Sprite Success;
    public Sprite Fail;
    public Sprite Default;
    public Image ImageContainer;
    
    public Color finishedColor = new Color(80, 141, 0, 255);
    public Color originalColor = new Color(154, 154, 154, 255);
    public Color distractorColor = new Color(255, 61, 61, 255);

    public void SetState(bool state)
    {
        if (state)
        {
            Debug.Log("Checklist Done : " + Module.name);
            Indicator.color = finishedColor;
        }
        else
        {
            Indicator.color = originalColor;
        }
    } 
    
    public void ForceSetState(bool state, bool distractor = false)
    {
        if (state)
        {
            Indicator.color = finishedColor;
            ImageContainer.color = new Color(finishedColor.r, finishedColor.g, finishedColor.b, ImageContainer.color.a);
        }
        else
        {
            Indicator.color = originalColor;
            ImageContainer.color = new Color(originalColor.r, originalColor.g, originalColor.b, ImageContainer.color.a);
        }

        if (distractor)
        {
            Indicator.color = distractorColor;
            Indicator.sprite = Fail;
            ImageContainer.color = new Color(distractorColor.r, distractorColor.g, distractorColor.b, ImageContainer.color.a);
        }
    }
}
