using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSetter : MonoBehaviour
{
    public virtual bool SetData()
    {

        return true;
    }

    public void ExecuteSetData()
    {
        SetData();
    }
}
