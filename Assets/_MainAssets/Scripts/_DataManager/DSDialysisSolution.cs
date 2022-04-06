using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSDialysisSolution : DataSetter
{
    public List<DatDialysisSolution> DialysisSolutions = new List<DatDialysisSolution>();
    /*
    public int DiaSolDistractorCount;
    //public List<VMDSolution> DialysisSolution = new List<VMDSolution>();
    public string MinimumExpYearRange;
    public string CorrectConcentration;
    public List<string> ConcentrationValues = new List<string>();
    public string CorrectVolume;
    public List<string> VolumeValues = new List<string>();
    public bool IntactGreenFrangibleSeal;
    public bool IntactBlueTwistCap;
    */

    [System.Serializable]
    public class FieldData
    {
        public string value;
        public bool isValid;
    }
    [System.Serializable]
    public class DatDialysisSolution
    {
        public VMDSolution DialysisSolution;
        public FieldData expiry;
        public FieldData volume;
        public FieldData concentration;
        public FieldData greenFrangibleSeal;
        public FieldData blueTwistCap;
        public FieldData serialNumber;
        public FieldData solutionType;
        public FieldData coloration;
        public FieldData leakage;
    }

    public void Start()
    {
        SetData();
    }

    public override bool SetData()
    {
        SetDialysisSolutionData();
        return true;
    }

    public void SetDialysisSolutionData()
    {
        foreach(DatDialysisSolution diaSol in DialysisSolutions)
        {
            if (int.TryParse(diaSol.expiry.value, out int n))
            {
                int mfgDate = int.Parse(diaSol.expiry.value) - 2;
                diaSol.DialysisSolution.Expiry.ValidationFieldText.text = "15. 09. " + mfgDate + "\n14. 09. " + diaSol.expiry.value;
            }
            else
            {
                diaSol.DialysisSolution.Expiry.ValidationFieldText.text = "15. 09. 2021\n14. 09. " + diaSol.expiry.value;
            }
            diaSol.DialysisSolution.Expiry.IsValid = diaSol.expiry.isValid;
            diaSol.DialysisSolution.Volume.ValidationFieldText.text = diaSol.volume.value;
            diaSol.DialysisSolution.Volume.IsValid = diaSol.volume.isValid;
            diaSol.DialysisSolution.Concentration.ValidationFieldText.text = diaSol.concentration.value;
            diaSol.DialysisSolution.Concentration.IsValid = diaSol.concentration.isValid;
            diaSol.DialysisSolution.BlueTwistCap.IsValid = diaSol.blueTwistCap.isValid;
            diaSol.DialysisSolution.GreenFrangibleSeal.IsValid = diaSol.greenFrangibleSeal.isValid;
            diaSol.DialysisSolution.bTwistCap.gameObject.SetActive(diaSol.blueTwistCap.isValid);
            SetFrangibleSeal(diaSol, diaSol.greenFrangibleSeal.isValid);
        }
    }

    /*
    public void SetFieldValue(VMField vField, string fVal, bool isValid)
    {
        vField.ValidationFieldText.text = fVal;
    }
    */
    public void SetTwistCap()
    {

    }
    
    public void SetFrangibleSeal(DatDialysisSolution diaSol, bool isIntact)
    {
        if (!isIntact)
        {
            diaSol.DialysisSolution.gFranSeal.localPosition = new Vector3(-1.59f, 0, -0.46f);
        }
        else
        {
            diaSol.DialysisSolution.gFranSeal.localPosition = new Vector3(0, 0, 0);
        }
    }
}
