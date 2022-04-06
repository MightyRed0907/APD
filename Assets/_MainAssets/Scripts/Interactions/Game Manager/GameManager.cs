using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameMode
{
    Training, 
    Practice,
    Assessment
}

public class GameManager : MonoBehaviour
{
    public GameData GameData;
    public GameMode GameMode;
    public ObjectivesManager ObjectivesManager;
    public TooltipManager TooltipManager;
    public ResultManager ResultManager;
    public HandwashModuleController HWController;
    public OpenableManager OpenableManager;
    public Transform LoadingScreen;
    public Transform DefaultCamera;
    public Transform LoadingCamera;
    public Transform InteractionBlocker;
    public List<GGameStage> GameStages = new List<GGameStage>();
    public List<Transform> SelectedDSol = new List<Transform>();
    public GGameStage CurrentStage;
    public GGameStage StartingStage;

    public Button SubmitBtn;
    public Button SubmitToggle;

    [Space(10)]
    [Header("Progress Report UI")]
    public Transform ProgressUI;
    public TMP_Text ProgressLabel;

    [Space(10)]
    [Header("Submit Confirmation UI")]
    public Button ExitBtn;
    public Transform SConfirmUI;
    public Transform ExitConfirmUI;

    public static float GameDuration;
    private static bool isGameRunning;
    public static bool isLoading;
    public static int selectedPhaseIndex;

    private float loadingSpeed = 1;
    private float loadingIntervalSpeed = 1.5f;


    public void Awake()
    {
        InteractionBlocker.gameObject.SetActive(true);
        SetMode();
    }

    public void Start()
    {
        isGameRunning = true;
        if (GameMode == GameMode.Training)
        {
            StartingStage.StartStage();
        }
    }

    public void SetMode()
    {
        if (!GameData) return;

        GameMode = GameData.GameMode;

        if(GameMode == GameMode.Training)
        {
            foreach(GGameStage stage in GameStages)
            {
                stage.IsSequential = true;
            }

            HWController.showLabels = true;
            HWController.showMistakes = true;
            HWController.isStaticSteps = true;

            SubmitToggle.gameObject.SetActive(false);
            LoadPracticeState(GameStages[GameData.StartStageIndex], GameStages[GameData.StartStageIndex].Phases[GameData.StartPhaseIndex]);
            OpenableManager.MassDisableOpenOption();
            
        }
        else if(GameMode == GameMode.Practice)
        {
            selectedPhaseIndex = GameData.StartPhaseIndex;
            foreach (GGameStage stage in GameStages)
            {
                stage.IsSequential = false;
            }
            HWController.showLabels = true;
            HWController.showMistakes = false;
            HWController.isStaticSteps = true;

            ObjectivesManager.objtvUI.gameObject.SetActive(false);
            SubmitToggle.gameObject.SetActive(true);
            LoadPracticeState(GameStages[GameData.StartStageIndex], GameStages[GameData.StartStageIndex].Phases[GameData.StartPhaseIndex]);
            OpenableManager.MassEnableOpenOption();
        }
        else
        {
            LoadingScreen.gameObject.SetActive(false);
            foreach (GGameStage stage in GameStages)
            {
                stage.IsSequential = false;
            }

            HWController.showLabels = true;
            HWController.showMistakes = false;
            HWController.isStaticSteps = false;

            ObjectivesManager.objtvUI.gameObject.SetActive(false);
            SubmitToggle.gameObject.SetActive(true);
            InteractionBlocker.gameObject.SetActive(false);
            OpenableManager.MassEnableOpenOption();
        }

        ResetGameDuration();
    }

    public UnityEvent OnSelectedDSolUpdate;

    public void AddToSelectedDSol(Transform obj)
    {
        SelectedDSol.Add(obj);
        OnSelectedDSolUpdate.Invoke();
    }
    
    public void DisableSelectedDSol()
    {
        foreach(Transform t in SelectedDSol)
        {
            if (t.GetComponent<Interactable>())
            {
                t.GetComponent<Interactable>().isSetSequentially = true;
                t.GetComponent<Interactable>().isInteractable = false;
            }
        }
    }

    public void ToggleExitConfirmation(bool show)
    {
        ExitConfirmUI.gameObject.SetActive(show);
        if (show)
        {
            isGameRunning = false;
        }
        else
        {
            isGameRunning = true;
        }
        if (Camera.main.GetComponent<CameraController>())
        {
            Camera.main.GetComponent<CameraController>().isControllable = !show;
        }
        DisableTooltips();
    }

    public void LoadMainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void EnableSelectedDSol()
    {
        foreach(Transform t in SelectedDSol)
        {
            if (t.GetComponent<Interactable>())
            {
                t.GetComponent<Interactable>().isSetSequentially = false;
                t.GetComponent<Interactable>().isInteractable = true;
            }
        }
    }
    public void RemoveFromSelectedDSol(Transform obj)
    {
        if (SelectedDSol.Contains(obj))
        {
            SelectedDSol.Remove(obj);
            OnSelectedDSolUpdate.Invoke();
        }
    }

    public IEnumerator IShowProgress(string label)
    {
        ObjectivesManager.objtvUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        if (!string.IsNullOrEmpty(label))
        {
            ProgressLabel.text = label;
        }
        else
        {
            ProgressLabel.text = "Phase Complete!";
        }

        ProgressUI.gameObject.SetActive(true);
        TooltipManager.HideTooltip();

        isGameRunning = false;

        yield break;
    }

    public void ShowProgressUI(string label)
    {
        if (GameManager.isLoading) return;
        StartCoroutine(IShowProgress(label));
    }
    
    public void HideProgressUI()
    {
        ProgressUI.gameObject.SetActive(false);
        ObjectivesManager.objtvUI.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ObjectivesManager.objtvUI.GetComponent<RectTransform>());
        UIReanchorTool.SetTopLeftAnchor(ObjectivesManager.objtvUI.GetComponent<RectTransform>());
        isGameRunning = true;
    }

    public void ToggleSubmitBtn(bool state)
    {
        SubmitBtn.gameObject.SetActive(state);
        SubmitToggle.gameObject.SetActive(!state);
    }

    public void EndSession()
    {
        ResultManager.ShowResults(CurrentStage);
        if (Camera.main.GetComponent<CameraController>())
        {
            Camera.main.GetComponent<CameraController>().isControllable = false;
        }
        DisableTooltips();
        isGameRunning = false;
    }

    public void DisableTooltips()
    {
        if (TooltipManager.isDisplayed)
        {
            TooltipManager.HideTooltip();
        }
    }

    public void ToggleSubmitConfirmation(bool state)
    {
        SConfirmUI.gameObject.SetActive(state);
        if (Camera.main.GetComponent<CameraController>())
        {
            Camera.main.GetComponent<CameraController>().isControllable = !state;
        }
        if (state)
        {
            isGameRunning = false;
        }
        else
        {
            isGameRunning = true;
        }

        DisableTooltips();
    }

    public void ResetGameDuration()
    {
        GameDuration = 0;
    }

    public void Update()
    {
        if (isGameRunning)
        {
            GameDuration += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            LoadPracticeState(GameStages[0], GameStages[0].Phases[3]);
        }
    }

    public void LoadPracticeState(GGameStage stage, GStagePhase phase)
    {
        StartCoroutine(ILoadPracticeState(stage, phase));
    }

    public IEnumerator ILoadPracticeState(GGameStage stage, GStagePhase phase)
    {
        Camera.main.GetComponent<CameraController>().isControllable = false;
        isLoading = true;
        ExitBtn.gameObject.SetActive(false);
        if (GameData.GameMode == GameMode.Training)
        {
            ObjectivesManager.ToggleObjectivesBar(false);
        }
        else
        {
            SubmitToggle.gameObject.SetActive(false);
        }
        LoadingScreen.gameObject.SetActive(true);
        if (LoadingCamera)
        {
            Camera.main.transform.position = LoadingCamera.position;
            Camera.main.transform.rotation = LoadingCamera.rotation;
        }
        yield return new WaitForSeconds(1);
        for(int i = 0; i < stage.Phases.Count; i++)
        {
            if(stage.Phases[i].PhaseIndex < phase.PhaseIndex)
            {
                for(int x = 0; x < stage.Phases[i].Modules.Count; x++)
                {
                    stage.Phases[i].Modules[x].OnAutofinish.Invoke();
                    while (!stage.Phases[i].Modules[x].IsFinished) yield return null;
                    yield return new WaitForSeconds(loadingSpeed);
                }
            }
        }
        yield return new WaitForSeconds(loadingIntervalSpeed);
        if (GameData.GameMode == GameMode.Training)
        {
            ObjectivesManager.ToggleObjectivesBar(true);
        }
        else
        {
            SubmitToggle.gameObject.SetActive(true);
        }
        LoadingScreen.gameObject.SetActive(false);
        ExitBtn.gameObject.SetActive(true);
        if (LoadingCamera)
        {
            Camera.main.GetComponent<CameraMovementController>().MoveToDefaultPos();
        }
        Camera.main.GetComponent<CameraController>().isControllable = true;
        isLoading = false;
        InteractionBlocker.gameObject.SetActive(false);
        yield break;
    }

    public void SkipLoading()
    {
        loadingSpeed = 0;
        loadingIntervalSpeed = 0;
    }



}


