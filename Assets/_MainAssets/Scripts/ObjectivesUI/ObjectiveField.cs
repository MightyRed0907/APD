using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveField : MonoBehaviour
{
    public delegate void OnUpdate();
    public OnUpdate OnSetState;
    public GPhaseModule Module;
    public Image Indicator;
    public TMP_Text Label;
    public Color finishedColor = new Color(80, 141, 0, 255);
    public Color originalColor = new Color(154,154,154,255);

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

        OnSetState.Invoke();
    }
}
