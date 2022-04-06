using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectLerper))]
[RequireComponent(typeof(ObjectRotator))]
public class OPItem : MonoBehaviour
{
    public bool isHoveringOPArea;
    public OPArea currentAreaPlaced;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(gameObject.name + " : POS : " + gameObject.transform.localPosition + " | ROT : " + gameObject.transform.rotation);
        }
    }

    
}
