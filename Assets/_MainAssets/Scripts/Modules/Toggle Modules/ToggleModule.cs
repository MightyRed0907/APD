using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleModule : Module
{
    public bool toggleStatus;
    public AudioSource aSource;
    public AudioClip OnAClip;
    public AudioClip OffAClip;

    public virtual bool Toggle()
    {
        if (!isModuleActive) return false;

        
        return true;
    }

    public void PlaySfx()
    {
        if (aSource)
        {
            aSource.Stop();
            if (toggleStatus == false)
            {
                if (OnAClip)
                {
                    aSource.clip = OnAClip;
                }
            }
            else
            {
                if (OffAClip)
                {
                    aSource.clip = OffAClip;
                }
            }
            aSource.Play();
        }
    }
}
