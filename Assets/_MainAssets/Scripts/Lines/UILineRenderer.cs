using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILineRenderer : MonoBehaviour
{
    public List<Transform> Points = new List<Transform>();
    public Transform ObjElemet;
    public Transform UiElement;
    public LineRenderer lr;

    private Vector3 uiToWorldPos;

    public void SetLine(Transform ObjElemet,Transform UiElement)
    {
        
    }

    public void Update()
    {
        if (UiElement)
        {
            uiToWorldPos = Camera.main.ScreenToWorldPoint(UiElement.GetComponent<RectTransform>().transform.position);
        }
    }
}
