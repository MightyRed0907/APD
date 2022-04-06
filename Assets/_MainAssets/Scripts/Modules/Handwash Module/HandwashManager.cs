using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum HWType
{
    Handwash,
    Handrub
}

public class HandwashManager : MonoBehaviour
{
    public HWType HandwashType;
    public Canvas handwashCanvas;
    public TMP_Text gameInstructions;
    public Button DoneBtn;
    public bool showLabel;
    public bool showIncorrectStep;
    public bool isStepsStatic;
    public Transform stepContainerBG;
    public Transform stepContainer;
    public List<HWStep> HWSteps = new List<HWStep>();
    public List<HWStep> PlacedHWSteps = new List<HWStep>();
    public Vector2 containerSizeOverride;
    public Color incorrectColor;
    public Color correctColor;
    [SerializeField]
    private bool isOverHManager;
    private RaycastHit hManagerHit = new RaycastHit();
    private PointerEventData cursor = new PointerEventData(EventSystem.current);

    private Ray ray;
    private RaycastHit hit;
    private RaycastResult hwManagerHit = new RaycastResult();
    private List<RaycastResult> objectsHit = new List<RaycastResult>();
    private List<Vector3> HWStepsSpawn = new List<Vector3>();

    public void Start()
    {
        SaveHWStepsSpawn();
        HWStepsSpawn = RandomizedList(HWStepsSpawn);

        //PrepareHandwash(true, true);
        
    }

    public void SetHandwashType(HWType type)
    {
        HandwashType = type;
    }

    public void SetStepMovemement(bool isStatic)
    {
        foreach (HWStep s in HWSteps)
        {
            s.GetComponent<PBubbleItem>().isStatic = isStatic;
            if (isStatic)
            {
                s.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
            else
            {
                s.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                s.GetComponent<PBubbleItem>().StartRandomMove();
            }
        }
    }

    public void SaveHWStepsSpawn()
    {
        foreach(HWStep s in HWSteps)
        {
            HWStepsSpawn.Add(s.transform.position);
        }
    }

    public void SetTitle(string newTitle)
    {
        gameInstructions.text = newTitle;
    }

    public List<Vector3> RandomizedList(List<Vector3> origList)
    {
        List<Vector3> oList = new List<Vector3>();
        foreach(Vector3 v in origList)
        {
            oList.Add(v);
        }

        List<Vector3> newList = new List<Vector3>();

        for(int i = 0; i < origList.Count; i++)
        {
            int randInt = Random.Range(0, oList.Count );
            newList.Add(oList[randInt]);
            oList.RemoveAt(randInt);
        }

        return newList;
    }

    public void PlaceHWStep(HWStep hws)
    {
        if (IsHoveringHWManager(objectsHit))
        {
            if (PlacedHWSteps.Contains(hws))
            {
                PlacedHWSteps.Remove(hws);
                hws.transform.SetParent(hws.transform.GetComponent<PBubbleItem>().origParent);
                hws.transform.SetParent(stepContainer);
                PlacedHWSteps.Add(hws);
            }
            else
            {
                hws.transform.SetParent(stepContainer);
                PlacedHWSteps.Add(hws);
                if (hws.transform.GetComponent<PBubbleItem>())
                {
                    if (!isStepsStatic)
                    {
                        hws.transform.GetComponent<PBubbleItem>().isStatic = true;
                    }
                }
                if (!isStepsStatic)
                {
                    hws.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    hws.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                }
            }
            if (showIncorrectStep)
            {
                CheckSequence();
            }
        }
        else
        {
            if (PlacedHWSteps.Contains(hws))
            {
                PlacedHWSteps.Remove(hws);
                if (showIncorrectStep)
                {
                    hws.GetComponent<Image>().color = correctColor;
                }
            }
            if (hws.transform.GetComponent<PBubbleItem>())
            {
                hws.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                if (!isStepsStatic)
                {
                    hws.transform.GetComponent<PBubbleItem>().isStatic = false;
                    hws.transform.GetComponent<Rigidbody2D>().velocity = hws.transform.GetComponent<PBubbleItem>().pauseVelocity;
                }
                hws.transform.SetParent(hws.transform.GetComponent<PBubbleItem>().origParent);
                if (isStepsStatic)
                {
                    hws.transform.position = hws.transform.GetComponent<PBubbleItem>().origPos;
                }
            }
            if (showIncorrectStep)
            {
                CheckSequence();
            }
        }

        if (gameInstructions)
        {
            if (PlacedHWSteps.Count <= 0)
            {
                gameInstructions.gameObject.SetActive(true);
            }
            else
            {
                gameInstructions.gameObject.SetActive(false);
            }
        }

        if (IsStepsFilledOut())
        {
            if (showIncorrectStep)
            {
                if (IsStepsCorrect())
                {
                    DoneBtn.gameObject.SetActive(true);
                }
                else
                {
                    DoneBtn.gameObject.SetActive(false);
                }
            }
            else
            {
                DoneBtn.gameObject.SetActive(true);
            }
        }
        else
        {
            DoneBtn.gameObject.SetActive(false);
        }
    }

    public void SetIsOverHManager(bool state)
    {
        isOverHManager = state;
    }

    public void RearrangeSWSteps()
    {
        List<Vector3> spawnPoints = RandomizedList(HWStepsSpawn);
        for(int i = 0; i < HWSteps.Count; i++)
        {
            HWSteps[i].transform.position = spawnPoints[i];
            HWSteps[i].GetComponent<PBubbleItem>().origPos = spawnPoints[i];
        }
    }

    public void OnMouseEnter()
    {
        SetIsOverHManager(true);
    }

    public void Update()
    {
        cursor.position = Input.mousePosition;
        
        EventSystem.current.RaycastAll(cursor, objectsHit);
        int count = objectsHit.Count;

        for (int i = 0; i < objectsHit.Count; i++)
        {
            RaycastResult rayRes = objectsHit[i];

            if (rayRes.gameObject.GetComponent<HandwashManager>())
            {
                hwManagerHit = rayRes;
            }
        }
    }

    public bool IsHoveringHWManager(List<RaycastResult> oH)
    {
        if (oH.Count <= 0) return false;

        if (oH.Contains(hwManagerHit))
        {
            return true;
            
        }
        else
        {
            return false;
        }
    }

    public bool IsStepsFilledOut()
    {
        if(PlacedHWSteps.Count >= HWSteps.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckSequence()
    {
        foreach (HWStep pStep in PlacedHWSteps)
        {
            if (HWSteps.Contains(pStep))
            {
                if (IsInCorrectSequence(pStep))
                {
                    pStep.GetComponent<Image>().color = correctColor;
                }
                else
                {
                    pStep.GetComponent<Image>().color = incorrectColor;
                }
            }
            else
            {
                pStep.GetComponent<Image>().color = incorrectColor;
            }
        }
    }

    public bool IsStepsCorrect()
    {
        bool correct = true;
        
        for(int i = 0; i < PlacedHWSteps.Count; i++)
        {
            if(PlacedHWSteps[i] != HWSteps[i])
            {
                correct = false;
            }
        }

        return correct;
    }

    public bool IsInCorrectSequence(HWStep step)
    {
        if (HWSteps.Contains(step))
        {
            if (PlacedHWSteps.Contains(step))
            {
                if(PlacedHWSteps.IndexOf(step) == HWSteps.IndexOf(step))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void UpdateContainer()
    {
        if (stepContainer)
        {
            stepContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(containerSizeOverride.x, containerSizeOverride.y);
        }
        if (stepContainerBG)
        {
            stepContainerBG.GetComponent<RectTransform>().sizeDelta = new Vector2(containerSizeOverride.x, containerSizeOverride.y);
        }
    }

    public void ReenableBubbleMovement()
    {
        for(int i = 0; i < HWSteps.Count; i++)
        {
            HWSteps[i].GetComponent<PBubbleItem>().ResumeMovement();
        }
    }

    public void PrepareHandwash(bool showMistake, bool showLabel, bool isStatic)
    {
        DoneBtn.gameObject.SetActive(false);
        isStepsStatic = isStatic;
        foreach(HWStep p in PlacedHWSteps)
        {
            p.GetComponent<Transform>().SetParent(p.GetComponent<PBubbleItem>().origParent);
            p.GetComponent<Image>().color = correctColor;
        }

        PlacedHWSteps.Clear();

        RearrangeSWSteps();

        SetStepMovemement(isStatic);

        if (!showLabel)
        {
            UpdateContainer();
        }

        showIncorrectStep = showMistake;

        if (showLabel)
        {
            foreach(HWStep step in HWSteps)
            {
                if (step.stepLabel)
                {
                    step.stepLabel.gameObject.SetActive(true);
                }
            }
        }

    }
}
