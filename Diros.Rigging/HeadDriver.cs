using UnityEngine;

namespace Diros.Rigging;

public class HeadDriver : MonoBehaviour
{
	public static HeadDriver instance;

	private Transform headTransform;
	private Quaternion defaultRotation;
	private float tiltAmount = 0f;

	private void Awake()
	{
		instance = this;
		headTransform = ((Component)this).GetComponent<Transform>();
		defaultRotation = headTransform.localRotation;
	}

	public void TiltDown(float degrees)
	{
		tiltAmount = degrees;
		Quaternion tiltRotation = Quaternion.Euler(degrees, 0f, 0f);
		headTransform.localRotation = defaultRotation * tiltRotation;
	}

	public void ResetTilt()
	{
		tiltAmount = 0f;
		headTransform.localRotation = defaultRotation;
	}
}
