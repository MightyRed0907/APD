using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isControllable;
    public float rotationSpeed;
    public bool isXMoveRestricted;
    private Camera cam;
    private Vector2 mousePos;
    private Vector2 screenSize;
    private Transform t;

    [SerializeField]
    private Vector2 xMoveRange;

    private void Awake()
    {
        t = GetComponent<Transform>();
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void Update()
    {
        mousePos = Input.mousePosition;
        screenSize.x = Screen.width;
        screenSize.y = Screen.height;

        if (isControllable)
        {

            if (mousePos.x < 5)
            {
                Debug.Log("Current Camera Rotation : " + t.eulerAngles + " X Move Range : " + xMoveRange);
                if (CanLookLeft(t.eulerAngles.y))
                {
                    t.RotateAround(t.position, t.up, -rotationSpeed * Time.deltaTime);
                    Debug.Log("Current Camera Rotation : " + t.eulerAngles);
                }
            }
            else if (mousePos.x > screenSize.x - 5)
            {
                if (CanLookRight(t.eulerAngles.y))
                {
                    t.RotateAround(t.position, t.up, rotationSpeed * Time.deltaTime);
                    Debug.Log("Current Camera Rotation : " + t.eulerAngles);
                }
            }

            if (mousePos.y < 5 && CanLookDown(t.eulerAngles.x))
            {
                float newRotSpeed = rotationSpeed / 1.5f;
                t.RotateAround(t.position, t.right, newRotSpeed * Time.deltaTime);
                //Debug.Log("Current Camera Rotation : " + t.eulerAngles);
            }
            else if (mousePos.y > screenSize.y - 5 && CanLookUp(t.eulerAngles.x))
            {
                float newRotSpeed = rotationSpeed / -1.5f;
                t.RotateAround(t.position, t.right, newRotSpeed * Time.deltaTime);
                //Debug.Log("Current Camera Rotation : " + t.eulerAngles);
            }
        }
    }

    public void SetXRestriction(bool isRestricted, Transform referenceTransform)
    {
        Vector2 newRange = new Vector2(0, 0);
        if (isRestricted)
        {
            isXMoveRestricted = true;
            xMoveRange.x = referenceTransform.eulerAngles.y - 10;
            float xsign = Mathf.Sign(xMoveRange.x);
            if (xsign != 1)
            {
                xMoveRange.x = 360 + xMoveRange.x;
            }
            xMoveRange.y = referenceTransform.eulerAngles.y + 9;
            if (xMoveRange.y > 360)
            {
                xMoveRange.y = xMoveRange.y - 360;
            }
        }
        else
        {
            isXMoveRestricted = false;
        }
    }

    public bool CanLookDown(float angle)
    {
        bool can = false;
        if(angle <= 55 || angle >= 339)
        {
            can = true;
        }
        return can;
    }
    
    public bool CanLookUp(float angle)
    {
        bool can = false;
        if (angle >= 340 || angle <= 56)
        {
            can = true;
        }
        return can;
    }

    public bool CanLookLeft(float angle)
    {
        if (!isXMoveRestricted) return true;
        bool can = false;

        if (angle >= xMoveRange.x || angle == 0 || angle < 10)
        {
            can = true;
        }
        return can;
    }

    public bool CanLookRight(float angle)
    {
        if (!isXMoveRestricted) return true;
        bool can = false;
        if (angle <= xMoveRange.y|| angle == 0 || angle > 349)
        {
            can = true;
        }
        return can;
    }

    public void ToggleControl(bool state)
    {
        isControllable = state;
    }
}
