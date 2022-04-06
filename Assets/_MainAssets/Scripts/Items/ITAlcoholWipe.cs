using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ITAlcoholWipe : Item
{
    public Transform refCam;
    public ClickSwipeModule CSwipeModule;
    public bool isStarted;
    public UnityEvent OnUse;
    public UnityEvent OnFinish;
    public Image SwipeCursor;
    private Coroutine currentModuleSession;
    private Transform currentCam;
    private bool isCompleted;

    public override bool UseItem()
    {
        if (!base.UseItem()) return false;

        if (CSwipeModule)
        {
            if (CSwipeModule.isActive)
            {
                if (currentModuleSession != null)
                {
                    StopCoroutine(currentModuleSession);
                }
                currentModuleSession = StartCoroutine(IStartCleanModule());
                OnUse.Invoke();
            }
        }

        return true;
    }

    public IEnumerator IStartCleanModule()
    {
        currentCam = Camera.main.GetComponent<CameraMovementController>().CurrentRefCam();
        Camera.main.GetComponent<CameraMovementController>().MoveCameraToGuide(refCam);
        CSwipeModule.isActive = true;
        while (Camera.main.GetComponent<CameraMovementController>().IsCurrentlyLerping())
        {
            yield return null;
        }

        transform.position = new Vector3(2000, 2000, 2000);
        CSwipeModule.EnableSwipeModules();
        isStarted = true;
        yield break;
    }

    public void EndCleanModule(float delay)
    {
        StartCoroutine(IEndCleanModule(delay));
    }

    public IEnumerator IEndCleanModule(float delay)
    {
        yield return new WaitForSeconds(delay);
        CSwipeModule.DisableSwipeModules();
        Camera.main.GetComponent<CameraMovementController>().MoveCameraToGuide(currentCam);
        CSwipeModule.isActive = false;
        isStarted = false;
        yield break;
    }

    public void AutoEndModule()
    {
        StartCoroutine(IAutoEndModule());
    }

    public IEnumerator IAutoEndModule()
    {
        while (GetComponent<ObjectLerper>().IsCurrentlyLerping()) yield return null;
        transform.position = new Vector3(2000, 2000, 2000);
        //CSwipeModule.DisableSwipeModules();
        CSwipeModule.isActive = false;
        isStarted = false;
        yield break;
    }

    public void Update()
    {
        if (isStarted)
        {
            if (!SwipeCursor.gameObject.activeSelf)
            {
                SwipeCursor.gameObject.SetActive(true);
            }
            if (SwipeCursor)
            {
                SwipeCursor.transform.position = Input.mousePosition;
            }
        }
        else
        {
            if (SwipeCursor.transform.position != new Vector3(4132, -866, -119))
            {
                SwipeCursor.gameObject.transform.position = new Vector3(4132, -866, -119);
            }
        }
    }
}
