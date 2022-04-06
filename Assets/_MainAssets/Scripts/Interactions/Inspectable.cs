using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public enum RescaleType
{
    none,
    enlarge,
    shrink
}

[RequireComponent(typeof(ObjectLerper))]
public class Inspectable : MonoBehaviour
{
    public bool IsCurrentlyInspected;

    public Vector3 inspectPosition;
    public Quaternion inspectRotation = new Quaternion(40, -90, -40, -90);
    public RescaleType RescaleType;
    public float rescaleFactor = 1f;
    public bool centerOnCamera = true;
    public float centerOnCameraZPos = 2.5f;
    public bool deselectOnEndInspect;

    private Rigidbody rigidBody;
    private Transform originalParent;
    private Vector3 originalPos;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private ObjectLerper oLerper;
    private InspectionManager inspectionManager;
}
