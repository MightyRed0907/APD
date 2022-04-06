using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PBubbleItem : MonoBehaviour
{
	private float x,y;
	public float Speed= 1f;
	public TMP_Text label;
	private bool isSelected;
	public bool isStatic;

	[HideInInspector]
	public Vector2 pauseVelocity;
	public Vector3 origPos;
	[HideInInspector]
	public Transform origParent;
	
	//can be vary manually
	// Use this for initialization
	private void Start ()
	{
		/*
		x = Random.Range (-1, 2);
		y = Random.Range (-1, 2);
		*/
		origParent = transform.parent;
		origPos = transform.position;
	}    
	
	// Continously assigning x and y.
	private void FixedUpdate ()
	{
		if (!isStatic)
		{
			this.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, y) * Speed, ForceMode2D.Force);
		}
	}

	public void ShowLabel()
    {
        if (label)
        {
			label.gameObject.SetActive(true);
        }
    }
	
	public void HideLabel()
    {
		if (label)
		{
			label.gameObject.SetActive(false);
		}
	}
	
	//Changing x and y on collision
	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (!isStatic)
		{
			if (x < 0)
			{
				x = 1;
			}
			else if (x > 0)
			{
				x = -1;
			}
			else
			{
				x = Random.Range(-1, 2);
			}

			if (y < 0)
			{
				y = 1;
			}
			else if (y > 0)
			{
				y = -1;
			}
			else
			{
				y = Random.Range(-1, 2);
			}
		}
	}

    public void SetToMousePos()
    {
		isStatic = true;
		pauseVelocity = transform.GetComponent<Rigidbody2D>().velocity;
		if (!isStatic)
		{
			transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
			transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
		transform.position = Input.mousePosition;
    }

	public void PauseMovement()
    {
		pauseVelocity = transform.GetComponent<Rigidbody2D>().velocity;
		isStatic = true;
    }

	public void StartRandomMove()
    {
		x = Random.Range(-1, 2);
		y = Random.Range(-1, 2);
	}
	
	public void ResumeMovement()
    {
		if (isStatic) return;
		isStatic = false;
		if (transform.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
		{
			x = Random.Range(-1, 2);
			y = Random.Range(-1, 2);
		}
		else
		{
			transform.GetComponent<Rigidbody2D>().velocity = pauseVelocity;
		}
	}

	public void EnableRandomMove()
    {
		isStatic = false;
    }

}
