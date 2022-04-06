using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CGTespy;
using TMPro;

public enum DisplayType
{
    ByPhase,
    ByStage
    
}

public class ObjectivesManager : MonoBehaviour
{
    public DisplayType DisplayType;
    public Transform objtvUI;
    public Transform objtvFieldPrefab;
    public Transform objtvFieldContainer;
    public TMP_Text PhaseLabel;
    public TMP_Text ObjectivesLabel;
    public List<Transform> Objectives = new List<Transform>();
    [SerializeField]
    private bool isCompactView;
    public Transform ToggleObjtvBtn;


    public void SetCompactView(bool state)
    {
        isCompactView = state;
    }

    public void AddObjtv(GPhaseModule mod)
    {
        Transform newObjF = Instantiate(objtvFieldPrefab, objtvFieldContainer);
        newObjF.name = mod.name + "_objtv";
        ObjectiveField oF = newObjF.GetComponent<ObjectiveField>();
        oF.Module = mod;
        string newName = mod.ModuleName;
        string replacedName = newName.Replace("<br>", "\n");
        oF.OnSetState += UpdateCompactView;
        oF.Label.text = replacedName;
        Objectives.Add(newObjF);
        mod.DoSetState += oF.SetState;
    }

    public void AddPhaseObjtvList(GStagePhase phase)
    {
        StartCoroutine(IAddPhaseObjtvList(phase));
    }

    public void SetPhaseLabel(string phaseName)
    {
        if (!PhaseLabel) return;
        PhaseLabel.text = phaseName;
    }

    public IEnumerator IAddPhaseObjtvList(GStagePhase phase)
    {
        foreach (GPhaseModule mod in phase.Modules)
        {
            AddObjtv(mod);
        }

        if (isCompactView)
        {
            UpdateCompactView();
        }
        else
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(objtvUI.GetComponent<RectTransform>());

            UIReanchorTool.SetTopLeftAnchor(objtvUI.GetComponent<RectTransform>());
        }
        yield break;
    }

    public void ToggleCompactView() 
    {
        if (isCompactView)
        {
            foreach (Transform objtv in Objectives)
            {
                objtv.gameObject.SetActive(true);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(objtvUI.GetComponent<RectTransform>());

            UIReanchorTool.SetTopLeftAnchor(objtvUI.GetComponent<RectTransform>());

            isCompactView = false;
            ObjectivesLabel.text = "Objectives : ";
            ToggleObjtvBtn.transform.rotation = new Quaternion(0,0,-0.7f, 0.7f);
        }
        else
        {
            isCompactView = true;
            UpdateCompactView();
            ObjectivesLabel.text = "Current Objective : ";
            ToggleObjtvBtn.transform.rotation = new Quaternion(0, 0, 0.7f, 0.7f);
        }
    }

    public void UpdateCompactView()
    {
        if (!isCompactView) return;
        foreach (Transform objtv in Objectives)
        {
            objtv.gameObject.SetActive(false);
        }

        foreach (Transform objtv in Objectives)
        {
            if (objtv.GetComponent<ObjectiveField>())
            {
                if (!objtv.GetComponent<ObjectiveField>().Module.IsFinished)
                {
                    objtv.gameObject.SetActive(true);
                    break;
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(objtvUI.GetComponent<RectTransform>());

        UIReanchorTool.SetTopLeftAnchor(objtvUI.GetComponent<RectTransform>());
    }


  

    public void UpdateObjtvList(GGameStage stage)
    {
        ClearObjtvList();

        foreach (GStagePhase phase in stage.Phases)
        {
            AddPhaseObjtvList(phase);
        }
    }

    public void ClearObjtvList()
    { 
        foreach (Transform objtv in Objectives)
        {
            Destroy(objtv.gameObject);
        }
        Objectives.Clear();
    }

    public void ToggleObjectivesBar(bool isOn)
    {
        if (isOn)
        {
            objtvUI.gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(objtvUI.GetComponent<RectTransform>());
            UIReanchorTool.SetTopLeftAnchor(objtvUI.GetComponent<RectTransform>());
        }
        else
        {
            objtvUI.gameObject.SetActive(false);
        }
    }
}
