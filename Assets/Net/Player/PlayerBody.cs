using System.Collections.Generic;
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

		[Header("Elements to show/hide when needed")]
		public List<GameObject> visuals;

		/// <summary>
		/// The player this body is controlled from.
		/// Set by the controller
		/// </summary>
		internal PlayerController player;

		public void Start()
		{
			this.Body.GetComponent<Renderer>().enabled = false;
		}

		/// <summary>
		/// Move the body of the player.
		///
		/// This works thanks to the NetworkTransformClient Utils.
		/// </summary>
		/// <param name="deltaTime">or `fixedDeltaTime` depending of the caller</param>
		public void HandleMovement(float deltaTime)
		{
			var isForward = Input.GetKey(KeyCode.W);
			var speed = getSpeed();

			if (isForward || Input.GetKey(KeyCode.S))
			{
				// The added force (forward the robot)
				var delta = speedMovement * deltaTime * speed;
				var force = Body.rotation * Vector3.forward * delta;

				Body.MovePosition(Body.position + force * (isForward ? 1 : -1));
			}

			var isLeft = Input.GetKey(KeyCode.A);
			if (isLeft || Input.GetKey(KeyCode.D))
			{
				var delta = speedRotation * deltaTime * speed;
				var force = Body.rotation.eulerAngles + new Vector3(0, isLeft ? -1 : 1, 0) * delta;

				Body.MoveRotation(Quaternion.Euler(force.x, force.y, force.z));
			}
		}

		public void Show()
		{
			foreach (var visual in visuals)
			{
				visual.SetActive(true);
			}
		}

		public void Hide()
		{
			foreach (var visual in visuals)
			{
				visual.SetActive(false);
			}
		}

		private float getSpeed()
		{
			var hasSpeed = player.PowerUps.powerUp == PowerUp.SPEED;

			return getSpeedHealth() * (hasSpeed ? 1.2f : 1f);
		}

		private float getSpeedHealth()
		{
			var health = player.Resources.Health.Value;

			if (health > 75)
			{
				return 1;
			}

			if (health > 60)
			{
				return 0.9f;
			}
			if (health > 50)
			{
				return 0.8f;
			}

			return 0.6f;
		}
	}
}
