using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GPhaseModule : MonoBehaviour
{
    public int ModuleIndex;
    public string ModuleName;
    public bool IsFinished;
    public List<OPArea> TargetAreas = new List<OPArea>();
    public List<Interactable> TargetInteractables = new List<Interactable>();
    [Space(10)]
    [Header("Module Config")]
    public List<InteractionArea> IAToDisableOnActivate = new List<InteractionArea>();
    public List<InteractionArea> IAToEnableOnActivate = new List<InteractionArea>();
    public List<Interactable> InteractablesToDisableOnActivate = new List<Interactable>();
    public List<Interactable> InteractablesToEnableOnActivate = new List<Interactable>();
    public UnityEvent OnActivate;
    public UnityEvent OnFinish;
    [HideInInspector]
    public GStagePhase phaseParent;
    public delegate void SetObjectiveState(bool state);
    public SetObjectiveState DoSetState;

    [Space(10)]
    [Header("Stage Prerequisites")]
    public List<GPhaseModule> reqMod = new List<GPhaseModule>();
    public int sequencePoints;
    public int possibleSeqPoint;
    private bool isSequencePointSet;

    [Space(10)]
    [Header("AutoFinish Events")]
    public UnityEvent OnAutofinish;

    [Space(10)]
    [Header("Distractors Activated")]
    public List<Distractor> DistractorsActivated = new List<Distractor>();

    public void SetPhaseParent(GStagePhase pParent)
    {
        phaseParent = pParent;
    }

    public void StartModule()
    {
        ToggleInteractionAreas(false, IAToDisableOnActivate, true);
        ToggleInteractionAreas(true, IAToEnableOnActivate, false);
        ToggleInteractables(false, InteractablesToDisableOnActivate, true);
        ToggleInteractables(true, InteractablesToEnableOnActivate, false);
        OnActivate.Invoke();
    }

    public void SetModuleStatus()
    {
        BSetModuleStatus();
        if (phaseParent.stageParent.IsSequential)
        {
            DoSetState(IsFinished);
        }
    }

    public virtual bool BSetModuleStatus()
    {
        UpdatePhaseParent();
        return true;
    }

    public void UpdatePhaseParent()
    {
        if (!phaseParent) return;
        phaseParent.CheckPhaseStatus();
        SetSequencePoint();
        
    }

    public void SetInteractablesState(bool state, bool setSequentially = true)
    {
        foreach(Interactable i in TargetInteractables)
        {
            i.isSetSequentially = setSequentially;
            i.isInteractable = state;
        }
    }

    public void ToggleInteractables(bool state, List<Interactable> iList, bool setSequentially = false)
    {
        foreach (Interactable i in iList)
        {
            i.GetComponent<Interactable>().isSetSequentially = setSequentially;
            i.GetComponent<Interactable>().isInteractable = state;
        }
    }

    public void ToggleInteractionAreas(bool state, List<InteractionArea> iaList, bool setSequentially = false)
    {
        foreach (InteractionArea ia in iaList)
        {
            ia.GetComponent<Interactable>().isSetSequentially = setSequentially;
            ia.GetComponent<Interactable>().isInteractable = state;
        }
    }

    public virtual int GetScorePoints()
    {
        int points = 0;

        if (IsFinished)
        {
            points = 1;
        }
        else
        {
            points = 0;
        }

        return points;
    }

    public virtual int GetPossiblePoints()
    {
        int points = 1;

        return points;
    }

    public void SetSequencePoint()
    {
        if (reqMod.Count <= 0 || isSequencePointSet) return;

        possibleSeqPoint = 1;

        if (IsReqModulesFinished())
        {
            sequencePoints = 1;
        }
        else
        {
            sequencePoints = 0;
        }

        isSequencePointSet = true;
    }

    public bool IsReqModulesFinished()
    {
        bool isFin = true;
        foreach (GPhaseModule mod in reqMod)
        {
            if (!mod.IsFinished)
            {
                isFin = false;
                break;
            }
        }

        return isFin;
    }


    public virtual int GetSequencePoint()
    {
        return sequencePoints;
    }

    public virtual int GetPossibleSequencePoints()
    {
        return possibleSeqPoint;
    }

    public void AutoFinish()
    {
        OnAutofinish.Invoke();
    }

    public void Update()
    {
        
    }
}
