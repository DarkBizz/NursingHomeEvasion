using UnityEngine;
using System.Collections;

public class CameraController  : MonoBehaviour, InputManager.Listener
{

	public float _speed = 1;
	private Camera _camera;

	// Use this for initialization
	void Start () 
	{
		InputManager.instance.registerListener(this);
		_camera = this.gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void onPointerDown ( int _id ,Vector2 _position )
	{
	}
	
	public void onPointerUp ( int _id ,Vector2 _position )
	{
		Ray ray = Camera.main.ScreenPointToRay(_position);
		RaycastHit[] raysHits = Physics.RaycastAll(ray);
		RaycastHit hit = new RaycastHit();
		bool bHit;

		bHit = ClickOnTagObject(raysHits,"Senior",ref hit);
		
		if(bHit) 
		{
			//TODO: changer de perso
			GoToPosition(hit.point);
			SeniorManager.instance._seniorSelected = hit.collider.gameObject.GetComponent<SeniorBehaviour>();
		}
	}
	
	public void onPointerDrag ( int _id ,Vector2 _position , Vector2 _displacement)
	{
		if(_id != 0) return;

		Vector3 Xoffset = -_camera.transform.right;
		Xoffset.Normalize();
		Xoffset *= _displacement.x * _speed;
		Xoffset /= Screen.height;

		Vector3 Zoffset = - _camera.transform.forward;
		Zoffset.y = 0;
		Zoffset.Normalize();
		Zoffset *= _displacement.y * _speed;
		Zoffset /= Screen.height;

		Vector3 camPos = this.transform.parent.transform.position + Xoffset + Zoffset;

		this.transform.parent.transform.position = camPos;
	}


	public void GoToPosition(Vector3 _position)
	{	
		Vector3 _newPosition = _position;
		_position.y = this.transform.parent.transform.position.y;

		this.transform.parent.transform.position = _position;
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
}
