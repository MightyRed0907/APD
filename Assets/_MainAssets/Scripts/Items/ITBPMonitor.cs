using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public enum BPCategory
{
    random,
    lowbp,
    normal,
    elevated,
    hbpStage1,
    hbpStage2,
    htCrisis
}


public class BPData
{
    public BPCategory Category;
    public string categoryName;
    public Vector2 systolicRange;
    public Vector2 diastolicRange;
}

public class ITBPMonitor : Item
{
    public BPCategory BloodPressureCategory;
    public Image Backlight;
    public TMP_Text SysText;
    public TMP_Text DiasText;
    public TMP_Text PulseText;
    public Button UseButton;
    public Button UnuseButton;
    public float SessionDuration = 20;
    public bool IsRandomizeSessionDuration = true;
    public bool Zoomable = false;
    public float LerpSpeed = 1f;
    public float ZoomSpeed = 1f;

    public Vector3 useLocation;
    public Quaternion useRotation;
    public Vector3 useScale = new Vector3(1,1,1);
    public float scaleFactor = 1;

    public Vector2 RandomSessionRange = new Vector2(20, 35);
    private Transform obj;
    private bool isCurrentlyUsed;

    private bool isRunning;
    private Coroutine currentSession;
    private bool canStart;

    private Transform originalParent;
    private Vector3 originalPos;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private Vector3 origZoomPos;
    private float origZoomDistance;

    private ObjectLerper oLerper;
    private ObjectRotator oRotator;

    public UnityEvent OnUse;
    public UnityEvent OnUnuse;
    public UnityEvent OnSuccess;

    public Vector3 BPResults;

    public void Start()
    {
        oLerper = GetComponent<ObjectLerper>();
        oRotator = GetComponent<ObjectRotator>();
    }

    public void EnableBPMonitor()
    {
        if (currentSession != null)
        {
            StopCoroutine(currentSession);
            currentSession = null;
            isRunning = false;
            ClearDisplay();
            
        }
        
        if (Backlight)
        {
            Backlight.enabled = true;
        }
        UseButton.enabled = true;
    }
    
    public void DisableBPMonitor()
    {
        if (currentSession != null)
        {
            StopCoroutine(currentSession);
            currentSession = null;
            isRunning = false;
            ClearDisplay();
        }

        if (Backlight)
        {
            Backlight.enabled = false;
        }
        UseButton.enabled = false;
    }

    public Vector2 GetBP(BPCategory bpCat)
    {
        Vector2 bp = Vector2.zero;

        bp.x = RandomSystolic(bpCat);
        bp.y = RandomDiastolic(bpCat);

        //Debug.Log("Blood Pressure Monitor Results : SYS " + bp.x + " DIA : " + bp.y);

        return bp;
    }


    public int RandomSystolic(BPCategory bpCat)
    {
        int sysval = 120;
        if (bpCat == BPCategory.lowbp)
        {
            sysval = (int)Random.Range(60, 79);
        }
        else if (bpCat == BPCategory.normal)
        {
            sysval = (int)Random.Range(80, 119);
        }
        else if (bpCat == BPCategory.elevated)
        {
            sysval = (int)Random.Range(120, 129);
        }
        else if (bpCat == BPCategory.hbpStage1)
        {
            sysval = (int)Random.Range(130, 139);
        }
        else if (bpCat == BPCategory.hbpStage2)
        {

            sysval = (int)Random.Range(140, 180);
        }
        else if (bpCat == BPCategory.htCrisis)
        {
            sysval = (int)Random.Range(181, 200);
        }
        else
        {
            sysval = (int)Random.Range(90, 120);
        }

        return sysval;
    }

    public int RandomDiastolic(BPCategory bpCat)
    {
        int sysval = 120;
        if (bpCat == BPCategory.lowbp)
        {
            sysval = (int)Random.Range(50, 59);
        }
        else if (bpCat == BPCategory.normal)
        {
            sysval = (int)Random.Range(60, 80);
        }
        else if (bpCat == BPCategory.elevated)
        {
            sysval = (int)Random.Range(70, 79);
        }
        else if (bpCat == BPCategory.hbpStage1)
        {
            sysval = (int)Random.Range(80, 89);
        }
        else if (bpCat == BPCategory.hbpStage2)
        {
            sysval = (int)Random.Range(90, 120);
        }
        else if (bpCat == BPCategory.htCrisis)
        {
            sysval = (int)Random.Range(120, 140);
        }
        else
        {
            sysval = (int)Random.Range(60, 80);
        }

        return sysval;
    }

    public void Update()
    {
        if (Zoomable)
        {
            float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            float origDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
            //Debug.Log("Scroll Axis : " + Input.GetAxis("Mouse ScrollWheel"));

            if (distance > 0.04559885f && Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, distance * LerpSpeed * Input.GetAxis("Mouse ScrollWheel"));
            }
            else if (origDistance > origZoomDistance && Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, origZoomPos, origDistance * LerpSpeed * (Input.GetAxis("Mouse ScrollWheel") * -1));
            }
        }
    }

    public int RandomPulse()
    {
        int randP = (int)Random.Range(60, 100);
        //Debug.Log("Pulse Rate : " + randP);
        return randP;
    }

    public void UseBPMonitor()
    {
        StartCoroutine(IUseBPMonitor());
    }

    public IEnumerator IUseBPMonitor()
    {
        if (isCurrentlyUsed) yield break;
        isCurrentlyUsed = true;
        originalParent = transform.parent;
        originalPos = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
        

        if (transform.GetComponent<Interactable>())
        {
            transform.GetComponent<Interactable>().isHighlightable = false;
        }

        if (transform.GetComponent<Outline>())
        {
            transform.GetComponent<Outline>().enabled = false;
        }

        DisableOtherActions(transform);
        OnUse.Invoke();

        transform.SetParent(Camera.main.transform);

        oLerper.LocalLerpTowards(useLocation, LerpSpeed);
        oRotator.LerpRotation(useRotation, LerpSpeed);

        Vector3 newScale = new Vector3(transform.localScale.x * scaleFactor, transform.localScale.y * scaleFactor, transform.localScale.z * scaleFactor);
        //newScale = new Vector3(transform.localScale.x / CurrentInspectedObj.rescaleFactor, origScale.y / CurrentInspectedObj.rescaleFactor, origScale.z / CurrentInspectedObj.rescaleFactor);
        StartCoroutine(ILerpScale(newScale, LerpSpeed));

        while(oLerper.IsCurrentlyLerping() || oRotator.IsCurrentlyLerping() || sLerping)
        {
            yield return null;
        }

        
        if (Camera.main.GetComponent<Toggle>())
        {
            Camera.main.GetComponent<Toggle>().ToggleTargets();
        }

        if (Camera.main.GetComponent<CameraController>())
        {
            Camera.main.GetComponent<CameraController>().isControllable = false;
        }
        if (UseButton)
        {
            UseButton.enabled = true;
        }

        if (UnuseButton)
        {
            UnuseButton.gameObject.SetActive(true);
            UnuseButton.onClick.AddListener(() => StopUseBPMonitor());
        }

        
        origZoomPos = transform.position;
        origZoomDistance = Vector3.Distance(transform.position, origZoomPos);
        Zoomable = true;

        yield break;
    }

    public void StopUseBPMonitor()
    {
        if (!isCurrentlyUsed) return;

        if (currentSession != null)
        {
            StopCoroutine(currentSession);
            currentSession = null;
            if (isRunning)
            {
                ClearDisplay();
            }
            isRunning = false;
            
        }

        if (UnuseButton)
        {
            UnuseButton.onClick.RemoveAllListeners();
            UnuseButton.gameObject.SetActive(false);
        }

        transform.SetParent(originalParent);
        oRotator.LerpRotation(originalRotation, LerpSpeed);
        oLerper.LerpTowards(originalPos, LerpSpeed);
        StartCoroutine(ILerpScale(originalScale, LerpSpeed));

        if (Camera.main.GetComponent<Toggle>())
        {
            Camera.main.GetComponent<Toggle>().ToggleTargets();
        }

        if (Camera.main.GetComponent<CameraController>())
        {
            Camera.main.GetComponent<CameraController>().isControllable = true;
        }
        isCurrentlyUsed = false;
        UseButton.enabled = false;
        Zoomable = false;
        OnUnuse.Invoke();
        
        if (transform.GetComponent<Interactable>())
        {
            transform.GetComponent<Interactable>().isHighlightable = true;
        }
        if (transform.GetComponent<Draggable>())
        {
            transform.GetComponent<Draggable>().enabled = false;
        }
        if (transform.GetComponent<Outline>())
        {
            transform.GetComponent<Outline>().enabled = false;
        }
        EnableOtherActions(transform);
    }


    public override bool UseItem()
    {
        if (!base.UseItem()) return false;

        UseBPMonitor();

        return true;
    }

    public void StartSession()
    {
        if (currentSession != null)
        {
            StopCoroutine(currentSession);
            currentSession = null;
            isRunning = false;
            ClearDisplay();
            if (Backlight)
            {
                Backlight.enabled = false;
            }
            return;
        }

        if (Backlight)
        {
            Backlight.enabled = true;
        }

        if (IsRandomizeSessionDuration)
        {
            SessionDuration = Random.Range(RandomSessionRange.x, RandomSessionRange.y);
        }

         currentSession = StartCoroutine(IStartSession(BloodPressureCategory, SessionDuration));
    }

    public void ClearDisplay()
    {
        SysText.text = "";
        DiasText.text = "";
        PulseText.text = "";
    }

   

    public IEnumerator IStartSession(BPCategory bC, float duration)
    {
        if (isRunning) yield break;
        ClearDisplay();
        isRunning = true;
        Vector2 bpSet = GetBP(bC);
        float currentVal = 0;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float smooth = t / duration;
            currentVal = Mathf.Lerp(0, bpSet.x, smooth);
            int intCV = (int)currentVal;
            if (SysText)
            {
                SysText.text = intCV.ToString();
            }
            //Debug.Log("Current SysValue : " + currentVal);
            yield return null;
        }
        int pulse = RandomPulse();
        BPResults = new Vector3(bpSet.x, bpSet.y, pulse);
        if (SysText)
        {
            SysText.text = bpSet.x.ToString();
        }
        if (DiasText)
        {
            DiasText.text = bpSet.y.ToString();
        }
        if (PulseText)
        {
            
            PulseText.text = pulse.ToString();
        }

        
        isRunning = false;
        while(BPResults == Vector3.zero)
        {
            yield return null;
        }
        OnSuccess.Invoke();
        yield break;
    }

    public void FastDisplayResult()
    {
        StartCoroutine(IFastDisplayResult());
    }
    
    public IEnumerator IFastDisplayResult()
    {
        if (isRunning) yield break;
        ClearDisplay();
        isRunning = true;
        Vector2 bpSet = GetBP(BloodPressureCategory);
        int pulse = RandomPulse();

        BPResults = new Vector3(bpSet.x, bpSet.y, pulse);

        if (SysText)
        {
            SysText.text = bpSet.x.ToString();
        }
        if (DiasText)
        {
            DiasText.text = bpSet.y.ToString();
        }
        if (PulseText)
        {
            
            PulseText.text = pulse.ToString();
        }

        isRunning = false;
        OnSuccess.Invoke();

        yield break;
    }

    private bool sLerping;

    private IEnumerator ILerpScale(Vector3 eScale, float duration)
    {
        while (sLerping) yield return new WaitForSeconds(0.1f);
        sLerping = true;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = eScale;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float smooth = t / duration;
            smooth = smooth * smooth * (3f - 2f * smooth);
            transform.localScale = Vector3.Lerp(startScale, endScale, smooth);
            yield return null;
        }
        transform.localScale = endScale;
        sLerping = false;
        yield break;
    }

    public void EnableOtherActions(Transform obj)
    {
        if (obj.GetComponent<Interactable>())
        {
            if (!obj.GetComponent<Interactable>().isSetSequentially)
            {
                obj.GetComponent<Interactable>().EnableInteraction();
            }
        }

        if (obj.GetComponent<Draggable>())
        {
            obj.GetComponent<Draggable>().EnableDrag();
        }
    }

    public void DisableOtherActions(Transform obj)
    {
        if (obj.GetComponent<Interactable>())
        {
            obj.GetComponent<Interactable>().DisableInteraction();
        }
        if (obj.GetComponent<Draggable>())
        {
            obj.GetComponent<Draggable>().DisableDrag();
        }
    }

    public Vector3 GetLastBPResult()
    {
        return BPResults;
    }

}
