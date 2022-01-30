/*
 * Written by:
 * Henrik
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO rotate held object

abstract public class Interactable : MonoBehaviour
{
	[SerializeField] KeyCode _interactionButton;
	
	public KeyCode interactionButton => _interactionButton;
	public bool canBeHeld;
	public bool lookTriggerEnabled;
	[HideInInspector] public bool lookIsActive;
	bool lookShouldStay;
	public object lookData;
	[HideInInspector] public bool held;
	private Rigidbody rb;
	private Transform originalParent;
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		originalParent = transform.parent;
	}
	
	void Update()
	{
		if(lookShouldStay)
			lookIsActive = true;
		else
		{
			lookIsActive = false;
		}
		LookInteraction();
		lookShouldStay = false;
	}

	public bool SetHeld(bool state, Transform parent)
	{
		if(canBeHeld)
		{
			if(rb)
			{
				held = state;
				rb.isKinematic = state;
				if(!held)
					transform.SetParent(originalParent, true);
				else
					transform.SetParent(parent, true);
				rb.velocity = Vector3.zero;
				return held;
			}
			else
				Debug.LogWarning("This object is missing a rigidbody to be picked up " + this);
		}
		return false;
	}
	
	public void lookTrigger(object target = null)
	{
		lookData = target;
		lookShouldStay = true;
	}
	
	virtual public void LookInteraction(){}
	abstract public void InteractTrigger(object target = null);
	abstract public void InteractionEndTrigger(object target = null);
}