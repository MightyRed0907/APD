using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMMatColor : ToggleModule
{
    public bool turnOnStart;
    public Color onColor;
    public Color offColor;
    
    private MeshRenderer mRenderer;
    private Color originalColor;

    public void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();

        if (!mRenderer)
        {
            Debug.Log("No meshrenderer attached to " + transform.gameObject.name);
        }
        else
        {
            originalColor = mRenderer.material.color;
            if (turnOnStart)
            {
                toggleStatus = true;
                mRenderer.material.color = onColor;
            }
        }
    }

    public override bool Toggle()
    {
        if (!base.Toggle()) return false;

        
        ToggleMatColor();

        return true;
    }

    public void ToggleMatColor()
    {
        PlaySfx();
        if (!mRenderer) return;

        if (!toggleStatus)
        {
            mRenderer.material.color = onColor;
            toggleStatus = true;
        }
        else
        {
            mRenderer.material.color = offColor;
            toggleStatus = false;
        }
    }
}
