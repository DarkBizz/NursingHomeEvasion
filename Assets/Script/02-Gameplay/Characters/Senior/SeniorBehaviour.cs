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

		//agent.SetDestination(destination);
	}

	public void GoToDoor(DoorBehaviour door)
	{
		NavMeshPath pathStart = new NavMeshPath();
		NavMeshPath pathEnd = new NavMeshPath();

		bool HasPath01 = agent.CalculatePath(door.link.startTransform.position,pathStart);
		bool HasPath02 = agent.CalculatePath(door.link.endTransform.position,pathEnd);


	}
	
}
