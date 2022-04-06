using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovementController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.3f;
    [SerializeField]
    private float rotSpeed = 0.3f;
    private ObjectLerper oLerper;
    private ObjectRotator oRotator;

    public Transform sampleGuide;
    public Transform defaultCam;

    private Coroutine currentMLerp;
    private Coroutine currentRLerp;
    private Transform currentRefCam;

    public Transform SceneCamView;

    public void SetRefCam(Transform t)
    {
        currentRefCam = t;
    }

    public void Start()
    {
        oLerper = GetComponent<ObjectLerper>();
        oRotator = GetComponent<ObjectRotator>();
    }

    public void MoveCamera(Vector3 position, Quaternion rotation)
    {
        oLerper.LerpTowards(position, moveSpeed);
        oRotator.LerpRotation(rotation, moveSpeed);
    }

    public void MoveToDefaultPos()
    {
        MoveCameraToGuide(defaultCam);
    }
    
    public void MoveCameraToGuide(Transform t, float mSpeed = 1f, float rSpeed = 1f)
    {
        if(oLerper.IsCurrentlyLerping() || oRotator.IsCurrentlyLerping())
        {
            StopCoroutine(currentMLerp);
            oLerper.StopMoveLerp();
            StopCoroutine(currentRLerp);
            oRotator.StopRotLerp();
        }
        currentRefCam = t;
        currentMLerp = StartCoroutine(oLerper.ILerpTowards(t.position, mSpeed));
        currentRLerp = StartCoroutine(oRotator.ILerpRotation(t.rotation, rSpeed));    
    }

    public void StopCameraMove()
    {
        if (oLerper.IsCurrentlyLerping() || oRotator.IsCurrentlyLerping())
        {
            StopCoroutine(currentMLerp);
            oLerper.StopMoveLerp();
            StopCoroutine(currentRLerp);
            oRotator.StopRotLerp();
        }
    }

    public bool IsFinishedLerping()
    {
        if(oLerper.IsCurrentlyLerping() || oRotator.IsCurrentlyLerping())
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    public bool IsCurrentlyLerping()
    {
        if(oLerper.IsCurrentlyLerping() || oRotator.IsCurrentlyLerping())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Transform CurrentRefCam()
    {
        if (currentRefCam)
        {
            return currentRefCam;
        }
        else
        {
            return null;
        }
    }

    public void GoToSceneCamView()
    {
        MoveCameraToGuide(SceneCamView);
    }

    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GoToSceneCamView();
        }
        
    }
}
