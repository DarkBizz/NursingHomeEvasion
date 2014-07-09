using UnityEngine;
using System.Collections;

public class DoorBehaviour : MonoBehaviour {

	public OffMeshLink link; 

	public bool _Lock = false;

	// Use this for initialization
	void Start () 
	{
		if(_Lock) LockDoor();
		else UnlockDoor();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void UnlockDoor() {link.activated = true;}
	public void LockDoor() {link.activated = false;}
}
