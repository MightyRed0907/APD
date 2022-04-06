using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Openable : MonoBehaviour
{
    public List<GameObject> Cover = new List<GameObject>();
    public UnityEvent OnOpen;
    public bool isOpened;

    public void OpenObject()
    {
        if (isOpened) return;
        foreach(GameObject c in Cover)
        {
            c.SetActive(false);
        }
        OnOpen.Invoke();
        isOpened = true;
    }
}
