using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class InteractionManager : MonoBehaviour
{
    
    [SerializeField]
    private Interactable currentInteractable; //holder for the selected interactable
    private Interactable currentUnselectable; //holder for the selected interactable
    [SerializeField]
    private float minimumClickTime = 0.11f; //This will be the basis to determine a click from a drag
    [SerializeField]
    private bool highlightInteractables;
    private bool isBlockedByUI = true;
    private InspectionManager inspectionManager;
    private ItemController itemController;

    private Ray ray;
    public RaycastHit hit;

    private bool isClickStarted; //This will be used to tell ClickTimer if a click was started
    private float clickTime; //This is the time from the initial mouseClick to the mouseRelease
    [SerializeField]
    private Outline currentOutline;
    private TooltipManager tooltipManager;

    private LayerMask allLayerMask = -1;
    public UnityEvent OnNullDeselect;

    public void Start()
    {
        inspectionManager = FindObjectOfType<InspectionManager>();
        itemController = FindObjectOfType<ItemController>();
        tooltipManager = FindObjectOfType<TooltipManager>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        ClickTimer(isClickStarted); //Sets and runs the clickTime

        if (Physics.Raycast(ray, out hit, 100, allLayerMask, QueryTriggerInteraction.Ignore)) 
        {
            if (hit.collider.GetComponent<Interactable>()) //this is just a sample class to determine whether the clicked object is an interactable / clickable
            {
                if (hit.collider.GetComponent<Interactable>().IsInteractable())
                {
                    if (tooltipManager) 
                    {
                        if (hit.collider.GetComponent<TooltipConfig>())
                        {
                            tooltipManager.ShowTooltip(hit.collider.GetComponent<TooltipConfig>());
                        }
                    }

                    if (highlightInteractables)
                    {
                        if (hit.collider.GetComponent<Outline>())
                        {
                            if (currentOutline)
                            {
                                if (currentOutline != hit.collider.GetComponent<Outline>())
                                {
                                    currentOutline.enabled = false;
                                    currentOutline = null;
                                    if (tooltipManager)
                                    {
                                        tooltipManager.HideTooltip();
                                    }
                                }
                            }
                            currentOutline = hit.collider.GetComponent<Outline>();
                            hit.collider.GetComponent<Outline>().enabled = true;
                            hit.collider.GetComponent<Outline>().OutlineColor = Color.white;
                            hit.collider.GetComponent<Outline>().OutlineWidth = 8;
                            currentOutline = hit.collider.GetComponent<Outline>();
                            if (tooltipManager)
                            {
                                if (hit.collider.GetComponent<TooltipConfig>())
                                {
                                    tooltipManager.ShowTooltip(hit.collider.GetComponent<TooltipConfig>());
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (currentOutline)
                    {
                        if (currentOutline != hit.collider.GetComponent<Outline>())
                        {
                            currentOutline.enabled = false;
                            currentOutline = null;
                        }
                    }

                    if (tooltipManager)
                    {
                        tooltipManager.HideTooltip();
                    }
                }

            }
            else
            {
                if (highlightInteractables)
                {
                    if (currentOutline != null)
                    {
                        currentOutline.enabled = false;
                        currentOutline = null;
                    }
                    else
                    {
                        currentOutline = null;
                    }
                }

                if (tooltipManager)
                {
                    tooltipManager.HideTooltip();
                }

            }

            if (Input.GetMouseButtonDown(0)) 
            {
                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<Interactable>()) //this is just a sample class to determine whether the clicked object is an interactable / clickable
                    {
                        if (hit.collider.GetComponent<Interactable>().IsInteractable())
                        {
                            isClickStarted = true; //set the click to start to get the clickTime via the ClickTimer
                            if (currentInteractable == hit.collider.GetComponent<Interactable>())
                            {
                                if (currentInteractable.isClickToDeselect)
                                {
                                    DeselectInteractable();
                                }
                            }
                            else
                            {
                                if (hit.collider.GetComponent<Interactable>().isSelectable)//get interactable object and set to a variable / container
                                {
                                    DeselectInteractable();
                                    SetInteractable(hit.collider.GetComponent<Interactable>());
                                }
                                else 
                                {
                                    currentUnselectable = hit.collider.GetComponent<Interactable>();
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) 
            {
                if (isClickStarted) //check if the click has already started
                {
                    if (currentInteractable != null)
                    {
                        if (clickTime <= minimumClickTime) //determine if the interaction was just a click
                        {
                            //INVOKE HERE THE EVENTS FOR CLICKING
                            if (currentInteractable.IsInteractable() || !EventSystem.current.IsPointerOverGameObject())
                            {
                                if (currentUnselectable)
                                {
                                    currentUnselectable.Interact();
                                    currentUnselectable = null;
                                }
                                else
                                {
                                    currentInteractable.Interact();
                                }
                                 //this is just a sample behaviour when the interaction is just a click
                            }
                            
                            //currentInteractable.Select(); //this is just a sample behaviour when the interaction is just a click
                           
                            //DeselectInteractable(); // *OPTIONAL* deselects whatever interactable was selected after clicking - this can be removed so that an object will still be selected after a click
                        }
                        else if (clickTime > minimumClickTime) //determine if the interaction was a drag event
                        {
                            //INVOKE HERE THE EVENTS AFTER DRAGGING AN OBJECT
                            
                            if (currentInteractable.GetComponent<Draggable>() && currentInteractable.GetComponent<Draggable>().enabled && !currentInteractable.GetComponent<Draggable>().isOverPlaceableArea) //this is just a sample behaviour when drag is released
                            {
                                currentInteractable.GetComponent<Draggable>().ReturnToOriginalPos();
                                currentInteractable.GetComponent<Draggable>().EndDrag();
                                DeselectInteractable(); //*OPTIONAL* Deselects whatever interactable was selected
                            }
                        }
                    }
                    else
                    {
                        if (currentUnselectable)
                        {
                            currentUnselectable.Interact();
                            currentUnselectable = null;
                        }
                    }
                }

                isClickStarted = false; //reset the clickTime via ClickTimer everytime the left mouse button is released
            }
        }
        else
        {
            if (highlightInteractables)
            {
                if (currentOutline)
                {
                    currentOutline.enabled = false;
                    currentOutline = null;
                }
            }

            if (tooltipManager)
            {
                tooltipManager.HideTooltip();
            }
        }

        #region DragHandler 
        //this region handles my sample mouse drag functionality

        if (clickTime > minimumClickTime && isClickStarted && currentInteractable != null) 
        {
            if (currentInteractable.GetComponent<Draggable>() && currentInteractable.GetComponent<Draggable>().enabled)
            {
                currentInteractable.GetComponent<Draggable>().Drag();
            }
        }

        if (currentInteractable != null)
        {
            if (currentInteractable.GetComponent<Draggable>() && currentInteractable.GetComponent<Draggable>().enabled)
            {
                if (Input.GetMouseButtonUp(0) && currentInteractable.GetComponent<Draggable>().isBeingDragged && !currentInteractable.GetComponent<Draggable>().isOverPlaceableArea)
                {
                    currentInteractable.GetComponent<Draggable>().ReturnToOriginalPos();
                    currentInteractable.GetComponent<Draggable>().EndDrag();
                }
            }
        }

        #endregion

        if (Input.GetMouseButtonUp(0))
        {
            isClickStarted = false;
        }

        if(currentInteractable != null)
        {
            if (currentInteractable.isHighlightable)
            {
                if (currentInteractable.GetComponent<Outline>())
                {
                    currentInteractable.GetComponent<Outline>().enabled = true;
                    currentInteractable.GetComponent<Outline>().OutlineColor = Color.yellow;
                    currentInteractable.GetComponent<Outline>().OutlineWidth = 10;
                }
            }
        }

    }

    private void ClickTimer(bool isCStarted) //This should run in the Update to add time to clickTime when isClickedStarted is true
    {
        if (isCStarted)
        {
            clickTime += Time.deltaTime;
        }
        else
        {
            clickTime = 0;
        }
    }

    public Interactable GetCurrentInteractable()
    {
        if (currentInteractable)
        {
            return currentInteractable;
        }
        else
        {
            return null;
        }
    }

    public void SetInteractable(Interactable i) //*OPTIONAL* Custom function to handle setting the interactable container (in this case, interactable container is currentInteractable)
    {
        currentInteractable = i;

        if (inspectionManager)
        {
            if (!inspectionManager.isCurrentlyInspecting)
            {
                if (i.GetComponent<Inspectable>())
                {
                    inspectionManager.CurrentInspectedObj = i.GetComponent<Inspectable>();
                }
            }
        }
        
        if (itemController)
        {
            if (currentInteractable.GetComponent<Item>())
            {
                itemController.selectedItem = currentInteractable.GetComponent<Item>();
            }
        }
    }

    public void DeselectInteractable() //*OPTIONAL* Custom function to deselect and clear the interactable container (in this case, interactable container is currentInteractable)
    {
        if (currentInteractable != null)
        {
            currentInteractable.Deselect();
        }

        if (inspectionManager)
        {
            if (inspectionManager.isCurrentlyInspecting)
            {
                inspectionManager.EndInspect();
            }
            else
            {
                inspectionManager.CurrentInspectedObj = null;
            }
        }

        if (itemController)
        {
            itemController.selectedItem = null;
        }

        if (currentInteractable != null)
        {
            if (currentInteractable.GetComponent<Outline>())
            {
                currentInteractable.GetComponent<Outline>().enabled = false;
                currentInteractable.GetComponent<Outline>().OutlineColor = Color.white;
                currentInteractable.GetComponent<Outline>().OutlineWidth = 8;
            }
        }

        currentInteractable = null;

        if(currentInteractable == null)
        {
            OnNullDeselect.Invoke();
        }
    }


}
