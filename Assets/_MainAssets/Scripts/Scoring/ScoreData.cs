using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_ScoreData", menuName = "ScoreData", order = 100)]
public class ScoreData : ScriptableObject
{
    public List<SDStage> Stages = new List<SDStage>();
}

[System.Serializable]
public class SDStage
{
    public string StageName;
    public float StageScore;
    public List<SDPhase> Phases = new List<SDPhase>();
}
[System.Serializable]
public class SDPhase
{
    public string PhaseName;
    public float PhaseScore;
    public List<SDModule> Module = new List<SDModule>();
}

[System.Serializable]
public class SDModule
{
    public string ModuleName;
    public float ModuleScore;
}
