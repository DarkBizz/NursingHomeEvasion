using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaregiverBehaviour : MonoBehaviour {


	private NavMeshAgent agent;

	public float _visionRadius = 5;
	public float _visionAnglePatrol = 45;
	public float _visionAngleAlert = 45;
	public LayerMask cullingMask;

	public float _searchRadius = 5;

	public enum State 
	{
		Idle,
		Patrol, 
		Search,
		Chase
	}

	private State _state;
	private float _ellapsedTime = 0;

	private Vector3 _lastTargetPosition;


	public int _CurrentPathIndex = -1;
	public List<Transform> _PatrolPointList = new List<Transform>();

	private GameObject _target;

	// Use this for initialization
	void Start () 
	{
		EnemyManager.instance.RegistryEnemy(this.gameObject);

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
		if(_target != null) agent.SetDestination( _target.transform.position );
	}

	private void SetSearch()
	{
		_state = State.Search;
		if(_target != null) _lastTargetPosition = _target.transform.position;
		//SearchAtRandomPosition();
	}

	private void SearchAtRandomPosition()
	{
		Vector2 randomPos = Random.insideUnitCircle * _searchRadius;
		Vector3 seachPos = _lastTargetPosition + new Vector3(randomPos.x,0,randomPos.y);

		agent.SetDestination(seachPos);
	}

	public void GoToSuspectPosition(Vector3 p_position)
	{
		SetSearch();
		_lastTargetPosition = p_position;
		agent.SetDestination(_lastTargetPosition);
	}

	private int GetClosestPatrolPoint()
	{
		int iResult = 0;
		float minDistance = Vector3.Distance(this.transform.position,_PatrolPointList[0].position);

		for(int i = 1; i < _PatrolPointList.Count; i++)
		{
			float distance = Vector3.Distance(this.transform.position,_PatrolPointList[i].position); 
			if(distance < minDistance)
			{
				minDistance = distance;
				iResult = i;
			}
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
		case State.Search:
			UpdateSearch(); break;
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

	void UpdateSearch()
	{
		_ellapsedTime += Time.deltaTime;

		if(_ellapsedTime > SeniorManager.instance._Param.SearchDuration)
		{
			SetPatrol();
		}

		if((agent.pathStatus == NavMeshPathStatus.PathComplete || agent.pathStatus == NavMeshPathStatus.PathPartial ) 
		   && agent.remainingDistance == 0)  
			SearchAtRandomPosition();
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
			else if (_state == State.Chase)
			{
				SetSearch();
			}
		}
	}

	private bool CanSeePlayer(Vector3 playerPos)
	{
		if(Vector3.Distance(playerPos,this.transform.position) > _visionRadius) return false;
		Vector3 pos = this.transform.position;
		pos.y = 1;
		Vector3 direction = playerPos - pos;
		direction.y = 0;
		direction.Normalize();

		if(Vector3.Angle(this.transform.forward,direction) > (GetAngleVision()/2)) return false;
		RaycastHit hit = new RaycastHit();

		if(!Physics.Raycast(pos,direction,out hit,_visionRadius,cullingMask)) return false;

		if(hit.collider == null || hit.collider.tag != "Senior") return false;
		return true;
	}

	private float GetAngleVision()
	{
		if(_state == State.Patrol) return _visionAnglePatrol;
		else return _visionAngleAlert;
	}

	private void OnDrawGizmosSelected()
	{
		for(int i = 0; i < _PatrolPointList.Count; i++)
		{
			Gizmos.DrawIcon(_PatrolPointList[i].transform.position,"icon_point.png");
			int j = i+1;
			if(j == _PatrolPointList.Count) j = 0;

			Gizmos.DrawLine(_PatrolPointList[i].transform.position,_PatrolPointList[j].transform.position);
		}
	}
}
