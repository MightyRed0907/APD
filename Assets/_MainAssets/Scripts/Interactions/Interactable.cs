using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    public bool isInteractable = true;
    public bool isHighlightable = true;
    public bool isSelectable = true;
    public bool IsCurrentlySelected;
    public UnityEvent OnInteract;
    public UnityEvent OnSelection;
    public UnityEvent OnDeselection;
    public bool isClickToDeselect = true;
    public bool isSetSequentially;

    private Outline outline;


    public virtual void Interact() //This is just a sample function to be invoked in the InteractionManager
    {
        if (!isInteractable) return;

        if (OnInteract == null) return;
        OnInteract.Invoke();
    }

    public void Deselect()
    {
        if (!isInteractable) return;
        OnDeselection.Invoke();
    }
    
    public void Select()
    {
        if (!isInteractable) return;
        OnSelection.Invoke();
    }

    public void DisableInteraction()
    {
        isInteractable = false;
    }
    
    public void EnableInteraction()
    {
        isInteractable = true;
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }

    public void Start()
    {
        outline = GetComponent<Outline>();
        if (outline) outline.enabled = false;
    }

    public void ExectueOnInteract()
    {
        OnInteract.Invoke();
    }
}
