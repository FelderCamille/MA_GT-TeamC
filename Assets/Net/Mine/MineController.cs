using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
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

		public ParticleSystem explosionEffect;

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
			if (other.TryGetComponent<PlayerBody>(out var playerBody))
			{
				if (this.firstInit)
				{
					// [Temporary]: avoid the explosion when setting the mine
					this.firstInit = false;
					return;
				}

				playerBody.player.WalkedOnMine(this);

				// TODO: Remove
				playerBody.Body.AddExplosionForce(
					100,
					other.ClosestPoint(playerBody.Body.position),
					1
				);

				StartCoroutine(this.ExplodeRoutine(playerBody.player));
			}
		}

		void OnTriggerExit(Collider other)
		{
			// TODO?
		}

		private IEnumerator ExplodeRoutine(PlayerController player)
		{
			this.HideMine();
			player.Game.SoundManager.PlayExplosionSound();
			this.explosionEffect.Play();

			yield return new WaitForSeconds(explosionEffect.main.duration - 1.5f);

			this.gameObject.SetActive(false);
			// Destroy(this.gameObject);
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

			this.HideMine();
		}

		private void HideMine()
		{
			// FIXME better
			this.Mine.enabled = false;
		}
	}
}
