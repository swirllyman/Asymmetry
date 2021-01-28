using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asymmetry
{
	public class VRMovement : OVRPlayerController
	{
		public override void UpdateMovement()
		{
			if (HaltUpdateMovement)
				return;

			if (EnableLinearMovement)
			{
				MoveScale = 1.0f;
				// No positional movement if we are in the air
				if (!Controller.isGrounded)
					MoveScale = 0.0f;

				MoveScale *= SimulationRate * Time.deltaTime;

				// Compute this for key movement
				float moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;

				Quaternion ort = transform.rotation;
				Vector3 ortEuler = ort.eulerAngles;
				ortEuler.z = ortEuler.x = 0f;
				ort = Quaternion.Euler(ortEuler);


				moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;

#if !UNITY_ANDROID // LeftTrigger not avail on Android game pad
				moveInfluence *= 1.0f + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
#endif

				Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

				// If speed quantization is enabled, adjust the input to the number of fixed speed steps.
				if (FixedSpeedSteps > 0)
				{
					primaryAxis.y = Mathf.Round(primaryAxis.y * FixedSpeedSteps) / FixedSpeedSteps;
					primaryAxis.x = Mathf.Round(primaryAxis.x * FixedSpeedSteps) / FixedSpeedSteps;
				}

				if (primaryAxis.y > 0.0f)
					MoveThrottle += ort * (primaryAxis.y * transform.lossyScale.z * moveInfluence * Vector3.forward);

				if (primaryAxis.y < 0.0f)
					MoveThrottle += ort * (Mathf.Abs(primaryAxis.y) * transform.lossyScale.z * moveInfluence *
										   BackAndSideDampen * Vector3.back);

				if (primaryAxis.x < 0.0f)
					MoveThrottle += ort * (Mathf.Abs(primaryAxis.x) * transform.lossyScale.x * moveInfluence *
										   BackAndSideDampen * Vector3.left);

				if (primaryAxis.x > 0.0f)
					MoveThrottle += ort * (primaryAxis.x * transform.lossyScale.x * moveInfluence * BackAndSideDampen *
										   Vector3.right);
			}

			if (EnableRotation)
			{
				Vector3 euler = transform.rotation.eulerAngles;
				float rotateInfluence = SimulationRate * Time.deltaTime * RotationAmount * RotationScaleMultiplier;
				euler.y += buttonRotation;
				buttonRotation = 0f;

				if (SnapRotation)
				{
					if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft) ||
						(RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft)))
					{
						if (ReadyToSnapTurn)
						{
							euler.y -= RotationRatchet;
							ReadyToSnapTurn = false;
						}
					}
					else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight) ||
						(RotationEitherThumbstick && OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight)))
					{
						if (ReadyToSnapTurn)
						{
							euler.y += RotationRatchet;
							ReadyToSnapTurn = false;
						}
					}
					else
					{
						ReadyToSnapTurn = true;
					}
				}
				else
				{
					Vector2 secondaryAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
					if (RotationEitherThumbstick)
					{
						Vector2 altSecondaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
						if (secondaryAxis.sqrMagnitude < altSecondaryAxis.sqrMagnitude)
						{
							secondaryAxis = altSecondaryAxis;
						}
					}
					euler.y += secondaryAxis.x * rotateInfluence;
				}

				transform.rotation = Quaternion.Euler(euler);
			}

			if (Input.GetButtonDown("Jump") && Controller.isGrounded)
			{
				Jump();
			}
		}
	}
}