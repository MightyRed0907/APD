using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMObjPlacement : GPhaseModule
{
    public string ResultItemLabel;
    public bool disableOnPlacement;
    public List<OPArea> OPAreas = new List<OPArea>();

    public bool IsAllItemsPlaced(OPArea area)
    {
        bool placedAll = true;

        if (area.AreaType == OPAreaType.specified) 
        {
            foreach (OPConfig conf in area.Items)
            {
                if (!conf.isPlaced)
                {
                    placedAll = false;
                }
                else
                {
                    if (disableOnPlacement && phaseParent.stageParent.IsSequential)
                    {
                        if (conf.Item.GetComponent<Interactable>())
                        {
                            if (conf.Item.GetComponent<Interactable>().isInteractable)
                            {
                                conf.Item.GetComponent<Interactable>().isInteractable = false;
                            }
                        }
                    }
                    
                }
            }
        }
        else
        {
            foreach (OPSlot slot in area.SlotsPos)
            {
                if (!slot.isTaken)
                {
                    placedAll = false;
                }
                else
                {
                    if (disableOnPlacement && phaseParent.stageParent.IsSequential)
                    {
                        if (slot.Item.GetComponent<Interactable>())
                        {
                            if (slot.Item.GetComponent<Interactable>().isInteractable)
                            {
                                slot.Item.GetComponent<Interactable>().isInteractable = false;
                            }
                        }
                    }
                }
            }
        }

        return placedAll;
    }

    public bool IsAllOPAreaItemsPlaced()
    {
        bool isComplete = true;
        foreach(OPArea oa in OPAreas)
        {
            if (!IsAllItemsPlaced(oa))
            {
                Debug.Log("OP Area " + oa.gameObject.name + " status : FALSE");
                isComplete = false;
            }
        }
        return isComplete;
    }

    public void UpdateModuleState()
    {
        if (IsFinished) return;
        IsFinished = IsAllOPAreaItemsPlaced();
        UpdatePhaseParent();
        if (phaseParent.IsSequential && phaseParent.stageParent.IsSequential)
        {

            if (IsFinished)
            {
                phaseParent.UnlockNextModule(this);
            }
            if (phaseParent.stageParent.IsSequential)
            {
                DoSetState(IsFinished);
            }
        }
        
    }

    public override int GetScorePoints()
    {
        int score = 0;

        foreach (OPArea area in OPAreas)
        {

            if (area.AreaType == OPAreaType.specified)
            {
                foreach (OPConfig conf in area.Items)
                {
                    if (conf.wasPlaced)
                    {
                        score += 1;
                    }
                }
            }
            else
            {
                foreach (OPSlot slot in area.SlotsPos)
                {
                    if (slot.wasTaken)
                    {
                        score += 1;
                    }
                }
            }
        }

        return score;
    }

    public override int GetPossiblePoints()
    {
        int points = 0;

        foreach (OPArea area in OPAreas)
        {

            if (area.AreaType == OPAreaType.specified)
            {
                foreach (OPConfig conf in area.Items)
                {
                    points += 1;
                }
            }
            else
            {
                foreach (OPSlot slot in area.SlotsPos)
                {
                    points += 1;
                }
            }
        }

        return points;
    }
}
