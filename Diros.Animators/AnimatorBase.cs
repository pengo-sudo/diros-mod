using UnityEngine;
using Diros.Rigging;

namespace Diros.Animators;

public abstract class AnimatorBase : MonoBehaviour
{
	protected Rig Rig { get; set; }
	protected Rigidbody Rigidbody { get; set; }
	protected Transform Body { get; set; }
	protected HandDriver LeftHand { get; set; }
	protected HandDriver RightHand { get; set; }

	protected virtual void Awake()
	{
		Rig = Rig.Instance;
		Rigidbody = Rig.rigidbody;
		Body = Rig.body;
		LeftHand = Rig.leftHand;
		RightHand = Rig.rightHand;
		Setup();
	}

	public abstract void Animate();

	public virtual void Setup()
	{
	}

	public virtual void Cleanup()
	{
	}
}
