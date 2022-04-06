using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum ValidationStatus
{
    unvalidated,
    valid,
    invalid
}

public class VMField : MonoBehaviour
{
    public ValidationStatus ValidationStatus;
    public TMP_Text ValidationFieldText;
    public bool isActive = false;
    public bool IsValid = false;
    public string FieldName;
    public string FieldQuestion;
    public string FieldValue;
    public SpriteRenderer mark;
    public Transform highlight;
    public Color HighlightColor = new Color(158, 246, 255, 0.35f);

    public UnityEvent OnValidate;

    public void SetField(string fieldValue)
    {
        FieldValue = fieldValue;
    }

    public bool GetValidation()
    {
        if (ValidationStatus == ValidationStatus.valid)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
