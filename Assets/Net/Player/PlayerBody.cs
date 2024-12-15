using Net;
using Unity.Netcode;
using UnityEngine;

namespace Net
{
	/// <summary>
	/// The player body manages the "robot" and represents its collider and physics.
	/// It also manage the movement (but only to not overload the controller)
	/// </summary>
	public class PlayerBody : NetworkBehaviour
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

		// Variables for network movement (only used in the server).
		// TODO: a client rigibody transform for that ?
		private bool isMovingForward = false;
		private bool isMovingBackward = false;
		private bool isRotatingLeft = false;
		private bool isRotatingRight = false;

		void FixedUpdate()
		{
			if (!this.IsServer)
			{
				// Only the server side does the "real" updates.
				return;
			}

			if (this.isMovingForward || isMovingBackward)
			{
				// The added force (forward the robot)
				var delta = speedMovement * Time.fixedDeltaTime;
				var force = Body.rotation * Vector3.forward * delta;

				Body.MovePosition(Body.position + force * (this.isMovingForward ? 1 : -1));
			}

			if (this.isRotatingLeft || this.isRotatingRight)
			{
				var delta = speedRotation * Time.fixedDeltaTime;
				var force =
					Body.rotation.eulerAngles
					+ new Vector3(0, this.isRotatingLeft ? -1 : 1, 0) * delta;

				Body.MoveRotation(Quaternion.Euler(force.x, force.y, force.z));
			}
		}

		[Rpc(SendTo.Server)]
		private void moveForwardRpc(bool up)
		{
			this.isMovingForward = up;
		}

		[Rpc(SendTo.Server)]
		private void moveBackwardRpc(bool down)
		{
			this.isMovingBackward = down;
		}

		[Rpc(SendTo.Server)]
		private void rotateLeftRpc(bool down)
		{
			this.isRotatingLeft = down;
		}

		[Rpc(SendTo.Server)]
		private void rotateRightRpc(bool down)
		{
			this.isRotatingRight = down;
		}

		/// <summary>
		/// Handle the movement (with network).
		/// To be called in a `Update` method.
		/// Each movement send toggle to the server that will do the "real" movement
		/// </summary>
		public void HandleMovement()
		{
			// Move up
			if (Input.GetKeyDown(Constants.Actions.MoveUp))
			{
				this.moveForwardRpc(true);
			}
			if (Input.GetKeyUp(Constants.Actions.MoveUp))
			{
				this.moveForwardRpc(false);
			}

			// Move down
			if (Input.GetKeyDown(Constants.Actions.MoveDown))
			{
				this.moveBackwardRpc(true);
			}
			if (Input.GetKeyUp(Constants.Actions.MoveDown))
			{
				this.moveBackwardRpc(false);
			}

			// Rotate left
			if (Input.GetKeyDown(Constants.Actions.MoveLeft))
			{
				this.rotateLeftRpc(true);
			}
			if (Input.GetKeyUp(Constants.Actions.MoveLeft))
			{
				this.rotateLeftRpc(false);
			}

			// Rotate left
			if (Input.GetKeyDown(Constants.Actions.MoveRight))
			{
				this.rotateRightRpc(true);
			}
			if (Input.GetKeyUp(Constants.Actions.MoveRight))
			{
				this.rotateRightRpc(false);
			}
		}
	}
}
