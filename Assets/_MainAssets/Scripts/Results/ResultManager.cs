using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public GameManager GameManager;
    public ScoringManager ScoreManager;
    public GGameStage stage;
    public Transform ResultsPanel;
    public Transform PR_Prefab;
    public Transform PRField_Prefab;
    public Transform PRFieldContainer;
    public TMP_Text StageLabel;
    public TMP_Text DurationField;
    public TMP_Text OverallAccField;
    public Button NextRPageBtn;
    public Button PrevRPageBtn;
    public List<Transform> ResultPages = new List<Transform>();
    public List<Transform> PRFields = new List<Transform>();
    private int currentResultPage = 0;

    [Space(10)]
    [Header("Results UI")]
    public TMP_Text ODuration_Fld;
    public TMP_Text OAccuracy_Fld;
    public TMP_Text OSeqAccuracy_Fld;
    public TMP_Text OStepsAccuracy_Fld;

    public void ShowResults(GGameStage stage)
    {
        if (GameManager.isLoading) return;
        int totalPointsGained = 0;
        int totalPossiblePoints = 0;
        int totalPhaseSequencePoints = 0;
        int totalSequencePointsGained = 0;
        int totalDistractorsDeduction = 0;
        List<PhaseResult> PhaseResults = new List<PhaseResult>();

        if (PRFields.Count > 0)
        {
            foreach(Transform t in PRFields)
            {
                Destroy(t.gameObject);
            }
            PRFields.Clear();
        }

        foreach(GStagePhase phase in stage.Phases)
        {
            int totalPhasePoints = 0;
            int phasePointsGained = 0;
            int phaseDistractorsDeduction = 0;

            totalSequencePointsGained += phase.GetSequencePoint();
            totalPhaseSequencePoints += phase.GetPossibleSequencePoints();

            Transform newP = Instantiate(PR_Prefab, PRFieldContainer);
            PRFields.Add(newP);
            newP.gameObject.name = "PR_" + phase.PhaseName;
            PhaseResult pR = newP.GetComponent<PhaseResult>();
            if (pR)
            {
                pR.GSPhase = phase;
                pR.PhaseLabel.text = phase.PhaseName;
                PhaseResults.Add(pR);

                foreach (GPhaseModule gpm in phase.Modules)
                {
                    totalPhasePoints += gpm.GetPossiblePoints();
                    totalPossiblePoints += gpm.GetPossiblePoints();
                    totalPointsGained += gpm.GetScorePoints();
                    phasePointsGained += gpm.GetScorePoints();
                    phaseDistractorsDeduction += gpm.DistractorsActivated.Count;

                    totalPhaseSequencePoints += gpm.GetPossibleSequencePoints();
                    totalSequencePointsGained += gpm.GetSequencePoint();

                    Debug.Log("Current Total Sequence Point " + gpm.ModuleName + " :" + totalSequencePointsGained);
                    Debug.Log("Overall Total Sequence Point " + gpm.ModuleName + " :" + totalPhaseSequencePoints);
                    

                    if (!gpm.GetComponent<PMObjPlacement>())
                    {
                        Transform newPRField = Instantiate(PRField_Prefab, pR.PRContainer);
                        newPRField.gameObject.name = "PRField_" + gpm.ModuleIndex;
                        ResultField rF = newPRField.GetComponent<ResultField>();
                        if (rF)
                        {
                            rF.Module = gpm;
                            rF.ForceSetState(gpm.IsFinished);
                            rF.Label.text = gpm.ModuleName;
                        }

                        if (gpm.GetComponent<PMValidation>())
                        {
                            PMValidation vMod = gpm.GetComponent<PMValidation>();

                            foreach (ValidationModule valMod in vMod.ValidationModules)
                            {
                                foreach (VMField vF in valMod.Fields)
                                {
                                    if (!vF.GetComponent<Distractor>())
                                    {
                                        if (vF.ValidationStatus != ValidationStatus.unvalidated)
                                        {
                                            Transform valPRField = Instantiate(PRField_Prefab, pR.PRContainer);
                                            valPRField.gameObject.name = "PRField_Val_" + gpm.ModuleIndex;
                                            ResultField valRf = valPRField.GetComponent<ResultField>();
                                            if (valRf)
                                            {
                                                valRf.Module = gpm;
                                                if (vF.IsValid != vF.GetValidation())
                                                {
                                                    valRf.ForceSetState(false, true);
                                                }
                                                else
                                                {
                                                    valRf.ForceSetState(true);
                                                }
                                                valRf.Label.text = "Validate Field : " + vF.FieldName;
                                            }
                                        }
                                        else if (vF.ValidationStatus == ValidationStatus.unvalidated)
                                        {
                                            Transform valPRField = Instantiate(PRField_Prefab, pR.PRContainer);
                                            valPRField.gameObject.name = "PRField_Val_" + gpm.ModuleIndex;
                                            ResultField valRf = valPRField.GetComponent<ResultField>();
                                            if (valRf)
                                            {
                                                valRf.Module = gpm;
                                                valRf.ForceSetState(false);
                                                valRf.Label.text = "Validate Field : " + vF.FieldName;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (gpm.GetComponent<PMObjPlacement>())
                    {
                        Debug.Log("ObjPlacement Module : " + gpm.ModuleName);
                        PMObjPlacement oPgpm = gpm.GetComponent<PMObjPlacement>();

                        foreach(OPArea area in oPgpm.OPAreas)
                        {
                            if (area.AreaType == OPAreaType.specified)
                            {
                                foreach (OPConfig item in area.Items)
                                {
                                    Transform newItemPRField = Instantiate(PRField_Prefab, pR.PRContainer);
                                    newItemPRField.gameObject.name = "PRField_" + item.Item.name;
                                    ResultField rIF = newItemPRField.GetComponent<ResultField>();

                                    if (rIF)
                                    {
                                        rIF.Module = gpm;
                                        if (gpm.IsFinished)
                                        {
                                            rIF.ForceSetState(true);
                                        }
                                        else
                                        {
                                            rIF.ForceSetState(item.wasPlaced);
                                        }

                                        if (!string.IsNullOrEmpty(oPgpm.ResultItemLabel))
                                        {
                                            string label = oPgpm.ResultItemLabel.Replace("<item>", item.Item.name);
                                            rIF.Label.text = label;
                                        }
                                        else
                                        {
                                            rIF.Label.text = item.Item.name;
                                        }
                                    }
                                }
                            }
                            else if (area.AreaType == OPAreaType.slot)
                            {
                                foreach (OPSlot slot in area.SlotsPos)
                                {
                                    Transform newItemPRField = Instantiate(PRField_Prefab, pR.PRContainer);
                                    newItemPRField.gameObject.name = "PRField_" + area.SlotItemName;
                                    ResultField rIF = newItemPRField.GetComponent<ResultField>();

                                    if (rIF)
                                    {
                                        rIF.Module = gpm;
                                        if (gpm.IsFinished)
                                        {
                                            rIF.ForceSetState(true);
                                        }
                                        else
                                        {
                                            rIF.ForceSetState(slot.wasTaken);
                                        }

                                        if (!string.IsNullOrEmpty(oPgpm.ResultItemLabel))
                                        {
                                            string label = oPgpm.ResultItemLabel.Replace("<item>", area.SlotItemName);
                                            rIF.Label.text = label;
                                        }
                                        else
                                        {
                                            rIF.Label.text = area.SlotItemName;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (Distractor d in gpm.DistractorsActivated)
                    {
                        Transform newPRField = Instantiate(PRField_Prefab, pR.PRContainer);
                        newPRField.gameObject.name = "PRField_" + gpm.ModuleIndex;
                        ResultField rF = newPRField.GetComponent<ResultField>();
                        if (rF)
                        {
                            rF.Module = gpm;
                            rF.ForceSetState(gpm.IsFinished, true);
                            rF.Label.text = "Unnecessary Action : " + d.Label;
                        }
                        totalDistractorsDeduction++;
                        Debug.Log("Total Distraction Deduction : " + totalDistractorsDeduction);
                    }

                }

                Debug.Log("Total Phase Points : " + totalPhasePoints + " Total Gained Points : " + phasePointsGained);

                int tPP = phasePointsGained - phaseDistractorsDeduction;
                float oAcc = (float)tPP / (float)totalPhasePoints;
                float finalAcc = oAcc * 100;

                int PercAcc = Mathf.RoundToInt(finalAcc);
                pR.AccuracyField.text = PercAcc.ToString() + "%";
                if (phase.phaseTime > 0)
                {
                    pR.DurationField.text = GetTimeString(phase.phaseTime);
                }
                else
                {
                    pR.DurationField.text = "N/A";
                }
            }
            float totalDuration = GameManager.GameDuration;
            float tPoints = totalPointsGained + totalSequencePointsGained;
            Debug.Log("Total Points Gained : " + tPoints);
            float tPGain = tPoints - totalDistractorsDeduction;
            Debug.Log("Total Points Gained with Deductions : " + tPGain);
            float tPossiblePScore = totalPhaseSequencePoints + totalPossiblePoints;
            float overallAcc = (float)tPGain / (float)tPossiblePScore;
            //float overallAcc = (float)totalPointsGained / (float)totalStagePoints;
            float fAcc = overallAcc * 100;

            int PAcc = Mathf.RoundToInt(fAcc);
            OAccuracy_Fld.text = PAcc.ToString() + "%";


            
            ODuration_Fld.text = GetTimeString(totalDuration);
            OSeqAccuracy_Fld.text = GetAccuracy(totalPhaseSequencePoints, totalSequencePointsGained).ToString() + "%";
            OStepsAccuracy_Fld.text = GetAccuracy(totalPossiblePoints, totalPointsGained).ToString() + "%";
            ResultsPanel.gameObject.SetActive(true);
            
            if(GameManager.GameMode == GameMode.Practice)
            {
                foreach(PhaseResult pr in PhaseResults)
                {
                    if(pr.GSPhase.PhaseIndex != GameManager.selectedPhaseIndex)
                    {
                        pr.gameObject.SetActive(false);
                    }
                }

                ResultPages[0].gameObject.SetActive(false);
                ResultPages[1].gameObject.SetActive(true);
                NextRPageBtn.gameObject.SetActive(false);
                PrevRPageBtn.gameObject.SetActive(false);
                //PRFieldContainer.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
            }
        }
    }

    public int GetAccuracy(int totalPoints, int pointsGained)
    {
        int acc = 0;

        float perc = (float)pointsGained / (float)totalPoints;
        float percAcc = perc * 100;
        acc = Mathf.RoundToInt(percAcc);

        return acc;
    }

    public string GetTimeString(float duration)
    {
        string t = "";

        if(duration > 60)
        {
            if(duration < 3600)
            {
                float sec = duration % 60;
                int seconds = Mathf.RoundToInt(sec);
                float min = duration / 60;
                int minutes = Mathf.FloorToInt(min);

                t = minutes + "m " + seconds + "s";
            }
        }
        else
        {
            int s = Mathf.FloorToInt(duration);
            t = s + "s";
        }

        return t;
    }

    public void GoToNextPage()
    {
        if(currentResultPage < ResultPages.Count)
        {
            ResultPages[currentResultPage].gameObject.SetActive(false);
            ResultPages[currentResultPage + 1].gameObject.SetActive(true);
            currentResultPage++;
            PrevRPageBtn.interactable = true;

            if (currentResultPage >= ResultPages.Count - 1)
            { 
                NextRPageBtn.interactable = false;
            }
        }
    }
    
    public void GoToPrevPage()
    {
        if(currentResultPage > 0)
        {
            ResultPages[currentResultPage].gameObject.SetActive(false);
            ResultPages[currentResultPage - 1].gameObject.SetActive(true);
            currentResultPage--;
            NextRPageBtn.interactable = true;

            if (currentResultPage <= 0)
            {
                PrevRPageBtn.interactable = false;
            }
        }
    }



    public void Update()
    {
        
    }
}
