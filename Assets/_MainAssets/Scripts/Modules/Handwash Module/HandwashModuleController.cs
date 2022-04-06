using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HandwashModuleController : MonoBehaviour
{
    public PMHandwash currentHandwashPM;
    public bool isInProgress;
    public Canvas HandwashCanvas;
    public Transform HandwashGUIContainer;
    public Transform RefCam;
    public bool IsCurrentSessionCorrect;
    public float camPanSpeed = 1;
    public bool showMistakes;
    public bool showLabels;
    public bool isStaticSteps;
    [HideInInspector]
    public bool requireHandDry;
    public HandwashManager HandwashManager;

    public UnityEvent OnStartModule;
    public UnityEvent OnEndModule;
    public UnityEvent OnSuccessful;
    public UnityEvent OnRequireDryHand;
    public UnityEvent OnFailed;
    private Transform currentCamera;

    private float sessionTime;

    public void SetRequireHandDry(bool required)
    {
        requireHandDry = required;
    }

    public void SetHandwashTitle(string title)
    {
        if (!string.IsNullOrEmpty(title))
        {
            HandwashManager.SetTitle(title);
        }
    }

    public void StartHandwashModule(Transform refCam)
    {
        if (isInProgress) return;
        StartCoroutine(IStartHandwashModule(refCam));
    }

    public void SetHandwashType(HWType type)
    {
        HandwashManager.SetHandwashType(type);
    }

    public void ResetRequireHandDry()
    {
        IsCurrentSessionCorrect = false;
    }

    public IEnumerator IStartHandwashModule(Transform refCam)
    {
        if (isInProgress) yield break;

        isInProgress = true;

        EnableHandwashModuleCol();

        if (refCam)
        {
            currentCamera = Camera.main.GetComponent<CameraMovementController>().CurrentRefCam();
        }

        HandwashManager.PrepareHandwash(showMistakes, showLabels, isStaticSteps);

        OnStartModule.Invoke();

        if (refCam)
        {
            Camera.main.GetComponent<CameraMovementController>().MoveCameraToGuide(refCam, camPanSpeed, camPanSpeed);
        }

        if (Camera.main.GetComponent<CameraController>())
        {
            Camera.main.GetComponent<CameraController>().isControllable = false;
        }

        if (refCam)
        {
            while (Camera.main.GetComponent<CameraMovementController>().IsCurrentlyLerping())
            {
                yield return null;
            }
        }

        HandwashGUIContainer.gameObject.SetActive(true);

        yield break;
    }

    public void EndHandwashModule()
    {
        StartCoroutine(IEndHandwashModule());
    }


    public IEnumerator IEndHandwashModule()
    {
        if (!isInProgress) yield break;

        isInProgress = false;

        if (HandwashManager.IsStepsFilledOut())
        {
            if (requireHandDry)
            {
                OnRequireDryHand.Invoke();
            }

            if (HandwashManager.IsStepsCorrect())
            {
                IsCurrentSessionCorrect = true;
                OnSuccessful.Invoke();
                if (requireHandDry)
                {
                    OnRequireDryHand.Invoke();
                }

            }
            else
            {
                IsCurrentSessionCorrect = false;
                OnFailed.Invoke();
            }
        }
        else
        {
            IsCurrentSessionCorrect = false;
            OnFailed.Invoke();
        }

        /*
        if (HandwashManager.IsStepsCorrect())
        {
            IsCurrentSessionCorrect = true;
            OnSuccessful.Invoke();
            if (requireHandDry)
            {
                OnRequireDryHand.Invoke();
            }

        }
        else
        {
            IsCurrentSessionCorrect = false;
            OnFailed.Invoke();
        }
        */

        HandwashGUIContainer.gameObject.SetActive(false);

        Camera.main.GetComponent<CameraMovementController>().MoveCameraToGuide(currentCamera, camPanSpeed , camPanSpeed);

        if (Camera.main.GetComponent<CameraController>())
        {
            Camera.main.GetComponent<CameraController>().isControllable = true;
        }

        OnEndModule.Invoke();

        DisableHandwashModuleCol();

        yield break;
    }

    public void SetHandwashPM(PMHandwash pmHw)
    {
        currentHandwashPM = pmHw;
    }

    public void SetHandwashStatus()
    {
        if (!currentHandwashPM) return;
        currentHandwashPM.IsFinished = true;
        currentHandwashPM.BSetModuleStatus();
        
        if (currentHandwashPM.phaseParent.IsSequential && currentHandwashPM.phaseParent.stageParent.IsSequential)
        {
            currentHandwashPM.phaseParent.UnlockNextModule(currentHandwashPM);
            currentHandwashPM.DoSetState(true);
        }
        currentHandwashPM = null;
    }

    public void EnableHandwashModuleCol()
    {
        HandwashManager.GetComponent<Image>().enabled = true;
    }
    public void DisableHandwashModuleCol()
    {
        HandwashManager.GetComponent<Image>().enabled = false;
    }
}
