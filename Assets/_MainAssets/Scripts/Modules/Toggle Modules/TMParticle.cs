using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMParticle : ToggleModule
{
    private ParticleSystem partSys;

    public void Start()
    {
        partSys = GetComponent<ParticleSystem>();

        if (partSys)
        {
            if (partSys.isPlaying)
            {
                toggleStatus = true;
            }
            else
            {
                toggleStatus = false;
            }
        }
        else
        {
            Debug.Log("No particle system attached to " + gameObject.name);
        }
    }

    public override bool Toggle()
    {
        if (!base.Toggle()) return false;

        ToggleParticles();

        return true;
    }

    public void ToggleParticles()
    {
        if (toggleStatus)
        {
            partSys.Stop();
            toggleStatus = false;
        }
        else
        {
            partSys.Play();
            toggleStatus = true;
        }
    }
}
