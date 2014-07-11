using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour 
{

	private List<GameObject> _enemyList = new List<GameObject>();

	private static EnemyManager _instance;
	public static EnemyManager instance {get{ return _instance; }}
	// Use this for initialization

	void Awake ()
	{
		_instance = this;
	}
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RegistryEnemy(GameObject enemy)
	{
		if(_enemyList.Contains(enemy))return;

		_enemyList.Add(enemy);
	}

	public GameObject GetClosestEnemy( Vector3 position )
	{
		GameObject closest = _enemyList[0];
		float minDistance = Vector3.Distance(this.transform.position,_enemyList[0].transform.position);
		
		for(int i = 1; i < _enemyList.Count; i++)
		{
			float distance = Vector3.Distance(position,_enemyList[i].transform.position); 
			if( distance < minDistance) 
			{
				minDistance = distance;
				closest = _enemyList[i];
			}
		}
		return closest;
	}
}
