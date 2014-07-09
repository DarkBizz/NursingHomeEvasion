using UnityEngine;
using System.Collections;

public class SecurityCameraBehaviour : MonoBehaviour {

	FOV2DEyes eyes;
	FOV2DVisionCone visionCone;

	public float _RotationTime = 2;
	public float _RotationAngle = 45;
	public AnimationCurve _RotationCurve;

	private Quaternion InitialAngle;
	private float ellapseTime = 0;

	public bool seeSomething = false;
	// Use this for initialization
	void Start () 
	{
		InitialAngle = transform.rotation;
		eyes = GetComponentInChildren<FOV2DEyes>();
		visionCone = GetComponentInChildren<FOV2DVisionCone>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateRotation();

		CheckSeniorInView();
	}

	private void UpdateRotation()
	{
		ellapseTime += Time.deltaTime;
		if(ellapseTime > _RotationTime) ellapseTime = 0;
		
		float offset = _RotationCurve.Evaluate(ellapseTime/_RotationTime) * _RotationAngle;

		transform.rotation = Quaternion.Euler(
			InitialAngle.eulerAngles.x,
			InitialAngle.eulerAngles.y + offset,
			InitialAngle.eulerAngles.z);
	}

	private void CheckSeniorInView()
	{
		bool seniorInView = false;
		
		foreach (RaycastHit hit in eyes.hits)
		{
			if (hit.transform && hit.transform.tag == "Senior")
			{
				seniorInView = true;
				break;
			}
		}
		
		if (seniorInView)
		{
			visionCone.status = FOV2DVisionCone.Status.Alert;
			seeSomething = true;
		}
		else
		{
			visionCone.status = FOV2DVisionCone.Status.Idle;
			seeSomething = false;
		}
	}
}
