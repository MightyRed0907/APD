using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPlacementManager : MonoBehaviour
{
    private InteractionManager interactionManager;

    private Ray ray;
    //private RaycastHit hit;
    [SerializeField]
    private OPArea highlightedOPArea;
    private OPArea currentOPArea;
    private int hitCount;
    private RaycastHit opHit;
    private OPItem activeItem;
    private CameraMovementController mainCameraController;
    private Transform currentRefCam;
    private bool isMovingToRef;

    private OPItem itemList;

    private void Start()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
        mainCameraController = Camera.main.GetComponent<CameraMovementController>();
    }

    public void Update()
    {
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

            if (hit.collider.GetComponent<OPArea>())
            {
                /*
                if (highlightedOPArea)
                {
                    if (hit.collider.GetComponent<OPArea>() != highlightedOPArea)
                    {
                        highlightedOPArea.HideArea(highlightedOPArea);
                        highlightedOPArea = null;
                        opHit = new RaycastHit();
                    }
                }
                */

                if (interactionManager)
                {
                    if (interactionManager.GetCurrentInteractable() != null)
                    {
                        Interactable newInt = interactionManager.GetCurrentInteractable();
                        if (interactionManager.GetCurrentInteractable().GetComponent<OPItem>())
                        {
                            OPItem currentOpItem = newInt.GetComponent<OPItem>();
                            if (newInt.GetComponent<Draggable>())
                            {
                                Draggable currentDraggable = newInt.GetComponent<Draggable>();
                                if (newInt.GetComponent<Draggable>().isBeingDragged)
                                {
                                    if (hit.collider.GetComponent<OPArea>())
                                    {
                                        if (hit.collider.GetComponent<OPArea>().isActive)
                                        {
                                            //Debug.Log("Hit " + hit.collider.gameObject.name + " placement area");
                                            //highlightedOPArea = hit.collider.GetComponent<OPArea>();
                                            //Debug.Log(highlightedOPArea.gameObject.name + " is the placeable area : " + IsItemPlaceable(highlightedOPArea, currentOpItem));

                                            if (IsItemPlaceable(hit.collider.GetComponent<OPArea>(), currentOpItem))
                                            {
                                                activeItem = currentOpItem;
                                                currentOPArea = hit.collider.GetComponent<OPArea>();
                                                if (highlightedOPArea)
                                                {
                                                    if (currentOPArea)
                                                    {
                                                        if(highlightedOPArea != currentOPArea)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                                highlightedOPArea = hit.collider.GetComponent<OPArea>();
                                                highlightedOPArea.ShowArea(highlightedOPArea);
                                                opHit = hit;
                                                currentDraggable.isOverPlaceableArea = true;
                                                if (mainCameraController)
                                                {
                                                    if (highlightedOPArea.referenceCam != null)
                                                    {
                                                        if (!isMovingToRef && mainCameraController.transform.position != highlightedOPArea.referenceCam.transform.position)
                                                        {
                                                            isMovingToRef = true;
                                                            if (mainCameraController.CurrentRefCam())
                                                            {
                                                                currentRefCam = mainCameraController.CurrentRefCam();
                                                                //Debug.Log("FROM CAMERA : " + currentRefCam.gameObject.name);
                                                            }
                                                            //Debug.Log("move to reference camera");
                                                            mainCameraController.MoveCameraToGuide(highlightedOPArea.referenceCam.transform, 1f, 1f);
                                                            //Debug.Log("TO CAMERA : " + mainCameraController.CurrentRefCam().gameObject.name);
                                                        }
                                                    }
                                                }

                                                if (Input.GetMouseButtonUp(0))
                                                {
                                                    if (highlightedOPArea.AreaType == OPAreaType.slot)
                                                    {
                                                        if (highlightedOPArea.GetAvailableSlots() > 0)
                                                        {
                                                            //Debug.Log("MOUSE BUTTON UP ON OP : " + GetItemConfig(highlightedOPArea.Items, currentOpItem));
                                                            highlightedOPArea.PlaceObject(GetItemConfig(highlightedOPArea.Items, currentOpItem));
                                                            currentDraggable.EndDrag();
                                                            currentDraggable.isOverPlaceableArea = false;
                                                            interactionManager.DeselectInteractable();
                                                        }
                                                        else
                                                        {

                                                            currentDraggable.ReturnToOriginalPos();
                                                            currentDraggable.EndDrag();
                                                            currentDraggable.isOverPlaceableArea = false;
                                                            interactionManager.DeselectInteractable();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Debug.Log("MOUSE BUTTON UP ON OP : " + GetItemConfig(highlightedOPArea.Items, currentOpItem).Item.name);
                                                        highlightedOPArea.PlaceObject(GetItemConfig(highlightedOPArea.Items, currentOpItem));
                                                        currentDraggable.EndDrag();
                                                        currentDraggable.isOverPlaceableArea = false;
                                                        interactionManager.DeselectInteractable();
                                                    }
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        if (rHits.Count <= 0 || !rHits.Contains(opHit))
        {
            if (highlightedOPArea)
            {
                highlightedOPArea.HideArea(highlightedOPArea);
                if (activeItem)
                {
                    if (activeItem.GetComponent<Draggable>())
                    {
                        activeItem.GetComponent<Draggable>().isOverPlaceableArea = false;
                    }
                }

                if (currentRefCam)
                {
                    mainCameraController.StopCameraMove();
                    Transform refCam = currentRefCam;
                    if (isMovingToRef)
                    {
                        mainCameraController.MoveCameraToGuide(refCam, 1.1f, 1.1f);
                        isMovingToRef = false;
                    }
                    currentRefCam = null;
                }
                highlightedOPArea = null;
                opHit = new RaycastHit();
            }
        }
    }

    public bool IsItemPlaceable(OPArea area, OPItem item)
    {
        bool isPresent = false;
        if(area.Items.Count > 0)
        {
            foreach(OPConfig oConf in area.Items)
            {
                if(oConf.Item == item)
                {
                    isPresent = true;
                    /*
                    if (oConf.isPlaced)
                    {
                        isPresent = false;
                    }
                    else
                    {
                        isPresent = true;
                    }
                    */
                }
            }
        }
        else
        {
            isPresent = false;
        }

        return isPresent;
    }

    public OPConfig GetItemConfig(List<OPConfig> configList, OPItem item)
    {
        OPConfig c = new OPConfig();
        foreach (OPConfig conf in configList)
        {
            if (conf.Item == item)
            {
                c = conf;
            }
        }
        return c;

    }
 
}

