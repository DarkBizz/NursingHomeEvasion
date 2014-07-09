using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour 
{
	public interface Listener
	{
		void onPointerDown ( int _id ,Vector2 _position );
		void onPointerUp ( int _id ,Vector2 _position );
		void onPointerDrag ( int _id ,Vector2 _position );
	}

	public void registerListener ( Listener _pointerListener ) {
		_listeners.Add( _pointerListener );
	}
	
	public void unregisterListener ( Listener _pointerListener ) {
		_listeners.Remove( _pointerListener );
	}

	private static InputManager _instance;
	public static InputManager instance { get{return _instance;}}

	private List<Listener> _listeners = new List<Listener>();

	void Awake ()
	{
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))_sendPointerDown(0,Input.mousePosition);
		if(Input.GetMouseButtonUp(0))_sendPointerUp(0,Input.mousePosition);
	}

	protected void _sendPointerDown ( int _id ,Vector2 _position ) {
		foreach( Listener curListener in _listeners )
			curListener.onPointerDown ( _id ,_position );
	}
	
	protected void _sendPointerUp ( int _id ,Vector2 _position ) {
		foreach( Listener curListener in _listeners )
			curListener.onPointerUp ( _id ,_position );
	}
	
	protected void _sendPointerDrag ( int _id ,Vector2 _position ) {
		foreach( Listener curListener in _listeners )
			curListener.onPointerDrag ( _id ,_position );
	}
}
