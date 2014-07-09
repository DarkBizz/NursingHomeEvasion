using UnityEngine;
using UnityEditor;
using System;

public class SeniorConfigEditor
{
	[MenuItem("Assets/Create/Asset/Senior Parameters")]
	
	public static void CreateAsset ()
	{
		CustomAssetUtility.CreateAsset<SeniorConfig> ();
	}
}