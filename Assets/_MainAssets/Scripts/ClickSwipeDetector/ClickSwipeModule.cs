using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClickSwipeModule : MonoBehaviour
{
    public bool isComplete;
    public bool isActive;
    public Canvas Canvas;
    public int finishedAreasCount;
    public List<ClickSwipeArea> Areas = new List<ClickSwipeArea>();
    public UnityEvent OnFinish;
    


    public void Start()
    {
        if(Areas.Count > 0)
        {
            foreach (ClickSwipeArea csa in Areas)
            {
                csa.SetParentModule(this);
            }
        }
    }

    public void EnableSwipeModules()
    {
        Canvas.gameObject.SetActive(true);
        foreach(ClickSwipeArea csa in Areas)
        {
            csa.gameObject.SetActive(true);
        }
    } 
    
    public void DisableSwipeModules()
    {
        if (!isComplete)
        {
            foreach (ClickSwipeArea csa in Areas)
            {
                csa.Reset();
            }
        }
        Canvas.gameObject.SetActive(false);
        foreach (ClickSwipeArea csa in Areas)
        {
            csa.gameObject.SetActive(false);
        }
    }

    public bool CheckComplete()
    {
        if(finishedAreasCount < Areas.Count)
        {
            return false; 
        }
        else
        {
            OnFinish.Invoke();
            return true;
        }
    }

   
}
