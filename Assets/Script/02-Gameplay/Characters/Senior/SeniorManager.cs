﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeniorManager : MonoBehaviour, InputManager.Listener
{

	public List<SeniorBehaviour> _seniorsList = new List<SeniorBehaviour>();
	public SeniorBehaviour _seniorSelected;
	public ParticleSystem _cursor; 

	private static SeniorManager _instance;
	public static SeniorManager instance {get {return _instance;}}

	void Awake()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		InputManager.instance.registerListener(this);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void registerSenior(SeniorBehaviour senior)
	{
		if(_seniorsList.Contains(senior)) return;
		_seniorsList.Add(senior);
	}


	public void onPointerDown ( int _id ,Vector2 _position )
	{
	}

	public void onPointerUp ( int _id ,Vector2 _position )
	{
		// Left click
		if( _id == 0)
		{
			onPointerUpLeft(_position);
		}
	}

	protected void onPointerUpLeft(Vector2 _position)
	{
		Ray ray = Camera.main.ScreenPointToRay(_position);
		RaycastHit[] raysHits = Physics.RaycastAll(ray);
		RaycastHit hit = new RaycastHit();
		bool bHit;


		if(_seniorSelected._Type == SeniorBehaviour.SeniorType.Thief)
		{
			bHit = ClickOnTagObject(raysHits,"Door",ref hit);
			if(bHit) 
			{
				_seniorSelected.GoToDestination(hit.collider.gameObject.transform.position);
				return;
			}
		}
	


		bHit = ClickOnTagObject(raysHits,"Ground",ref hit);


		if(bHit) 
		{
			_seniorSelected.GoToDestination(hit.point);
			Vector3 pos = hit.point;
			pos.y += 0.01f;
			_cursor.transform.position = pos;
			_cursor.Emit(1);
		}
	}

	private bool ClickOnTagObject(RaycastHit[] raysHits, string p_tag, ref RaycastHit p_Hit )
	{
		for( int i = 0; i < raysHits.Length; i++)
		{
			if (raysHits[i].collider.tag == p_tag)
			{
				p_Hit = raysHits[i];
				return true;
			}	
		}
		return false;
	}

	public void onPointerDrag ( int _id ,Vector2 _position )
	{
	}
}
