using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APDMachine : MonoBehaviour
{
    public Canvas machineScreenCanvas;

    public void TurnOn()
    {
        machineScreenCanvas.gameObject.SetActive(true);
    }
}
