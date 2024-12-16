using Net;
using UnityEngine;

namespace Net
{
	/// <summary>
	/// The player body manages the "robot" and represents its collider and physics.
	/// It also manage the movement (but only to not overload the controller)
	/// </summary>
	public class PlayerBody : MonoBehaviour
	{
		[Header("The default speed of the robot (straight)")]
		public float speedMovement = 6;

		[Header("The default rotating speed of the robot")]
		public float speedRotation = 200;

		[Header("The body for movement")]
		public Rigidbody Body;

		/// <summary>
		/// The player this body is controlled from.
		/// Set by the controller
		/// </summary>
		internal PlayerController player;

		/// <summary>
		/// Move the body of the player.
		///
		/// This works thanks to the NetworkTransformClient Utils.
		/// </summary>
		/// <param name="deltaTime">or `fixedDeltaTime` depending of the caller</param>
		public void HandleMovement(float deltaTime)
		{
			var isForward = Input.GetKey(KeyCode.W);
			if (isForward || Input.GetKey(KeyCode.S))
			{
				// The added force (forward the robot)
				var delta = speedMovement * deltaTime;
				var force = Body.rotation * Vector3.forward * delta;

				Body.MovePosition(Body.position + force * (isForward ? 1 : -1));
			}

			var isLeft = Input.GetKey(KeyCode.A);
			if (isLeft || Input.GetKey(KeyCode.D))
			{
				var delta = speedRotation * deltaTime;
				var force = Body.rotation.eulerAngles + new Vector3(0, isLeft ? -1 : 1, 0) * delta;

				Body.MoveRotation(Quaternion.Euler(force.x, force.y, force.z));
			}
		}
	}
}
