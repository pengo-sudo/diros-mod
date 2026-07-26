using UnityEngine;

namespace Diros.Rigging;

public class HandDriver : MonoBehaviour
{
	public Vector3 targetPosition;
	public Vector3 hit;
	public Vector3 normal;
	public Vector3 lookAt;
	public Vector3 DefaultPosition { get; private set; }
	public bool grounded;
	public Vector3 lastSnap;
	public Vector3 up = Vector3.up;

	public void Init(bool initIsLeft)
	{
		DefaultPosition = ((Component)this).transform.localPosition;
	}

	public void Reset()
	{
		targetPosition = DefaultPosition;
	}

	private void LateUpdate()
	{
		((Component)this).transform.position = targetPosition;
		if (up != Vector3.zero)
		{
			((Component)this).transform.up = up;
		}
	}
}
