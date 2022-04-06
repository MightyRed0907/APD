using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Events;

public class ValidationManager : MonoBehaviour
{
    public ValidationModule CurrentVModule;
    public VMField CurrentFocusedField;
    public Transform VMRadialMenu;
    public TMP_Text FieldQuestion;
    public Sprite ValidSprite;
    public Sprite InvalidSprite;
    public bool isStarted;

    public UnityEvent OnValidation;
   

    private Ray ray;
    private RaycastHit vfHit;
    private VMField currentHighlightedField;

    public void StartVModule()
    {
        if (!CurrentVModule) { Debug.Log("Validation Module Cannot be Started : CURRENT VMODULE NULL "); return; }
        isStarted = true;
        CurrentVModule.EnableValidationArea();
    }
    
    public void EndVModule()
    {
        if (!CurrentVModule) { Debug.Log("Validation Module Cannot be Started : CURRENT VMODULE NULL "); return; }

        if (CurrentFocusedField)
        {
            UnfocusField();
        }

        CurrentVModule.DisableValidationArea();
        CurrentVModule = null;
        isStarted = false;
    }

    public void FocusField(VMField vF)
    {
        if (CurrentFocusedField)
        {
            UnfocusField();
        }

        CurrentFocusedField = vF;
        FieldQuestion.text = vF.FieldQuestion;
        vF.mark.gameObject.SetActive(true);
        if (vF.highlight)
        {
            if (vF.highlight.GetComponent<Renderer>()) 
            {
                vF.highlight.GetComponent<Renderer>().enabled = false;
            }
        }
        VMRadialMenu.gameObject.SetActive(true);
    }

    public void UnfocusField()
    {
        if(CurrentFocusedField.ValidationStatus == ValidationStatus.unvalidated)
        {
            CurrentFocusedField.mark.gameObject.SetActive(false);
        }
        if (CurrentFocusedField.highlight)
        {
            if (CurrentFocusedField.highlight.GetComponent<Renderer>())
            {
                CurrentFocusedField.highlight.GetComponent<Renderer>().enabled = false;
            }
        }
        CurrentFocusedField = null;
        VMRadialMenu.gameObject.SetActive(false);
    }

    public void VMFieldSetStateValid()
    {
        if (!CurrentFocusedField) { Debug.Log("No current focused field"); return; }
        //CurrentFocusedField.IsValid = true;
        CurrentFocusedField.ValidationStatus = ValidationStatus.valid;
        CurrentFocusedField.OnValidate.Invoke();
        CurrentVModule.OnAnyValidation.Invoke();
        if (CurrentFocusedField.mark)
        {
            CurrentFocusedField.mark.gameObject.SetActive(true);
            CurrentFocusedField.mark.sprite = ValidSprite;
            CurrentFocusedField.mark.color = Color.white;
        }
        UnfocusField();
    }
    
    public void VMFieldSetStateInvalid()
    {
        if (!CurrentFocusedField) { Debug.Log("No current focused field"); return; }
        //CurrentFocusedField.IsValid = false;
        CurrentFocusedField.ValidationStatus = ValidationStatus.invalid;
        CurrentFocusedField.OnValidate.Invoke();
        CurrentVModule.OnAnyValidation.Invoke();
        if (CurrentFocusedField.mark)
        {
            CurrentFocusedField.mark.gameObject.SetActive(true);
            CurrentFocusedField.mark.sprite = InvalidSprite;
            CurrentFocusedField.mark.color = Color.white;
        }
        UnfocusField();
    }

    public void VMFieldSetState(ValidationModule vMod, VMField field, bool state)
    {
        //field.IsValid = true;
        field.ValidationStatus = ValidationStatus.valid;
        field.OnValidate.Invoke();
        vMod.OnAnyValidation.Invoke();
        if (field.mark)
        {
            //field.mark.gameObject.SetActive(true);
            if (state)
            {
                field.mark.sprite = ValidSprite;
            }
            else
            {
                field.mark.sprite = InvalidSprite;
            }
            field.mark.color = Color.white;
        }
    }

    public void MoveCameraToField(Transform obj)
    {

    }

    public void Update()
    {
        if (!isStarted) return;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, 100);

        List<RaycastHit> rHits = hits.ToList<RaycastHit>();
        //Debug.Log("Raycast Hit List Count : " + rHits.Count);


        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            //Renderer rend = hit.transform.GetComponent<Renderer>();
            //Color origColor = rend.material.color;

            if (hit.collider.GetComponent<VMField>())
            {
                VMField newField = hit.collider.GetComponent<VMField>();

                if (newField.isActive)
                {
                    if (newField.highlight && CurrentFocusedField == null)
                    {
                        currentHighlightedField = newField;
                        vfHit = hit;
                        if (newField.GetComponent<Renderer>())
                        {
                            Renderer rend = newField.GetComponent<Renderer>();
                            rend.enabled = true;
                            rend.material.shader = Shader.Find("Transparent/Diffuse");
                            Color tempColor = rend.material.color;
                            tempColor = newField.HighlightColor;
                            tempColor.a = 0.5f;
                            rend.material.color = tempColor;
                        }
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        FocusField(newField);
                    }
                }
            }
        }

        if (rHits.Count <= 0 || !rHits.Contains(vfHit))
        {
            if (currentHighlightedField)
            {
                if (currentHighlightedField.highlight)
                {
                    if (currentHighlightedField.highlight.GetComponent<Renderer>())
                    {
                        currentHighlightedField.highlight.GetComponent<Renderer>().enabled = false;
                        currentHighlightedField = null;
                        vfHit = new RaycastHit();
                    }
                }
            }
        }
    }




}
