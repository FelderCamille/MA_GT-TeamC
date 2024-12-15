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

		void OnTriggerEnter(Collider other)
		{
			// Only show the base as "editable" for the localplayer
			// TODO: only the local player and for example to the player1 is this is base1
			if (
				other.TryGetComponent<PlayerController>(out var player)
				&& player.IsLocalPlayer
				&& player.Camp.baseController == this
			)
			{
				this.truc.SetActive(true);
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (
				other.TryGetComponent<PlayerController>(out var player)
				&& player.IsLocalPlayer
				&& player.Camp.baseController == this
			)
			{
				this.truc.SetActive(false);
			}
		}
	}
}
