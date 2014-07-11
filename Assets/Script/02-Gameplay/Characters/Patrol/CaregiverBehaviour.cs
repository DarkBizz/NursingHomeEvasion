using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaregiverBehaviour : MonoBehaviour {


	private NavMeshAgent agent;

	public float _visionRadius = 5;
	public float _visionAnglePatrol = 45;
	public float _visionAngleAlert = 45;
	public LayerMask cullingMask;

	public enum State 
	{
		Idle,
		Patrol, 
		Inspect,
		Chase
	}

	private State _state;


	public int _CurrentPathIndex = -1;
	public List<Transform> _PatrolPointList = new List<Transform>();

	private GameObject _target;

	// Use this for initialization
	void Start () 
	{

		agent = GetComponent<NavMeshAgent>();

		if(_PatrolPointList.Count < 1)
		{
			Debug.LogWarning("CaregiverBehaviour : The caregiver " + this.name + " has no patrol point");
			SetIdle();
		}
		else 
		{
			SetPatrol();
		}
	}

	private void SetIdle()
	{
		_state = State.Idle;
	}

	private void SetPatrol()
	{
		_state = State.Patrol;
		_CurrentPathIndex = GetClosestPatrolPoint();
		agent.SetDestination( _PatrolPointList[_CurrentPathIndex].position );
	}

	private void SetChase()
	{
		_state = State.Chase;
		agent.SetDestination( _target.transform.position );
	}

	private int GetClosestPatrolPoint()
	{
		int iResult = 0;
		float minDistance = Vector3.Distance(this.transform.position,_PatrolPointList[0].position);

		for(int i = 1; i < _PatrolPointList.Count; i++)
		{
			if(Vector3.Distance(this.transform.position,_PatrolPointList[i].position) < minDistance) iResult = i;
		}
		return iResult;
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateVision();

		switch(_state)
		{
		case State.Idle:
			UpdateIddle(); break;
		case State.Patrol:
			UpdatePatrol(); break;
		case State.Chase:
			UpdateChase(); break;
		}
	}

	void UpdatePatrol()
	{
		if(agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0) 
		{
			_CurrentPathIndex++;
			if(_CurrentPathIndex >= _PatrolPointList.Count) _CurrentPathIndex = 0;
			agent.SetDestination( _PatrolPointList[_CurrentPathIndex].position);
		}
	}

	void UpdateChase()
	{
	}

	void UpdateIddle()
	{
	}

	void UpdateVision()
	{
		for(int i = 0; i < SeniorManager.instance._seniorsList.Count; i++)
		{
			Vector3 SeniorPos = SeniorManager.instance._seniorsList[i].transform.position;
			if(CanSeePlayer(SeniorPos))
			{
				_target = SeniorManager.instance._seniorsList[i].gameObject;
				SetChase();
			}
		}
	}

	private bool CanSeePlayer(Vector3 playerPos)
	{
		if(Vector3.Distance(playerPos,this.transform.position) > _visionRadius) return false;
		Vector3 pos = this.transform.position;
		pos.y = 1;
		Vector3 direction = playerPos - pos;

		if(Vector3.Angle(this.transform.forward,direction) > (GetAngleVision()/2)) return false;
		RaycastHit hit = new RaycastHit();

		Ray ray = new Ray(pos,direction.normalized);
		Debug.DrawRay(pos,direction.normalized,Color.cyan);
		Debug.DrawLine(pos,hit.point,Color.yellow);

		Physics.Raycast(ray,out hit,_visionRadius,cullingMask);
		if(hit.collider == null || hit.collider.tag != "Senior") return false;
		Debug.Log("I SEEEEEEEEEEEEEE U !");
		return true;
	}

	private float GetAngleVision()
	{
		if(_state == State.Patrol) return _visionAnglePatrol;
		else return _visionAngleAlert;
	}
}
