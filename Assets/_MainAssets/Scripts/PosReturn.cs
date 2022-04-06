using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosReturn : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("LOCAL POS : " + transform.localPosition + " LOCAL ROT : " + transform.localRotation);
            Debug.Log("WORLD POS : " + transform.localPosition + " WORLD ROT : " + transform.localRotation);
        }
    }
}
