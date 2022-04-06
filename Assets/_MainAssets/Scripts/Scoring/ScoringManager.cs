using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringManager : MonoBehaviour
{
    public GameManager GameManager;
    public GGameStage CurrentStage;
    public GStagePhase CurrentPhase;
    public int OverallScore;
    public int CurrentStageScore;
    public int CurrentPhaseScore;
    public float OverallAccuracy;
    public float CurrentStageAccuracy;
    public float CurrentPhaseAccuracy;

    private int currentStageIndex;
    private int currentPhaseIndex;
    private int currentModuleIndex;

    public int CurrentPSequenceScore;


    public void AdjustScore()
    {
        OverallScore += 1;
    }

    public void UpdateTotalStageScore()
    {
        CurrentStageScore = GetTotalStageScore(CurrentStage);
    }

    public int GetTotalStageScore(GGameStage gStage)
    {
        int stageScore = 0;

        foreach(GStagePhase phase in gStage.Phases)
        {
            stageScore += GetTotalPhaseScore(phase);
        }
        return stageScore;
    }
    
    public int GetTotalPossibleStageScore(GGameStage gStage)
    {
        int stageScore = 0;

        foreach(GStagePhase phase in gStage.Phases)
        {
            stageScore += GetTotalPhaseScore(phase);
        }
        return stageScore;
    }

    public int GetTotalPhaseScore(GStagePhase sPhase)
    {
        int score = 0;

        foreach(GPhaseModule pm in sPhase.Modules)
        {
            score += pm.GetScorePoints();
        }

        CurrentPhaseScore = score;
        return score;
    }
    
    public int GetTotalPossiblePhaseScore(GStagePhase sPhase)
    {
        int score = 0;

        foreach(GPhaseModule pm in sPhase.Modules)
        {
            score += pm.GetPossiblePoints();
        }

        CurrentPhaseScore = score;
        return score;
    }
    
    public int GetTotalPossibleSeqPhaseScore(GStagePhase sPhase)
    {
        int seqScore = 0;

        foreach(GPhaseModule pm in sPhase.Modules)
        {
            seqScore += pm.GetPossiblePoints();
        }

        CurrentPSequenceScore = seqScore;
        return seqScore;
    }
    
    public int GetTotalModuleScore(GPhaseModule pModule)
    {
        int score = 0;

        


        return score;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(GetTotalStageScore(CurrentStage));
        }
    }
}
