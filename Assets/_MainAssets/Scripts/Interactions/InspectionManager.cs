using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InspectionManager : MonoBehaviour
{
    public Inspectable CurrentInspectedObj;
    public bool isCurrentlyInspecting;
    public bool canRotateObjects;

    public UnityEvent OnStartInspect;
    public UnityEvent OnEndInspect;

    private bool enableShadows;
    private Vector3 posLastFrame;

    private Transform originalParent;
    private Vector3 originalPos;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private InspectionManager inspectionManager;
    private ObjectLerper oLerper;
    private ObjectRotator oRotator;
    private ValidationManager validationManager;

    public void Start()
    {
        validationManager = FindObjectOfType<ValidationManager>();
    }
    public void Update()
    {
        RotateObject();
    }

    public void RotateObject()
    {
        if (CurrentInspectedObj == null || !canRotateObjects || !isCurrentlyInspecting) return;

        if (Input.GetMouseButtonDown(0))
        {
            posLastFrame = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - posLastFrame;
            posLastFrame = Input.mousePosition;

            var axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;

            CurrentInspectedObj.transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.1f, axis) * CurrentInspectedObj.transform.rotation;
        }
    }

    public void StartInspect()
    {
        if (isCurrentlyInspecting) return;

        StartCoroutine(IStartInspect());
    }

    public IEnumerator IStartInspect()
    {
        if (isCurrentlyInspecting || CurrentInspectedObj == null)
        {
            yield break;
        }

        if (validationManager) //Set currentInspectedObject for validationManager
        {
            if (CurrentInspectedObj.GetComponent<ValidationModule>())
            {
                validationManager.CurrentVModule = CurrentInspectedObj.GetComponent<ValidationModule>();
                validationManager.StartVModule();
            }
        }

        oLerper = CurrentInspectedObj.GetComponent<ObjectLerper>();
        oRotator = CurrentInspectedObj.GetComponent<ObjectRotator>();


        if (CurrentInspectedObj.GetComponent<Interactable>())
        {
            CurrentInspectedObj.GetComponent<Interactable>().isHighlightable = false;
        }

        if (CurrentInspectedObj.GetComponent<Outline>())
        {
            CurrentInspectedObj.GetComponent<Outline>().enabled = false;
        }

        isCurrentlyInspecting = true;
        CurrentInspectedObj.IsCurrentlyInspected = true;

        originalParent = CurrentInspectedObj.transform.parent;
        originalPos = CurrentInspectedObj.transform.position;
        originalRotation = CurrentInspectedObj.transform.rotation;
        originalScale = CurrentInspectedObj.transform.localScale;

        OnStartInspect.Invoke();

        CurrentInspectedObj.transform.SetParent(Camera.main.transform);

        DisableOtherActions(CurrentInspectedObj);

        if (CurrentInspectedObj.centerOnCamera)
        {

            oLerper.LocalLerpTowards(new Vector3(0, 0, CurrentInspectedObj.centerOnCameraZPos), 0.4f);
        }
        else
        {
            oLerper.LocalLerpTowards(new Vector3(CurrentInspectedObj.inspectPosition.x, CurrentInspectedObj.inspectPosition.y, CurrentInspectedObj.inspectPosition.z), 0.4f);
        }

        //CurrentInspectedObj.transform.rotation = CurrentInspectedObj.inspectRotation;
        oRotator.LerpRotation(CurrentInspectedObj.inspectRotation, 0.4f);
        StartCoroutine(ILerpScale(Rescale(CurrentInspectedObj.RescaleType, CurrentInspectedObj.transform.localScale), 0.4f));

        if (!enableShadows)
        {
            if (CurrentInspectedObj.GetComponent<MeshRenderer>())
            {
                CurrentInspectedObj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }
        else
        {
            if (CurrentInspectedObj.GetComponent<MeshRenderer>())
            {
                CurrentInspectedObj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }

        while(oRotator.IsCurrentlyLerping() || oLerper.IsCurrentlyLerping() || sLerping)
        {
            yield return null;
        }

        if (Camera.main.GetComponent<Toggle>())
        {
            Camera.main.GetComponent<Toggle>().ToggleTargets();
        }

        yield break;
    }

    public Vector3 Rescale(RescaleType rType, Vector3 origScale)
    {
        Vector3 newScale = new Vector3(0, 0, 0);

        if (rType == RescaleType.shrink)
        {
            newScale = new Vector3(origScale.x / CurrentInspectedObj.rescaleFactor, origScale.y / CurrentInspectedObj.rescaleFactor, origScale.z / CurrentInspectedObj.rescaleFactor);
            return newScale;
        }
        else if (rType == RescaleType.enlarge)
        {
            newScale = new Vector3(origScale.x * CurrentInspectedObj.rescaleFactor, origScale.y * CurrentInspectedObj.rescaleFactor, origScale.z * CurrentInspectedObj.rescaleFactor);
            return newScale;
        }
        else
        {
            return origScale;
        }
    }

    public void EndInspect()
    {
        if (!isCurrentlyInspecting) return;
        if (validationManager) //Set currentInspectedObject for validationManager
        {
            if (CurrentInspectedObj.GetComponent<ValidationModule>())
            {
                validationManager.EndVModule();
            }
        }

        isCurrentlyInspecting = false;
        CurrentInspectedObj.transform.SetParent(originalParent);
        //CurrentInspectedObj.transform.rotation = originalRotation;
        oRotator.LerpRotation(originalRotation, 0.4f);
        oLerper.LerpTowards(originalPos, 0.4f);
        StartCoroutine(ILerpScale(originalScale, 0.4f));
        CurrentInspectedObj.IsCurrentlyInspected = false;
        if (CurrentInspectedObj.GetComponent<MeshRenderer>())
        {
            CurrentInspectedObj.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        if (CurrentInspectedObj.GetComponent<Interactable>())
        {
            CurrentInspectedObj.GetComponent<Interactable>().isHighlightable = true;
        }

        if (CurrentInspectedObj.GetComponent<Outline>())
        {
            CurrentInspectedObj.GetComponent<Outline>().enabled = true;
        }
        EnableOtherActions(CurrentInspectedObj);
        if (Camera.main.GetComponent<Toggle>())
        {
            Camera.main.GetComponent<Toggle>().ToggleTargets();
        }

        
        OnEndInspect.Invoke();
    }

    private bool sLerping;

    private IEnumerator ILerpScale(Vector3 eScale, float duration)
    {
        if (!CurrentInspectedObj) yield break;
        while (sLerping) yield return new WaitForSeconds(0.1f);
        sLerping = true;
        Vector3 startScale = CurrentInspectedObj.transform.localScale;
        Vector3 endScale = eScale;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float smooth = t / duration;
            smooth = smooth * smooth * (3f - 2f * smooth);
            CurrentInspectedObj.transform.localScale = Vector3.Lerp(startScale, endScale, smooth);
            yield return null;
        }
        CurrentInspectedObj.transform.localScale = endScale;
        sLerping = false;
        yield break;
    }

    public void ResetRotation()
    {
        if (!CurrentInspectedObj) return;
        //CurrentInspectedObj.transform.rotation = new Quaternion(0, 0, 0, 0);
        CurrentInspectedObj.transform.rotation = CurrentInspectedObj.inspectRotation;
    }

    public void EnableOtherActions(Inspectable obj)
    {
        if (obj.GetComponent<Interactable>())
        {
            if (!obj.GetComponent<Interactable>().isSetSequentially)
            {
                obj.GetComponent<Interactable>().EnableInteraction();
            }
        }

        if (obj.GetComponent<Draggable>())
        {
            obj.GetComponent<Draggable>().EnableDrag();
        }
    }
    
    public void DisableOtherActions(Inspectable obj)
    {
        if (obj.GetComponent<Interactable>())
        {
            obj.GetComponent<Interactable>().DisableInteraction();
        }
            if (obj.GetComponent<Draggable>())
        {
            obj.GetComponent<Draggable>().DisableDrag();
        }
    }

}
