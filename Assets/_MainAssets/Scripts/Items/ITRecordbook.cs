using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ITRecordbook : Item
{
    public Transform cover;
    public Transform coverRend;
    public Material uiShader;
    public Canvas uiCanvas;
    private bool isOpened;

    public Transform TargetParent;
    public float UseSpeed;
    public Vector3 UIPos;
    public Quaternion UIRotation;
    public float ScaleFactor = 1;
    public Vector3 ToggleOnPos;
    public Vector3 ToggleOnRot;
    public float ToggleOnScaleFactor;
    public float ToggleSpeed = 0.3f;
    private UnityEvent OpenBookEvent = new UnityEvent();
    public UnityEvent OnUseRecordbook;

    private ObjectLerper oLerper;
    private ObjectRotator oRotator;
    private bool isMovingToUI;
    [SerializeField]
    private Shader origShader;

    public void Start()
    {
        oLerper = GetComponent<ObjectLerper>();
        oRotator = GetComponent<ObjectRotator>();
        if (!TargetParent)
        {
            TargetParent = Camera.main.transform;
        }

        origShader = uiShader.shader;

        OpenBookEvent.AddListener(ToggleBook);
    }

    public override bool UseItem()
    {
        if (!base.UseItem()) return false;

        MoveToUI();
        OnUseRecordbook.Invoke();

        return true;
    }

    public void ForceUseCCPD()
    {
        StartCoroutine(IForceUseCCPD());
    }

    public IEnumerator IForceUseCCPD()
    {
        while (oLerper.IsCurrentlyLerping()) yield return null;
        MoveToUI();
        OnUseRecordbook.Invoke();
        yield break;
    }

    public void ToggleBook()
    {
        if (oLerper.IsCurrentlyLerping()) return;
        StartCoroutine(IToggleBook());
    }

    public IEnumerator IToggleBook()
    {
        if (isOpened)
        {
            CloseRBook();
            MoveToOrigUIPos();
            if (uiCanvas)
            {
                uiCanvas.gameObject.SetActive(false);
            }
        }
        else
        {
            ShowRBook();
        }

        yield break;
    }

    public void ShowRBook()
    {
        if (isOpened) return;
        OpenRBook();
        MoveToView();
        if (uiCanvas)
        {
            uiCanvas.gameObject.SetActive(true);
        }
    }
    
    public void KeepRBook()
    {
        if (!isOpened) return;
        CloseRBook();
        MoveToOrigUIPos();
        if (uiCanvas)
        {
            uiCanvas.gameObject.SetActive(false);
        }
    }

    public void MoveToUI()
    {
        StartCoroutine(IMoveToUI());
    }

    public IEnumerator IMoveToUI()
    {
        if (oLerper.IsCurrentlyLerping()) yield break;

        isMovingToUI = true;

        transform.SetParent(TargetParent);

        if (oLerper)
        {
            oLerper.LocalLerpTowards(UIPos, UseSpeed);
        }

        if (oRotator)
        {
            oRotator.LocalLerpRotation(UIRotation, UseSpeed);
        }

        if (!sLerping)
        {
            StartCoroutine(ILerpScale(new Vector3(transform.localScale.x * ScaleFactor, transform.localScale.y * ScaleFactor, transform.localScale.z * ScaleFactor), UseSpeed));
        }

        
        if (transform.GetComponent<Interactable>())
        {
            //transform.GetComponent<Interactable>().OnInteract.RemoveAllListeners();
            transform.GetComponent<Interactable>().OnDeselection.Invoke();
            transform.GetComponent<Interactable>().OnInteract = new UnityEvent();
            transform.GetComponent<Interactable>().isSelectable = false;

        }

        while(oLerper.IsCurrentlyLerping() || oRotator.IsCurrentlyLerping() || sLerping)
        {
            yield return null;
        }

        if (coverRend)
        {
            if (coverRend.GetComponent<Renderer>())
            {
                uiShader.shader = Shader.Find("Unlit/Texture");
            }
        }

        if (transform.GetComponent<TooltipConfig>())
        {
            Destroy(transform.GetComponent<TooltipConfig>());
        }

        if (transform.GetComponent<Interactable>())
        {
            transform.GetComponent<Interactable>().OnInteract.AddListener(()=> ShowRBook());
        }
        if (transform.GetComponent<Outline>())
        {
            transform.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
        }

        isMovingToUI = false;

        yield break;
    }

    public void MoveToView()
    {
        if (oLerper.IsCurrentlyLerping() || oRotator.IsCurrentlyLerping() || sLerping) return;
        StartCoroutine(IMoveToView());
    }

    public void MoveToOrigUIPos()
    {
        if (oLerper.IsCurrentlyLerping() || oRotator.IsCurrentlyLerping() || sLerping) return;
        StartCoroutine(IMoveToOrigUIPos());
    }

    public IEnumerator IMoveToOrigUIPos()
    {
        if (oLerper.IsCurrentlyLerping()) yield break;

        isMovingToUI = true;

        if (oLerper)
        {
            oLerper.LocalLerpTowards(UIPos, ToggleSpeed);
        }

        if (!sLerping)
        {
            StartCoroutine(ILerpScale(new Vector3(origScale.x, origScale.y, origScale.z), ToggleSpeed));
        }

        if (transform.GetComponent<Interactable>())
        {
            transform.GetComponent<Interactable>().isInteractable = true;
        }

        yield break;
    }

    private Vector3 origScale;

    public IEnumerator IMoveToView()
    {
        if (oLerper.IsCurrentlyLerping()) yield break;

        origScale = transform.localScale;

        isMovingToUI = true;

        transform.SetParent(TargetParent);

        if (oLerper)
        {
            oLerper.LocalLerpTowards(ToggleOnPos, ToggleSpeed);
        }

        if (!sLerping)
        {
            StartCoroutine(ILerpScale(new Vector3(transform.localScale.x * ToggleOnScaleFactor, transform.localScale.y * ToggleOnScaleFactor, transform.localScale.z * ToggleOnScaleFactor), ToggleSpeed));
        }

        if (transform.GetComponent<Interactable>())
        {
            transform.GetComponent<Interactable>().isInteractable = false;
        }

        if (transform.GetComponent<Outline>())
        {
            transform.GetComponent<Outline>().enabled = false;
        }

        yield break;
    }



    public void SetToUI()
    {
        transform.SetParent(TargetParent);

        transform.localPosition = UIPos;
        transform.localRotation = UIRotation;
        transform.localScale = new Vector3(transform.localScale.x * ScaleFactor, transform.localScale.y * ScaleFactor, transform.localScale.z * ScaleFactor);

        if (coverRend)
        {
            if (coverRend.GetComponent<Renderer>())
            {
                uiShader.shader = Shader.Find("Unlit/Texture");
            }
        }

    }

    public void OpenRBook()
    {
        if (cover)
        {
            if (cover.GetComponent<TMRotator>())
            {
                cover.GetComponent<TMRotator>().OnRotation();
                isOpened = true;
            }
        }
    }

    public void CloseRBook()
    {
        if (cover)
        {
            if (cover.GetComponent<TMRotator>())
            {
                cover.GetComponent<TMRotator>().OffRotation();
                isOpened = false;
            }
        }
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

    public void Update()
    {

    }

}
