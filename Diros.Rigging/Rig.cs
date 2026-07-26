using GorillaLocomotion;
using UnityEngine;
using Diros.Animators;

namespace Diros.Rigging;

public class Rig : MonoBehaviour
{
	private const float RaycastLength = 1.3f;
	private const float RaycastRadius = 0.3f;

	public Transform head;
	public Transform body;
	public HeadDriver headDriver;
	public HandDriver leftHand;
	public HandDriver rightHand;
	public Rigidbody rigidbody;

	public Vector3 targetPosition;
	public Vector3 lastNormal;
	public Vector3 lastGroundPosition;
	public bool onGround;
	public bool active;
	public bool useGravity = true;

	private readonly Vector3 raycastOffset = new Vector3(0f, 0.4f, 0f);
	private AnimatorBase animator;
	private float scale = 1f;

	public static Rig Instance { get; private set; }

	public Vector3 SmoothedGroundPosition { get; set; }

	public AnimatorBase Animator
	{
		get
		{
			return animator;
		}
		set
		{
			if (Object.op_Implicit((Object)(object)animator) && (Object)(object)value != (Object)(object)animator)
			{
				animator.Cleanup();
			}
			animator = value;
			if (Object.op_Implicit((Object)(object)animator))
			{
				((Behaviour)animator).enabled = true;
				animator.Setup();
			}
			((Behaviour)leftHand).enabled = Object.op_Implicit((Object)(object)animator);
			((Behaviour)rightHand).enabled = Object.op_Implicit((Object)(object)animator);
			((Behaviour)headDriver).enabled = Object.op_Implicit((Object)(object)animator);
			if (!Object.op_Implicit((Object)(object)animator))
			{
				leftHand.Reset();
				rightHand.Reset();
			}
		}
	}

	private void Awake()
	{
		Instance = this;
		head = ((Component)GTPlayer.Instance.headCollider).transform;
		body = ((Component)GTPlayer.Instance.bodyCollider).transform;
		rigidbody = ((Collider)GTPlayer.Instance.bodyCollider).attachedRigidbody;
		
		leftHand = new GameObject("Diros Left Hand Driver").AddComponent<HandDriver>();
		leftHand.Init(initIsLeft: true);
		((Behaviour)leftHand).enabled = false;
		
		rightHand = new GameObject("Diros Right Hand Driver").AddComponent<HandDriver>();
		rightHand.Init(initIsLeft: false);
		((Behaviour)rightHand).enabled = false;
		
		headDriver = new GameObject("Diros Head Driver").AddComponent<HeadDriver>();
		((Behaviour)headDriver).enabled = false;
	}

	private void FixedUpdate()
	{
		scale = GTPlayer.Instance.NativeScale;
		targetPosition = body.position;
		lastGroundPosition = body.position;
		onGround = true;
		lastNormal = Vector3.up;
		SmoothedGroundPosition = lastGroundPosition;
		
		if (active && rigidbody.isKinematic)
		{
			rigidbody.velocity = Vector3.zero;
		}
		
		if (!active && !rigidbody.isKinematic)
		{
			rigidbody.isKinematic = false;
		}
		
		if (useGravity && !rigidbody.useGravity)
		{
			rigidbody.useGravity = true;
		}
		else if (!useGravity && rigidbody.useGravity)
		{
			rigidbody.useGravity = false;
		}
	}
}
