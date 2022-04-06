using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserMode
{
    Patient,
    Caregiver
}

[CreateAssetMenu(fileName = "New_GameData", menuName = "GameData", order = 100)]
public class GameData : ScriptableObject
{
    public UserMode UserMode;
    public GameMode GameMode;
    public int StartStageIndex;
    public int StartPhaseIndex;
}
