using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIReanchorTool
{
    public static void SetTopLeftAnchor(RectTransform rectTarget, bool forceRebuildUI = false)
    {
        rectTarget.anchorMin = new Vector2(0, 1);
        rectTarget.anchorMax = new Vector2(0, 1);
        rectTarget.pivot = new Vector2(0.5f, 0.5f);
        float uiHeight = rectTarget.GetComponent<RectTransform>().rect.height;
        float posY = uiHeight / 2;
        float uiWidth = rectTarget.GetComponent<RectTransform>().rect.width;
        float posX = uiWidth / 2;
        rectTarget.GetComponent<RectTransform>().anchoredPosition = new Vector3(posX, posY * -1, rectTarget.position.z);
    }
}
