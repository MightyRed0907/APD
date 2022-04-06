using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionAreaManager : MonoBehaviour
{
    public CameraMovementController MainCamera;
    public InteractionArea CurrentInteractionArea;
    public Transform DefaultRefCamTransform;
    public Button ResetViewButton;

    public List<InteractionArea> interactionAreasPassed = new List<InteractionArea>();

    private Coroutine areaActivating;
    private Quaternion previousRotation;

    public void SetInteractionArea(InteractionArea ia)
    {
        CurrentInteractionArea = ia;
    }

    public IEnumerator IActivateArea(InteractionArea ia)
    {
        if (!ia.isActive) yield break;
        bool isResetBEnabled = false;

        if (CurrentInteractionArea != null)
        {
            ResetViewButton.gameObject.SetActive(true);
            if (CurrentInteractionArea.SubAreas.Contains(ia))
            {
                if (!interactionAreasPassed.Contains(ia))
                {
                    interactionAreasPassed.Add(CurrentInteractionArea);
                }
                
                if (ResetViewButton && ia.isAreaToggle)
                {
                    ResetViewButton.gameObject.SetActive(false);
                }
                
                CurrentInteractionArea = ia;
            }
            else if(CurrentInteractionArea == ia && ia.isAreaToggle)
            {
                if (interactionAreasPassed.Count > 0)
                {
                    int lastIndex = interactionAreasPassed.Count - 1;
                    Debug.Log("interaction area to go to : " + interactionAreasPassed[lastIndex].referenceCamTransform.gameObject.name);
                    MainCamera.MoveCameraToGuide(interactionAreasPassed[lastIndex].referenceCamTransform, interactionAreasPassed[lastIndex].camPanSpeed, interactionAreasPassed[lastIndex].camPanSpeed);
                    if (ia.isAreaToggledOn)
                    {
                        ia.isAreaToggledOn = false;
                    }
                    else
                    {
                        ia.isAreaToggledOn = true;
                    }
                    CurrentInteractionArea = interactionAreasPassed[lastIndex];
                    CurrentInteractionArea.EnableInteractables();
                    if (!CurrentInteractionArea.interactionOnActive)
                    {
                        if (CurrentInteractionArea.GetComponent<Interactable>())
                        {
                            CurrentInteractionArea.GetComponent<Interactable>().isInteractable = false;
                        }
                    }
                    interactionAreasPassed.Remove(interactionAreasPassed[lastIndex]);
                    
                    yield break;
                }
            }
            else
            {
                if (interactionAreasPassed.Count > 0)
                {
                    if (interactionAreasPassed[interactionAreasPassed.Count - 1].SubAreas.Contains(CurrentInteractionArea))
                    {
                        DisableArea(interactionAreasPassed[interactionAreasPassed.Count - 1]);
                        if (CurrentInteractionArea.GetComponent<Interactable>())
                        {
                            CurrentInteractionArea.GetComponent<Interactable>().isInteractable = false;
                        }
                        CurrentInteractionArea.DisableInteractables();
                        interactionAreasPassed.Remove(interactionAreasPassed[interactionAreasPassed.Count - 1]);
                    }
                }
                else
                {
                    DisableArea(CurrentInteractionArea);
                }
                if (ResetViewButton)
                {
                    ResetViewButton.gameObject.SetActive(false);
                }
                isResetBEnabled = true;
            }
        }
        bool noCurIA = false;
        if (CurrentInteractionArea == null)
        {
            noCurIA = true;
        }


        List<InteractionArea> activeSa = new List<InteractionArea>();
        if(ia.SubAreas.Count > 0)
        {
            foreach(InteractionArea sa in ia.SubAreas)
            {
                if (sa.isAreaToggledOn)
                {
                    activeSa.Add(sa);
                }
            }
        }

        if(activeSa.Count > 0)
        {
            CurrentInteractionArea = activeSa[activeSa.Count - 1];
            if (CurrentInteractionArea.GetComponent<Interactable>())
            {
                if (!CurrentInteractionArea.GetComponent<Interactable>().isSetSequentially)
                {
                    CurrentInteractionArea.GetComponent<Interactable>().isInteractable = true;
                }
            }
            interactionAreasPassed.Add(ia);
        }
        else
        {
            CurrentInteractionArea = ia;
        }

        CurrentInteractionArea.isAreaToggledOn = true;
        CurrentInteractionArea.EnableInteractables();
        CurrentInteractionArea.GetComponent<Collider>().enabled = false;
        previousRotation = MainCamera.transform.rotation;
        

        if (CurrentInteractionArea.referenceCamTransform)
        {
            if (MainCamera.transform.position != CurrentInteractionArea.referenceCamTransform.transform.position ||
                MainCamera.transform.rotation != CurrentInteractionArea.referenceCamTransform.transform.rotation)
            {
                MainCamera.MoveCameraToGuide(CurrentInteractionArea.referenceCamTransform, CurrentInteractionArea.camPanSpeed, CurrentInteractionArea.camPanSpeed);
            }
        }

        if (!CurrentInteractionArea.interactionOnActive)
        {
            if (CurrentInteractionArea.GetComponent<Interactable>())
            {
                CurrentInteractionArea.GetComponent<Interactable>().isInteractable = false;
            }
        }

        while (!MainCamera.IsFinishedLerping())
        {
            yield return null;
        }

        MainCamera.GetComponent<CameraController>().SetXRestriction(true, CurrentInteractionArea.referenceCamTransform.transform);

        if (noCurIA)
        {
            if (ResetViewButton)
            {
                ResetViewButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (isResetBEnabled)
            {
                if (ResetViewButton)
                {
                    ResetViewButton.gameObject.SetActive(true);
                }
            }
        }

        CurrentInteractionArea.OnActivate.Invoke();
        areaActivating = null;
        yield break;
    }

    public void ActivateArea(InteractionArea ia)
    {
        if(areaActivating != null)
        {
            StopCoroutine(areaActivating);
        }
        areaActivating = StartCoroutine(IActivateArea(ia));
    }

    public void DisableArea(InteractionArea ia)
    {
        ia.DisableInteractables();

        if (!ia.isAreaToggle)
        {
            ia.isAreaToggledOn = false;
        }

        ia.OnDeactivate.Invoke();

        if (ia.GetComponent<Interactable>())
        {
            if (!ia.GetComponent<Interactable>().isSetSequentially)
            {
                ia.GetComponent<Interactable>().isInteractable = true;
            }
        }

        ia.GetComponent<Collider>().enabled = true;
    }

    public void ReturnToPreviousArea()
    {
        if (!MainCamera.IsFinishedLerping()) return;
        if(interactionAreasPassed.Count > 0)
        {
            int lastIndex = interactionAreasPassed.Count - 1;
            ActivateArea(interactionAreasPassed[lastIndex]);
            Debug.Log("Interaction area : " + interactionAreasPassed[lastIndex].gameObject.name);
            interactionAreasPassed.Remove(interactionAreasPassed[lastIndex]);
            if (ResetViewButton)
            {
                ResetViewButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if(CurrentInteractionArea != null)
            {
                DisableArea(CurrentInteractionArea);
                MainCamera.MoveCamera(DefaultRefCamTransform.transform.position, new Quaternion(0, MainCamera.transform.rotation.y, 0, MainCamera.transform.rotation.w));
                MainCamera.SetRefCam(DefaultRefCamTransform);
                MainCamera.GetComponent<CameraController>().SetXRestriction(false, DefaultRefCamTransform);
                CurrentInteractionArea = null;
                if (ResetViewButton)
                {
                    ResetViewButton.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToPreviousArea();
        }
    }


    private bool ResetBtnState;

    public void SetResetBtnState()
    {
        if (!ResetViewButton) return;
        ResetBtnState = ResetViewButton.gameObject.activeSelf;
        ResetViewButton.gameObject.SetActive(false);
    }

    public void ToggleResetBtn()
    {
        if (!ResetViewButton) return;
        ResetViewButton.gameObject.SetActive(ResetBtnState);
    }

}
