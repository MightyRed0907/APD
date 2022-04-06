using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class InteractionArea : MonoBehaviour
{
    public bool isActive;
    public bool interactionOnActive;
    public bool isAreaToggle;
    public bool isAreaToggledOn;
    public Transform referenceCamTransform;
    public float camPanSpeed = 1f;
    private InteractionAreaManager iaManager;

    public List<InteractionArea> SubAreas = new List<InteractionArea>();
    public List<Interactable> AreaInteractables = new List<Interactable>();

    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    private void Start()
    {
        iaManager = FindObjectOfType<InteractionAreaManager>();
    }

    public void EnableInteractables()
    {
        foreach(Interactable i in AreaInteractables.ToList())
        {
            if (!i.isSetSequentially)
            {
                i.GetComponent<Collider>().enabled = true;
                i.isInteractable = true;
            }
        }
    } 
    
    public void DisableInteractables()
    {
        foreach(Interactable i in AreaInteractables.ToList())
        {
            i.GetComponent<Collider>().enabled = false;
            i.isInteractable = false;
        }
    }
    public void RemoveAreaInteractable(Interactable i)
    {
        if (AreaInteractables.Contains(i))
        {
            AreaInteractables.Remove(i);
        }
    }
}
