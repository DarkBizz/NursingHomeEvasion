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

		Vector3 camPos = _camera.transform.position + Xoffset + Zoffset;

		_camera.transform.position = camPos;
	}


	private void GoToPosition()
	{
	}
}
