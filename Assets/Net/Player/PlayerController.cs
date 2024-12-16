using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using Unity.Netcode;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Net
{
	// Identifier used through network
	// Mainly because "complex" object can not be syncronized
	public enum PlayerIdentifer
	{
		UNSET,
		PLAYER1,
		PLAYER2,
	}

	public class PlayerController : NetworkBehaviour
	{
		public bool InGameCamera = false;

		// "In-game" camera
		public Camera MyCamera;

		// Arrow that inidcates that the player is looking at an interactive lenght of a mine
		public GameObject lookingAtAMineInd;

		[Header("The body of the player (with collision physics, ...)")]
		public PlayerBody Body;

		[Header("The collider to detect if there is mines nearby (and forbid the mining)")]
		public CapsuleCollider NearByMines;

		[Header("Ressources object of the player")]
		public PlayerResources Resources;

		/// <summary>
		/// The camp of the player, set by the GameController
		/// </summary>
		internal CampController Camp;
		internal GameController Game;

		// Managed by Game
		internal NetworkVariable<PlayerIdentifer> identifier = new(PlayerIdentifer.UNSET);

		/// <summary>
		/// The mine that the player can currently interact (demine)
		/// </summary>
		private MineController MineInteractive = null;

		void Start() =>
			// Reverse relation
			this.Body.player = this;

		public override void OnNetworkSpawn()
		{
			base.OnNetworkSpawn();

			this.Game = FindAnyObjectByType<GameController>();
			Game.RegisterPlayer(this);

			// For the "in-game" camera
			if (InGameCamera)
			{
				this.MyCamera.gameObject.SetActive(this.IsLocalPlayer);
			}
		}

		public override void OnNetworkDespawn()
		{
			base.OnNetworkDespawn();
			// TODO
		}

		// TODO: as network variables (to known when the enemy is on "our" field)
		// There is 2 variables because if the camps have some shared space it can be the 2 at the same time
		private bool _isOnOwnField = false;
		private bool _isOnEnemyField = false;

		/// <summary>
		/// Is the player on its own camp/field (demine mode)
		/// </summary>
		public bool IsOnOwnField
		{
			// Updated by `FieldController`
			get => _isOnOwnField;
			internal set { this._isOnOwnField = value; } // TODO: Events (or remove and use `Update`) ?
		}

		/// <summary>
		/// Is the player on the enemy's camp/field (mine mode)
		/// </summary>
		public bool IsOnEnemyField
		{
			// Updated by `FieldController`
			get => _isOnEnemyField;
			internal set { this._isOnEnemyField = value; } // TODO: Events (or remove and use `Update`) ?
		}

		private bool _isNearBase = false;

		/// <summary>
		/// Is the player near its base (can open the store)
		/// </summary>
		public bool IsNearBase
		{
			// Updated by `BaseController`
			get => _isNearBase;
			internal set { this._isNearBase = value; } // TODO: Events (or remove and use `Update`) ?
		}

		[Rpc(SendTo.Server)]
		void LayMineRpc()
		{
			// The server has authority if the player can effectively set or not the mine

			if (!this.IsOnEnemyField && !this.Game.Configuration.PLAYER_OWN_FIELD_MINING)
			{
				Debug.Log("Can only set a mine on a enemy field");
				return;
			}

			var hits = Physics.SphereCastAll(
				NearByMines.transform.position,
				NearByMines.radius,
				new Vector3(1, 1, 1)
			);
			foreach (var hit in hits)
			{
				if (hit.collider.TryGetComponent<MineController>(out var mine))
				{
					Debug.Log("Can not set a mine near another one");
					return;
				}
			}

			this.Game.LayMine(this);
		}

		/// <summary>
		/// When a mine explose to the "player's feet"
		/// </summary>
		/// <param name="mine">The mine that exploded</param>
		public void WalkedOnMine(MineController mine)
		{
			if (this.IsServer)
			{
				// TODO: get correct value from mine + store for stats
				this.Resources.DeltaHealthRpc(-20);
				return;
			}
		}

		/// <summary>
		/// Handle the feedback/repercussion of demining
		/// </summary>
		/// <param name="mine">that the player answered</param>
		/// <param name="correct">was the response correct</param>
		public void DemineResult(MineController mine, bool correct)
		{
			this.MineInteractive = null;

			if (correct)
			{
				mine.ExtractMine();
				// TODO: score/stats
			}
			else
			{
				mine.Explode(this);

				if (this.IsServer)
				{
					// TODO: get correct value from mine + store for stats
					this.Resources.DeltaHealthRpc(-10);
				}
			}
		}

		void Update()
		{
			if (!this.IsLocalPlayer)
			{
				return;
			}

			this.lookingAtAMineInd.SetActive(this.MineInteractive != null);

			// Encyclopedia is always available?
			if (Input.GetKeyDown(KeyCode.Q))
			{
				this.Game.UIProxy.PanelEncyclopedia.Toogle();
			}

			if (IsNearBase && Input.GetKeyDown(KeyCode.E))
			{
				this.Game.UIProxy.PanelStore.Toogle(this);
			}
			if (this.MineInteractive != null && Input.GetKeyDown(KeyCode.E))
			{
				this.Game.UIProxy.PanelQuestion.Toogle(this, this.MineInteractive);
			}

			if (this.Game.UIProxy.HasPanelInteractiveOpen())
			{
				return;
			}

			this.Body.HandleMovement(Time.deltaTime);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.LayMineRpc();
			}
		}

		// TODO: FROM Bonus
		private bool hasVision = false;

		/// <summary>
		/// When a mine touches one of the player collider
		/// </summary>
		/// <param name="mine">the mine that enters the collider</param>
		/// <param name="type">the type of the collider</param>
		public void OnMineTriggerEnter(MineController mine, PlayerMineCollider.TYPE type)
		{
			if (!this.IsLocalPlayer)
			{
				return;
			}

			if (type == PlayerMineCollider.TYPE.INTERACTION)
			{
				// TODO: better (for now, let's suppose that it can not have multiple mine here)
				this.MineInteractive = mine;
				return;
			}

			if (type == PlayerMineCollider.TYPE.EXTENDED && !this.hasVision)
			{
				// Vision is not set
				return;
			}

			mine.Show();
		}

		/// <summary>
		/// When a mine exits one of the player collider
		/// </summary>
		/// <param name="mine">the mine that exits the collider</param>
		/// <param name="type">the type of the collider</param>
		public void OnMineTriggerExit(MineController mine, PlayerMineCollider.TYPE type)
		{
			if (!this.IsLocalPlayer)
			{
				return;
			}

			if (type == PlayerMineCollider.TYPE.INTERACTION)
			{
				this.MineInteractive = null;
				return;
			}

			if (type == PlayerMineCollider.TYPE.NORMAL && this.hasVision)
			{
				// Only hide with from extended if vision
				return;
			}

			mine.Hide(this);
		}
	}
}
