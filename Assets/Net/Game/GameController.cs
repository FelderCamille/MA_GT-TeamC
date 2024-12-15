using System.Collections.Generic;
using Core;
using Unity.Netcode;
using UnityEngine;
using BaseUI = UI;

namespace Net
{
	/// <summary>
	/// The Game Controller works as a proxy (to get all necessary game objects).
	/// And the main logic unit (even if player does a lot too)
	/// </summary>
	public class GameController : NetworkBehaviour
	{
		// Global "configuration"
		[Header("The sound manager")]
		public SoundManager SoundManager;

		[Header("Global UI controller")]
		public UI.UIController UIProxy;

		// Gameplay content
		[Header("Camp for player 1")]
		public CampController Camp1;

		[Header("Camp for player 2")]
		public CampController Camp2;

		[Header("Mine prefab")]
		public MineController MinePrefab;

		[Header(
			"Position to spawn the initial mines when the 2 players are connected. (for player 1)"
		)]
		public List<Transform> MineInitialsPlayer1 = new();

		[Header(
			"Position to spawn the initial mines when the 2 players are connected (for player 2)"
		)]
		public List<Transform> MineInitialsPlayer2 = new();

		// The registered players
		private PlayerController Player1;
		private PlayerController Player2;

		// The local player (to interact with, ...) // TODO: remove ?
		private PlayerController PlayerLocal;

		private HUDRessourcesPlayer HUDRessources1;
		private HUDRessourcesPlayer HUDRessources2;

		public override void OnNetworkDespawn()
		{
			base.OnNetworkDespawn();

			if (this.HUDRessources1 != null)
			{
				this.HUDRessources1.Clear();
			}
			if (this.HUDRessources2 != null)
			{
				this.HUDRessources2.Clear();
			}
		}

		/// <summary>
		/// Store the player and affects its camp
		/// </summary>
		/// <param name="player"></param>
		public void RegisterPlayer(PlayerController player)
		{
			if (this.Player1 == null)
			{
				// Mostly for log/debug
				player.name = "Player 1";
				player.Camp = this.Camp1;

				this.Player1 = player;
			}
			else if (this.Player2 == null)
			{
				// Mostly for log/debug
				player.name = "Player 2";
				player.Camp = this.Camp2;

				this.Player2 = player;

				// TODO: start game (and "lock")
			}

			var spawnPlayer = player.Camp.spawnPosition;
			player.Body.Body.position = spawnPlayer.position;
			player.Body.Body.rotation = spawnPlayer.rotation;

			HUDRessourcesPlayer hud = player.IsLocalPlayer
				? (
					this.HUDRessources1 = new HUDRessourcesPlayer(
						player,
						this.UIProxy.PanelHUD.ressourcesP1
					)
				)
				: (
					this.HUDRessources2 = new HUDRessourcesPlayer(
						player,
						this.UIProxy.PanelHUD.ressourcesP2
					)
				);
			hud.Init();
		}

		void LateUpdate()
		{
			if (this.PlayerLocal == null)
			{
				return;
			}

			// TODO: remove (just temporary)

			var HUD = this.UIProxy.PanelHUD;
			HUD.OnOwnFieldText.SetActive(this.PlayerLocal.IsOnOwnField);
			HUD.OnEnemyFieldText.SetActive(this.PlayerLocal.IsOnEnemyField);
		}
	}

	public class HUDRessourcesPlayer
	{
		private readonly PlayerController player;
		private readonly BaseUI.Ressources ressources;

		public HUDRessourcesPlayer(PlayerController player, BaseUI.Ressources ressources)
		{
			this.player = player;
			this.ressources = ressources;
		}

		public void Init()
		{
			var resources = player.Resources;
			resources.Health.OnValueChanged += this.OnHealthChange;
			resources.Money.OnValueChanged += this.OnMoneyChange;

			this.OnMoneyChange(resources.Health.Value, resources.Health.Value);
			this.OnHealthChange(resources.Money.Value, resources.Money.Value);
		}

		public void Clear()
		{
			var resources = player.Resources;
			resources.Money.OnValueChanged -= this.OnMoneyChange;
			resources.Health.OnValueChanged -= this.OnHealthChange;
		}

		private void OnHealthChange(int previous, int current)
		{
			this.ressources.SetHealth((int)current);
		}

		private void OnMoneyChange(int previous, int current)
		{
			this.ressources.SetMoney((int)current);
		}
	}
}
