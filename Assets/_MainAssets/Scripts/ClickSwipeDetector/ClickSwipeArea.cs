using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickSwipeArea : MonoBehaviour
{
    public float requiredSwipeTime = 5;
    public bool isSwipeFinished;
    public Image ProgressIndicator;
    public Color unfinishedColor;
    public Color FinishedColor;
    public Image SwipeCursor;
    private Image img;
    [SerializeField]
    private float progress;
    private float perc;
    private Color origCol;
    private ClickSwipeModule parentModule;

    public void SetParentModule(ClickSwipeModule csm)
    {
        parentModule = csm;
    }

    public void Start()
    {
        img = GetComponent<Image>();
        origCol = img.color;
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && !isSwipeFinished)
        {
            if(progress >= requiredSwipeTime)
            {
                isSwipeFinished = true;
                ProgressIndicator.color = FinishedColor;
                if (parentModule)
                {
                    parentModule.finishedAreasCount++;
                    parentModule.CheckComplete();
                }
            }
            progress += Time.deltaTime;
            perc = progress / requiredSwipeTime;
            float op = perc * 1;
            Color col = img.color;
            float curOp = origCol.a * perc;
            float newOp = origCol.a - curOp;
            col.a = newOp;
            img.color = col;
            if (ProgressIndicator)
            {
                ProgressIndicator.fillAmount = perc;
            }
        }
    }

    public void Reset()
    {
        progress = 0;
        ProgressIndicator.color = unfinishedColor;
        img.color = origCol;
        perc = 0;
        isSwipeFinished = false;
    }
}
