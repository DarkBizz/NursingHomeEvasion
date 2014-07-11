using UnityEngine;
using System.Collections;

public class SeniorButton : MonoBehaviour {

	public SeniorBehaviour _seniorTarget;
 	void OnClick()
	{
		SeniorManager.instance._seniorSelected = _seniorTarget;
		CameraController.instance.GoToPosition(_seniorTarget.transform.position);
	}
}
