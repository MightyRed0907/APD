using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameConfigManager : MonoBehaviour
{
    public GameData GameData;
    public UserMode UserMode;
    public GameMode GameMode;
    public TMP_Dropdown UserMode_dd;
    public TMP_Dropdown GameMode_dd;
    
    public TMP_Dropdown Stage_dd;
    public TMP_Dropdown Phase_dd;

    public Transform StageSelect;
    public Transform ModeSelect;

    public void Start()
    {
        if (GameData)
        {
            GameData.UserMode = UserMode.Patient;
            GameData.GameMode = GameMode.Training;
            GameData.StartStageIndex = Stage_dd.value;
            GameData.StartPhaseIndex = Phase_dd.value;
        }
    }

    public void ShowStageSelect()
    {
        if (GameMode == GameMode.Assessment)
        {
            Phase_dd.value = 0;
            Phase_dd.interactable = false;
        }
        else
        {
            Phase_dd.value = GameData.StartPhaseIndex;
            Phase_dd.interactable = true;
        }
        Phase_dd.RefreshShownValue();
        GameData.StartPhaseIndex = Phase_dd.value;
        GameData.StartStageIndex = Stage_dd.value;
        StageSelect.gameObject.SetActive(true);
        ModeSelect.gameObject.SetActive(false);
    }
    
    public void ShowModeSelect()
    {
        StageSelect.gameObject.SetActive(false);
        ModeSelect.gameObject.SetActive(true);
    }

    public void SetStartingStage()
    {
        GameData.StartStageIndex = Stage_dd.value;
    }
    
    public void SetStartingPhase()
    {
        GameData.StartPhaseIndex = Phase_dd.value;
    }

    public void SetUserMode()
    {
        if(UserMode_dd.value == 0)
        {
            UserMode = UserMode.Patient;
        }
        else if(UserMode_dd.value == 1)
        {
            UserMode = UserMode.Caregiver;
        }
        else
        {
            UserMode = UserMode.Patient;
        }

        if (GameData)
        {
            GameData.UserMode = UserMode;
        }
    }
    
    public void SetGameMode()
    {
        if (GameMode_dd.value == 0)
        {
            GameMode = GameMode.Training;
        }
        else if (GameMode_dd.value == 1)
        {
            GameMode = GameMode.Practice;
        }
        else if (GameMode_dd.value == 2)
        {
            GameMode = GameMode.Assessment;
        }
        else
        {
            GameMode = GameMode.Training;
        }

        if (GameData)
        {
            GameData.GameMode = GameMode;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Test");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
