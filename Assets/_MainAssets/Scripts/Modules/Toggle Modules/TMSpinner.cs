using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMSpinner : ToggleModule
{
    [SerializeField]
    private float spinSpeed;
    [SerializeField]
    private bool spinOnStart;
    private Transform obj;
    private bool isSpinning;

    public void Start()
    {
        obj = GetComponent<Transform>();

        if (spinOnStart)
        {
            isSpinning = true;
            toggleStatus = true;
        }
    }

    private void Update()
    {
        if (isModuleActive)
        {
            if (isSpinning)
            {
                obj.transform.Rotate(Vector3.up * Time.deltaTime * spinSpeed);
            }
        }
    }

    public override bool Toggle()
    {
        if (!base.Toggle()) return false;

        ToggleSpin();

        return true;
    }

    public void ToggleSpin()
    {
        if (!toggleStatus)
        {
            isSpinning = true;
            toggleStatus = true;
        }
        else
        {
            isSpinning = false;
            toggleStatus = false;
        }
    }
}
