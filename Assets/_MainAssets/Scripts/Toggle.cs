using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Toggle : MonoBehaviour
{
    public List<Transform> Targets = new List<Transform>();

    public void ToggleTargets()
    {
        foreach(Transform t in Targets.ToList())
        {
            if (!t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }
    
    public void EnableTargets()
    {
        foreach(Transform t in Targets.ToList())
        {
            t.gameObject.SetActive(true);
        }
    }
    
    public void DisableTargets()
    {
        foreach(Transform t in Targets.ToList())
        {
            t.gameObject.SetActive(false);
        }
    }
}
