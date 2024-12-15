using Unity.Netcode;
using UnityEngine;

namespace Net
{
	public class MineController : NetworkBehaviour
	{
		[Header("On \"debug\", always show this element, no matter the player")]
		public GameObject DebugIndicator;

		[Header("Indicator that the mine editable (de-mine)")]
		public GameObject asEditable;

		[Header("The mine object")]
		public Renderer Mine;

		/// <summary>
		/// Player that set this mine
		/// </summary>
		internal PlayerController Player;

		// TODO: the question of this mine

		// FIXME: better
		internal bool firstInit = false;

		void Start()
		{
			this.DebugIndicator.SetActive(Constants.DebugShowMines);
		}

		void OnTriggerEnter(Collider other)
		{
			// TODO: explosion

			if (other.TryGetComponent<PlayerBody>(out var playerBody))
			{
				if (this.firstInit)
				{
					// [Temporary]: avoid the explosion when setting the mine
					this.firstInit = false;
					return;
				}

				// No matter the robot
				playerBody.Body.AddExplosionForce(
					200,
					other.ClosestPoint(playerBody.Body.position),
					1
				);

				this.gameObject.SetActive(false);
				// TODO: notify player? (if it has a list of seen mine)
				// Destroy(this.gameObject);
			}
		}

		void OnTriggerExit(Collider other)
		{
			// TODO?
		}

		// Player sees the mine and could de-mine it
		public void ShowAsUsabe()
		{
			this.asEditable.SetActive(true);
		}

		public void UnshowAsUsabe()
		{
			this.asEditable.SetActive(false);
		}

		// Player sees the mine
		public void Show()
		{
			// FIXME better
			this.Mine.enabled = true;
		}

		public void Hide(PlayerController player)
		{
			if (player == this.Player)
			{
				this.Show();
				return;
			}

			// FIXME better
			this.Mine.enabled = false;
		}
	}
}
