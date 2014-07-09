using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InGameDebugLog : MonoBehaviour 
{

	public bool _bDisplayDebug = false;

	private  Dictionary<string,DebugValue> _ValueList = new Dictionary<string,DebugValue>();

	private static InGameDebugLog _instance = null;

	public static InGameDebugLog instance () { return _instance; }

	void Awake ( ) {
		if( null==_instance ) {
			_instance = this;
		}else Debug.LogError( "Cannot have two instances of InGameDebugLog." );
	}
	
	void OnDestroy () {
		_instance = null;
	}

	private abstract class DebugValue 
	{
		public abstract string GetValue();
	}
	private class DebugValue<DataType> : DebugValue where DataType : struct
	{
		public string 	_name; 
		public DataType _value;
		
		public DebugValue(string p_name, DataType p_value)
		{
			_name = p_name;
			_value = p_value;
		}

		public override string GetValue()
		{
			return _value.ToString();
		}
	}
	
	public void RegisterValue<T>(string p_Name, T p_Value) where T : struct
	{
		// if Debug Value already exist
		if(_ValueList.ContainsKey(p_Name))
		{
			try
			{
			DebugValue<T> value = (DebugValue<T>)Convert.ChangeType(
				_ValueList[p_Name], typeof(DebugValue<T>), null);

			value._value = p_Value;
			}
			catch (InvalidCastException e)
			{
				Debug.Log( "InGameDebugLog > UpdateValue : " + p_Name + " : Invalid Cast >\n" + e.Message);
			}
		}
		else
		{
			_ValueList.Add(p_Name,new DebugValue<T>(p_Name,p_Value));
		}
	}

	void OnGUI()
	{
		if(!_bDisplayDebug || _ValueList.Count == 0) return;

		float fHeight = Screen.height * 0.9f;
		float fWidth = fHeight * 0.5f;
		Rect LabelRect = new Rect ( Screen.width/2 - fWidth/2,Screen.height/2 - fHeight/2,fWidth,fHeight); 

		GUIStyle labelStyle = GUI.skin.label;
		labelStyle.fontSize = (int)(fHeight * 2.25f/100f);

		string sText = "DebugLog :";

		foreach(KeyValuePair<string,DebugValue> pair in _ValueList)
		{
			sText += "\n" + pair.Key.ToString() + " : " + pair.Value.GetValue();
		}
		GUI.Label(LabelRect,sText,labelStyle);
	}
}
