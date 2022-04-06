using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VMDSolution : ValidationModule
{
    public VMField Concentration;
    public VMField Expiry;
    public VMField Volume;
    public VMField GreenFrangibleSeal;
    public VMField BlueTwistCap;
    public VMField SerialNumber;
    public VMField SolutionType;
    public VMField Coloration;
    public VMField Leakage;

    public Transform gFranSeal;
    public Transform bTwistCap;
}
