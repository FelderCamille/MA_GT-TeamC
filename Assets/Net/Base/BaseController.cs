using Unity.Netcode;
using UnityEngine;

namespace Net
{
	public class BaseController : MonoBehaviour
	{
		// Show "availibility"
		public GameObject truc;

		void Start()
		{
			this.truc.SetActive(false);
		}

		void OnTriggerEnter(Collider collider)
		{
			if (this.GetPlayer(collider, out var player))
			{
				this.truc.SetActive(player.IsNearBase = true);
			}
		}

		void OnTriggerExit(Collider collider)
		{
			if (this.GetPlayer(collider, out var player))
			{
				this.truc.SetActive(player.IsNearBase = false);
			}
		}

		// Get the player, owner of the base
		private bool GetPlayer(Collider other, out PlayerController player)
		{
			if (other.TryGetComponent<PlayerBody>(out var body))
			{
				player = body.player;
				return player.IsLocalPlayer && player.Camp.baseController == this;
			}

			player = null;
			return false;
		}
	}
}
