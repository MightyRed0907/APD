using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
//THIS IS JUST A SAMPLE DRAG SCRIPT
public class Draggable : MonoBehaviour
{
    public bool isBeingDragged;
    public List<UnityEvent> OnStartDrag = new List<UnityEvent>();
    public Event OnEndDrag;
    public bool isOverPlaceableArea;
    public Vector3 dragPosOffset;
    private Rigidbody rigidBody;

    private Vector3 originalPos;
    private bool origPosRecorded;

    private bool dragEnabled = true;

    private float dist;
    private Vector3 v3Offset;
    private Plane plane;


    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody)
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
        }
    }

    public void Drag()
    {
        if (!dragEnabled) return;
        if (!origPosRecorded)
        {
            originalPos = transform.position;
            origPosRecorded = true;
        }

        
        //rigidBody.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, rigidBody.position.z - Camera.main.transform.position.z));
        
        //rigidBody.useGravity = false;
        isBeingDragged = true;
        
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float dist;
        plane.Raycast(ray, out dist);
        Vector3 v3Pos = ray.GetPoint(dist);
        Vector3 newv3Pos = new Vector3(v3Pos.x + dragPosOffset.x, v3Pos.y + dragPosOffset.y, v3Pos.z + dragPosOffset.z);
        //transform.position = v3Pos + v3Offset;
        transform.position = newv3Pos + v3Offset + dragPosOffset;
        
    }

    public void EndDrag()
    {
        if (!dragEnabled) return;
        //rigidBody.useGravity = true;
        origPosRecorded = false;
        isBeingDragged = false;
    }

    public void ReturnToOriginalPos()
    {
        if (!dragEnabled || !isBeingDragged) return;
        transform.position = originalPos;
    }

    public void EnableDrag()
    {
        dragEnabled = true;
    }

    public void DisableDrag()
    {
        dragEnabled = false;
    }

    void OnMouseDown()
    {
        plane.SetNormalAndPosition(Camera.main.transform.forward, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float dist;
        plane.Raycast(ray, out dist);
        v3Offset = transform.position - ray.GetPoint(dist);
    }

    /*
    void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        plane.Raycast(ray, out dist);
        Vector3 v3Pos = ray.GetPoint(dist);
        transform.position = v3Pos + v3Offset;
    }
    */
}
