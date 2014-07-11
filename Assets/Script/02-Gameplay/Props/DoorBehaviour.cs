using UnityEngine;
using System.Collections;

public class DoorBehaviour : MonoBehaviour {

	public OffMeshLink link; 

	public bool _Lock = false;

	public UIPanel _IconPanel;
	public UISprite _IconSprite;


	private bool _UnlockInProgress = false;
	private float _UnlockTimer = 0;

	// Use this for initialization
	void Start () 
	{
		if(_Lock)
		{
			LockDoor();
		}
		else
		{
			UnlockDoor();
			_IconPanel.alpha = 0;
		}


	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void UnlockDoor() 
	{
		link.activated = true;
		_Lock = false;
	}
	public void LockDoor() 
	{
		link.activated = false;
		_Lock = true;
	}


	void OnTriggerEnter(Collider other)
	{
		SeniorBehaviour senior = other.gameObject.GetComponent<SeniorBehaviour>();
		if( senior != null && senior._Type == SeniorBehaviour.SeniorType.Thief && _Lock)
		{
			_UnlockInProgress = true;
			_IconPanel.alpha = 1;
			_IconSprite.fillAmount = 0;
			_UnlockTimer = 0;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(!_UnlockInProgress) return;

		SeniorBehaviour senior = other.gameObject.GetComponent<SeniorBehaviour>();

		if( senior != null && senior._Type == SeniorBehaviour.SeniorType.Thief)
		{
			_UnlockTimer += Time.deltaTime;

			_IconSprite.fillAmount = _UnlockTimer/SeniorManager.instance._Param.TimeToUnlockDoor;

			if( _UnlockTimer > SeniorManager.instance._Param.TimeToUnlockDoor)
			{
				UnlockDoor();
				_UnlockInProgress = false;
				_IconPanel.alpha = 0;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(!_UnlockInProgress) return;
		
		SeniorBehaviour senior = other.gameObject.GetComponent<SeniorBehaviour>();
		
		if( senior != null && senior._Type == SeniorBehaviour.SeniorType.Thief)
		{
			_UnlockInProgress = false;
			_IconPanel.alpha = 0;
		}
	}
}
