using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    public Transform startObj;
    public Transform endUIElement;
    private LineRenderer lr;

    //public Transform obj;

    public void SetLine(Transform obj, Transform ui)
    {
        lr.SetPosition(0, startObj.transform.position);
        //lr.SetPosition(0, obj.transform.position);
        Vector3 uiLinePos = Camera.main.ScreenToWorldPoint(endUIElement.transform.position);
        //Vector3 newUiLinePos = new Vector3(uiLinePos.x, uiLinePos.y, 1);
        lr.SetPosition(1, Camera.main.ScreenToWorldPoint(uiLinePos));
        //lr.SetPosition(1, Camera.main.ScreenToWorldPoint(ui.transform.position));
    }

    public void Update()
    {
        SetLine(startObj, endUIElement);
    }

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

}
