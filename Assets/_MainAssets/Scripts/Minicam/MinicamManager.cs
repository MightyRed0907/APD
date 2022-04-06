using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinicamManager : MonoBehaviour
{
    public RawImage minicamRawImg;
    public RenderTexture rendText;
    public Minicam TargetCamera;

    public void Start()
    {
    }

    public void ShowMinicam(Minicam mCam)
    {
        minicamRawImg.gameObject.SetActive(true);
        minicamRawImg.texture = rendText;
        mCam.gameObject.SetActive(true);
        mCam.GetComponent<Camera>().targetTexture = rendText;
    }
    
    public void HideMinicam(Minicam mCam)
    {
        minicamRawImg.texture = null;
        minicamRawImg.gameObject.SetActive(false);
        mCam.GetComponent<Camera>().targetTexture = null;
        mCam.gameObject.SetActive(false);
    }

}
