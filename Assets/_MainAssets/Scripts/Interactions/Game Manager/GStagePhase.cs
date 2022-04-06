using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GStagePhase : MonoBehaviour
{
    public int PhaseIndex;
    public string PhaseName;
    public bool IsFinished;
    public bool IsSequential;
    public bool IsModuleInterchangeable;
    public List<GPhaseModule> Modules = new List<GPhaseModule>();
    [Space(10)]
    [Header("Phase Config")]
    public List<InteractionArea> IAToDisableOnActivate = new List<InteractionArea>();
    public List<InteractionArea> IAToEnableOnActivate = new List<InteractionArea>();
    public List<Interactable> InteractablesToDisableOnActivate = new List<Interactable>();
    public List<Interactable> InteractablesToEnableOnActivate = new List<Interactable>();
    [Space(10)]
    public UnityEvent OnActivate;
    [Space(10)]
    public UnityEvent OnFinish;
    [HideInInspector]
    public GGameStage stageParent;

    [Space(10)]
    [Header("Stage Prerequisites")]
    public List<GStagePhase> reqPhase = new List<GStagePhase>();
    public int sequencePoints;
    public int possibleSeqPoint;
    [SerializeField]
    private bool isSequencePointSet;
    public float phaseTime;

    public void Start()
    {
        foreach(GPhaseModule gpm in Modules)
        {
            gpm.SetPhaseParent(this);
        }
    }

    public void StartPhase()
    {
        if (IsSequential)
        {
            if (!IsModuleInterchangeable)
            {
                foreach (GPhaseModule m in Modules)
                {
                    m.SetInteractablesState(false, false);
                }
            }
            if (Modules.Count > 0)
            {
                Debug.Log("Starting Module : " + GetStartingModule().ModuleName);
                GPhaseModule startMod = GetStartingModule();
                startMod.SetInteractablesState(true, false);
            }

            ToggleInteractionAreas(false, IAToDisableOnActivate, true);
            ToggleInteractionAreas(true, IAToEnableOnActivate);
            ToggleInteractables(false, InteractablesToDisableOnActivate, true);
            ToggleInteractables(true, InteractablesToEnableOnActivate);
            OnActivate.Invoke();
        }

        
    }

    public void ToggleInteractables(bool state, List<Interactable> iList, bool setSequentially = false)
    {
        foreach (Interactable i in iList)
        {
            i.GetComponent<Interactable>().isInteractable = state;
            i.GetComponent<Interactable>().isSetSequentially = setSequentially;
        }
    }

    public void ToggleInteractionAreas(bool state, List<InteractionArea> iaList, bool setSequentially = false)
    {
        foreach(InteractionArea ia in iaList)
        {
            ia.GetComponent<Interactable>().isInteractable = state;
            ia.GetComponent<Interactable>().isSetSequentially = setSequentially;
        }
    }

    public void SetStageParent(GGameStage sParent)
    {
        stageParent = sParent;
    }

    public void UnlockNextModule(GPhaseModule currentModule)
    {
        
        if (GetNextModule(currentModule))
        {
            GPhaseModule nModule = GetNextModule(currentModule);
            if (!nModule.IsFinished)
            {
                nModule.SetInteractablesState(true, false);
                nModule.StartModule();
            }
        }
    }

    public GPhaseModule GetNextModule(GPhaseModule currentModule)
    {
        GPhaseModule nextModule = null;
        int nextIndex = currentModule.ModuleIndex + 1;
        foreach (GPhaseModule gpM in Modules)
        {
            if (gpM.ModuleIndex > currentModule.ModuleIndex)
            {
                int maxIndex = gpM.ModuleIndex;
                if (maxIndex <= nextIndex && maxIndex > currentModule.ModuleIndex)
                {
                    int modIndex = Modules.IndexOf(gpM);
                    nextIndex = modIndex;
                }
            }
        }

        if(nextIndex >= Modules.Count)
        {
            nextModule = null;
        }
        else
        {
            nextModule = Modules[nextIndex];
        }
        

        return nextModule;
    } 
    
    public GPhaseModule GetStartingModule()
    {
        GPhaseModule nextModule = null;
        int nextIndex = 0;
        foreach (GPhaseModule gpM in Modules)
        {
            if (gpM.ModuleIndex <= 0)
            {
                int currIndex = gpM.ModuleIndex;
                int modIndex = Modules.IndexOf(gpM);
                if (currIndex <= Modules[modIndex].ModuleIndex)
                {
                    nextIndex = modIndex;
                } 
            }
        }

        if(Modules.Count <= 0)
        {
            nextModule = null;
        }
        else
        {
            nextModule = Modules[nextIndex];
        }
        

        return nextModule;
    }

    public void CheckPhaseStatus()
    {
        stageParent.CheckStageStatus();
        SetSequencePoint();
        if (IsFinished) return;
        phaseTime = GameManager.GameDuration;
        IsFinished = IsPhaseComplete();
        if (stageParent.IsSequential)
        {
            if (IsFinished)
            {
                stageParent.UnlockNextPhase(this);
                OnFinish.Invoke();
            }
        }

        Debug.Log("Checked stage status for " + name);
    }

    public void SetSequencePoint()
    {
        if (reqPhase.Count <= 0 || isSequencePointSet) return;

        possibleSeqPoint = 1;

        if (IsReqPhasesFinished())
        {
            sequencePoints = 1;
        }
        else
        {
            sequencePoints = 0;
        }

        isSequencePointSet = true;
        Debug.Log("Sequence Point Set for " + PhaseName);
    }

    public bool IsReqPhasesFinished()
    {
        bool isFin = true;
        foreach(GStagePhase phase in reqPhase)
        {
            if (!phase.IsFinished)
            {
                isFin = false;
                break;
            }
        }

        return isFin;
    }

    
    public bool IsPhaseComplete()
    {
        bool isComplete = true;

        foreach(GPhaseModule pMod in Modules)
        {
            if (pMod.IsFinished == false)
            {
                isComplete = false;
                break;
            }
        }

        return isComplete;
    }

    public virtual int GetSequencePoint()
    {
        return sequencePoints;
    }

    public virtual int GetPossibleSequencePoints()
    {
        return possibleSeqPoint;
    }
}
