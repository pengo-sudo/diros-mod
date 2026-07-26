using System;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Diros.Plugin;
using Diros.Rigging;
using Diros.Tools;

namespace Diros.Animators;

public class DirosAnimator : AnimatorBase
{
	private float speed = 3f;
	private float height = 0.2f;
	private float heightModifier = 0f;
	private bool isWalking = false;
	private Vector3 currentDirection = Vector3.zero;
	private Vector3 targetDestination = Vector3.zero;
	
	private BehaviorLogger behaviorLogger;
	private HeadDriver headDriver;
	
	private bool fleeing = false;
	private float fleeStartTime = 0f;
	private const float FLEE_TIMEOUT = 5f;
	private const float DETECTION_RANGE = 5f;
	
	private Vector3 lastPosition = Vector3.zero;
	private float walkCycleTime = 0f;
	private string currentMap = "";
	private string lastMap = "";

	public override void Animate()
	{
		CheckMapChange();
		MoveBody();
		AnimateHands();
		UpdateAI();
	}

	private void CheckMapChange()
	{
		currentMap = SceneManager.GetActiveScene().name;
		if (currentMap != lastMap)
		{
			lastMap = currentMap;
			behaviorLogger.LogEvent($"Entered new map ({currentMap})");
		}
	}

	public void MoveBody()
	{
		Rig.active = Rig.onGround;
		Rig.useGravity = !Rig.onGround;
		
		if (Rig.onGround)
		{
			float num3;
			float num;
			float num2;
			
			if (currentDirection == Vector3.zero)
			{
				num = 0.5f;
				num2 = 0.55f;
				num3 = Time.time * Mathf.PI * 2f;
			}
			else
			{
				num = 0.3f;
				num2 = 0.8f;
				num3 = walkCycleTime * Mathf.PI * 2f;
			}
			
			num += heightModifier;
			num2 += heightModifier;
			height = Extensions.Map(Mathf.Sin(num3), -1f, 1f, num, num2);
			
			Vector3 val = Rig.lastGroundPosition + Vector3.up * (height * GTPlayer.Instance.NativeScale);
			Vector3 val2 = Rig.body.TransformDirection(currentDirection);
			val2.y = 0f;
			if (Vector3.Dot(Rig.lastNormal, Vector3.up) > 0.3f)
			{
				val2 = Vector3.ProjectOnPlane(val2, Rig.lastNormal);
			}
			val2 *= GTPlayer.Instance.NativeScale;
			val += val2 * speed / 10f;
			Rig.targetPosition = val;
			
			isWalking = Vector3.Distance(lastPosition, Rig.targetPosition) > 0.01f;
			lastPosition = Rig.targetPosition;
		}
	}

	private void AnimateHands()
	{
		if (!Rig.onGround)
		{
			LeftHand.grounded = false;
			RightHand.grounded = false;
			Vector3 val = Vector3.up * (0.2f * GTPlayer.Instance.NativeScale);
			LeftHand.targetPosition = LeftHand.DefaultPosition;
			RightHand.targetPosition = RightHand.DefaultPosition + val;
			return;
		}
		
		if (currentDirection == Vector3.zero)
		{
			LeftHand.targetPosition = LeftHand.DefaultPosition;
			RightHand.targetPosition = RightHand.DefaultPosition;
			return;
		}
		
		// Simple arm swing while walking - no inertia
		float swingAmount = Mathf.Sin(Time.time * 5f) * 0.3f;
		LeftHand.targetPosition = LeftHand.DefaultPosition + Rig.body.right * swingAmount * GTPlayer.Instance.NativeScale;
		RightHand.targetPosition = RightHand.DefaultPosition - Rig.body.right * swingAmount * GTPlayer.Instance.NativeScale;
	}

	private void UpdateAI()
	{
		// Detect nearby player
		bool playerNearby = DetectNearbyPlayer();
		
		if (playerNearby)
		{
			if (!fleeing)
			{
				fleeing = true;
				fleeStartTime = Time.time;
				behaviorLogger.LogEvent("Detected player nearby (fled from them)");
				PickNewDirection();
			}
			
			headDriver.TiltDown(18f);
		}
		else
		{
			if (fleeing && Time.time - fleeStartTime > 2f)
			{
				fleeing = false;
				behaviorLogger.LogEvent("Lost sight of player (walked far enough away)");
			}
			
			if (!fleeing)
			{
				headDriver.ResetTilt();
				
				if (Vector3.Distance(Rig.targetPosition, targetDestination) < 1f)
				{
					behaviorLogger.LogEvent("Reached destination");
					PickNewDirection();
				}
				
				if (UnityEngine.Random.value < 0.3f)
				{
					behaviorLogger.LogEvent("Changed direction mid-walk");
					PickNewDirection();
				}
			}
		}
	}

	private bool DetectNearbyPlayer()
	{
		Player player = Player.Instance;
		if (player == null) return false;
		
		float distance = Vector3.Distance(player.bodyCollider.transform.position, Rig.body.position);
		return distance < DETECTION_RANGE;
	}

	private void PickNewDirection()
	{
		currentDirection = UnityEngine.Random.onUnitSphere;
		currentDirection.y = 0f;
		currentDirection.Normalize();
		
		targetDestination = Rig.targetPosition + currentDirection * 10f;
		behaviorLogger.LogEvent("Started walking toward new destination");
	}

	public override void Setup()
	{
		headDriver = HeadDriver.instance;
		behaviorLogger = ((Component)this).gameObject.AddComponent<BehaviorLogger>();
		currentMap = SceneManager.GetActiveScene().name;
		lastMap = currentMap;
		PickNewDirection();
	}
}
