using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PMRecordBP : PMRecording
{
    public ITBPMonitor bpMonitor;

    public void CheckRecordField()
    {
        if (!bpMonitor) return;
        if (bpMonitor.GetLastBPResult() == Vector3.zero) return;
        string p1format = bpMonitor.GetLastBPResult().x.ToString() + "/" + bpMonitor.GetLastBPResult().y.ToString() + " (" + bpMonitor.GetLastBPResult().z + ")";
        string p2format = bpMonitor.GetLastBPResult().x.ToString() + " / " + bpMonitor.GetLastBPResult().y.ToString() + " ( " + bpMonitor.GetLastBPResult().z + " )";
        string p3format = bpMonitor.GetLastBPResult().x.ToString() + "/" + bpMonitor.GetLastBPResult().y.ToString() + "(" + bpMonitor.GetLastBPResult().z +")";
        if (recordField.text == p1format ||
            recordField.text == p2format ||
            recordField.text == p3format)
        {
            IsFinished = true;
            BSetModuleStatus();
            if (phaseParent.IsSequential && phaseParent.stageParent.IsSequential)
            {
                phaseParent.UnlockNextModule(this);
                DoSetState(IsFinished);
            }
           
        }
    }

    public void AutoRecordInField()
    {
        StartCoroutine(IAutoRecordInField());
    }

    public IEnumerator IAutoRecordInField()
    {
        if (!bpMonitor) yield break;

        while (bpMonitor.GetLastBPResult() == Vector3.zero) yield return null;

        recordField.text = bpMonitor.GetLastBPResult().x.ToString() + "/" + bpMonitor.GetLastBPResult().y.ToString() + " (" + bpMonitor.GetLastBPResult().z + ")";
        IsFinished = true;

        BSetModuleStatus();
        if (phaseParent.IsSequential && phaseParent.stageParent.IsSequential)
        {
            phaseParent.UnlockNextModule(this);
            DoSetState(IsFinished);
        }
    }
}
