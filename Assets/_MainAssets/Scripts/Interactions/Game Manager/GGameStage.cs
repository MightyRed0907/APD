using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GGameStage : MonoBehaviour
{
    public int StageIndex;
    public string StageName;
    public bool IsFinished;
    public bool IsSequential;
    public List<GStagePhase> Phases = new List<GStagePhase>();
    public GStagePhase startingPhase;
    public UnityEvent OnUpdate;
    public UnityEvent OnFinish;

    public void Start()
    {
        foreach (GStagePhase sgp in Phases)
        {
            sgp.SetStageParent(this);
        }
    }

    public void StartStage()
    {
        if (IsSequential)
        {
            if (Phases.Count > 0)
            {
                Debug.Log("Starting Module : " + GetStartingPhase().PhaseName);
                GStagePhase sPhase = GetStartingPhase();
                sPhase.StartPhase();
                startingPhase = sPhase;
            }
        }
    }

    public void CheckStageStatus()
    {
        OnUpdate.Invoke();
        if (IsFinished) return;
        IsFinished = IsStageComplete();
        if (IsFinished)
        {
            OnFinish.Invoke();
        }
        
    }

    public bool IsStageComplete()
    {
        bool isComplete = true;

        foreach (GStagePhase sPhase in Phases)
        {
            if (sPhase.IsFinished == false)
            {
                isComplete = false;
                break;
            }
        }

        return isComplete;
    }

    public void UnlockNextPhase(GStagePhase currentPhase)
    {
        if (GetNextPhase(currentPhase))
        {
            GStagePhase nPhase = GetNextPhase(currentPhase);
            nPhase.StartPhase();
        }
    }

    public GStagePhase GetNextPhase(GStagePhase currentPhase)
    {
        GStagePhase nextPhase = null;
        int nextIndex = currentPhase.PhaseIndex + 1;
        foreach (GStagePhase gsp in Phases)
        {
            if (gsp.PhaseIndex > currentPhase.PhaseIndex)
            {
                int maxIndex = gsp.PhaseIndex;
                if (maxIndex <= nextIndex && maxIndex > currentPhase.PhaseIndex)
                {
                    int modIndex = Phases.IndexOf(gsp);
                    nextIndex = modIndex;
                }
            }
        }

        if (nextIndex >=  Phases.Count)
        {
            nextPhase = null;
        }
        else
        {
            nextPhase = Phases[nextIndex];
        }


        return nextPhase;
    }

    public GStagePhase GetStartingPhase()
    {
        GStagePhase nextPhase = null;
        int nextIndex = 0;
        foreach (GStagePhase gpM in Phases)
        {
            if (gpM.PhaseIndex <= 0)
            {
                int currIndex = gpM.PhaseIndex;
                int modIndex = Phases.IndexOf(gpM);
                if (currIndex <= Phases[modIndex].PhaseIndex)
                {
                    nextIndex = modIndex;
                }
            }
        }

        if (Phases.Count <= 0)
        {
            nextPhase = null;
        }
        else
        {
            nextPhase = Phases[nextIndex];
        }


        return nextPhase;
    }

}
