using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour 
{
	public interface Listener
	{
		void onPointerDown ( int _id ,Vector2 _position );
		void onPointerUp ( int _id ,Vector2 _position );
		void onPointerDrag ( int _id ,Vector2 _position, Vector2 _displacement );
	}

	public void registerListener ( Listener _pointerListener ) {
		_listeners.Add( _pointerListener );
	}
	
	public void unregisterListener ( Listener _pointerListener ) {
		_listeners.Remove( _pointerListener );
	}

	private static InputManager _instance;
	public static InputManager instance { get{return _instance;}}

	private bool[] _isPressed = new bool[3];
	private Vector2 _curPos;

	private List<Listener> _listeners = new List<Listener>();

	void Awake ()
	{
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 curPos = Input.mousePosition;

		for(int i = 0; i < 3; i++)
		{
			bool isPressed = Input.GetMouseButton(i);
			if( isPressed!=_isPressed[i] ) {
				if( isPressed ) _sendPointerDown( i ,curPos );
				else 			_sendPointerUp( i ,curPos );
				_isPressed[i] = isPressed;
			}else if( isPressed && (curPos!=_curPos) ) {
				_sendPointerDrag ( i ,curPos, curPos - _curPos);

			}
		}
		_curPos = curPos;
	}

	public Vector2 getPointerPosition () {
		return _curPos;
	}

	protected void _sendPointerDown ( int _id ,Vector2 _position ) {
		foreach( Listener curListener in _listeners )
			curListener.onPointerDown ( _id ,_position );
	}
	
	protected void _sendPointerUp ( int _id ,Vector2 _position ) {
		foreach( Listener curListener in _listeners )
			curListener.onPointerUp ( _id ,_position );
	}
	
	protected void _sendPointerDrag ( int _id ,Vector2 _position, Vector2 _displacement ) {
		foreach( Listener curListener in _listeners )
			curListener.onPointerDrag ( _id ,_position,_displacement );
	}
}
