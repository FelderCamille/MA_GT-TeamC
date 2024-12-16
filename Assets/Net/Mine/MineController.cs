using System.Collections;
using System.Data.Common;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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

		/// <summary>
		/// Game on which the mine is set
		/// </summary>
		internal GameController Game;

		/// <summary>
		/// Unique id set to the mine (mostly for debug)
		/// </summary>
		internal uint mineId;

		public override void OnNetworkSpawn()
		{
			base.OnNetworkSpawn();

			this.Game = FindAnyObjectByType<GameController>();
			this.DebugIndicator.SetActive(Game.Configuration.SHOW_DEBUG_MINES);
		}

		public override void OnNetworkDespawn()
		{
			base.OnNetworkDespawn();
			// TODO?
		}

		[Rpc(SendTo.Everyone)]
		internal void InitRpc(PlayerIdentifer playerId, uint mineId)
		{
			// Hide by default
			this.HideMine();
			this.mineId = mineId;
			this.name = $"Mine ${mineId}";

			if (playerId == PlayerIdentifer.UNSET)
			{
				Debug.Log("No player assigned to this mine");
				return;
			}

			this.Player = playerId == PlayerIdentifer.PLAYER1 ? Game.Player1 : Game.Player2;
			if (this.Player == Game.PlayerLocal)
			{
				// Only show to the LocalPlayer if he is the owner
				this.Show();
			}

			// TODO: better
			firstInit = true;
		}

		// TODO: the question of this mine

		// FIXME: better
		private bool firstInit = false;

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

				var player = playerBody.player;
				player.WalkedOnMine(this);

				// TODO: Remove?
				playerBody.Body.AddExplosionForce(
					100,
					other.ClosestPoint(playerBody.Body.position),
					1
				);

				this.Explode(player);
			}
		}

		void OnTriggerExit(Collider other)
		{
			// TODO?
		}

		/// <summary>
		/// Run an explosion animation (and removes the object)
		/// </summary>
		/// <param name="player">Player that trigger the explosion (if there is any animation)</param>
		public void Explode(PlayerController player)
		{
			this.Explode();
		}

		/// <summary>
		/// Run an explosion animation (and removes the object)
		/// </summary>
		public void Explode()
		{
			this.ExplodeRpc();
		}

		// Send the explosion to everyone
		[Rpc(SendTo.Everyone)]
		private void ExplodeRpc()
		{
			StartCoroutine(this.ExplodeRoutine());
		}

		/// <summary>
		/// When the mine is corretly removed (successful demining)
		/// </summary>
		public void ExtractMine()
		{
			this.ExtractMineRpc();
		}

		// Send the "cleaning" to everyone
		[Rpc(SendTo.Everyone)]
		private void ExtractMineRpc()
		{
			// TODO: animations (even if only the currently could see it)?

			this.HideMine();
			this.destroyMine();
		}

		private IEnumerator ExplodeRoutine()
		{
			// TODO: get (or keep) the player that triggered the explosion (if any) and apply some animations ?
			this.HideMine();

			// TODO: get general?
			this.Game.SoundManager.PlayExplosionSound();

			this.explosionEffect.Play();
			yield return new WaitForSeconds(explosionEffect.main.duration - 1.5f);

			this.destroyMine();
		}

		private void destroyMine()
		{
			this.gameObject.SetActive(false);

			if (this.IsServer)
			{
				var instanceNetworkObject = this.GetComponent<NetworkObject>();
				instanceNetworkObject.Despawn();
			}
		}

		// TODO: Clean

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
