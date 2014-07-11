using UnityEngine;
using System.Collections;

public class SeniorBehaviour : MonoBehaviour 
{
	private NavMeshAgent agent;

	public enum SeniorType 
	{
		Veteran = 0,
		Thief = 1,
		Tank = 2
	}
	public SeniorType _Type = SeniorType.Veteran;

	private float _UnlockDoorTimer = 0;

	public SeniorButton _button;

	public bool busted = false;

	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent>();
		SeniorManager.instance.registerSenior(this);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void GoToDestination(Vector3 destination)
	{
		NavMeshPath path = new NavMeshPath();
		bool HasPath = agent.CalculatePath(destination,path);
		if(HasPath && path.status == NavMeshPathStatus.PathComplete)
		{
			agent.SetPath(path);
		}
	}

	public void GoToDoor(DoorBehaviour door)
	{
		Debug.Log("GoToDoor");
		NavMeshPath pathStart = new NavMeshPath();
		NavMeshPath pathEnd = new NavMeshPath();

		bool HasPath01 = agent.CalculatePath(door.link.startTransform.position,pathStart);
		bool HasPath02 = agent.CalculatePath(door.link.endTransform.position,pathEnd);

		if(!HasPath01 && !HasPath02) return;
		else if(!HasPath01 && HasPath02) agent.SetPath(pathEnd);
		else if(HasPath01 && !HasPath02) agent.SetPath(pathStart);
		else if( GetPathLength(pathStart) < GetPathLength(pathEnd) ) agent.SetPath(pathStart);
		else agent.SetPath(pathEnd);
	}

	private float GetPathLength( NavMeshPath path )
	{
		float length = 0;
		for(int i = 0; i < path.corners.Length - 1; i++)
		{
			length += Vector3.Distance(path.corners[i],path.corners[i+1]);
		}
		return length;
	}
}
