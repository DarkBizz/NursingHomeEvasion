using UnityEngine;
using System.Collections;

public class SeniorButton : MonoBehaviour {

	public SeniorBehaviour _seniorTarget;
 	void OnClick()
	{
		SeniorManager.instance._seniorSelected = _seniorTarget;
		_seniorTarget._button = this;
		CameraController.instance.GoToPosition(_seniorTarget.transform.position);
	}

	public void DisableButton()
	{
		this.gameObject.GetComponent<UIButton>();
	}
}
