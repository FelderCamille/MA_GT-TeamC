using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Netcode;
using UnityEngine;

namespace Net
{
	public class PlayerController : NetworkBehaviour
	{
		public bool InGameCamera = false;

		// "In-game" camera
		public Camera MyCamera;

		// Arrow that inidcates that the player is looking at an interactive lenght of a mine
		public GameObject lookingAtAMineInd;

		[Header("The body of the player (with collision physics, ...)")]
		public PlayerBody Body;

		/// <summary>
		/// The camp of the player, set by the GameController
		/// </summary>
		internal CampController Camp;
		private GameController Game;

		void Start()
		{
			// Reverse relation
			this.Body.player = this;
		}

		public override void OnNetworkSpawn()
		{
			base.OnNetworkSpawn();

			Game = FindAnyObjectByType<GameController>();
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

		[Rpc(SendTo.Server)]
		void MineRpc()
		{
			if (this.editableMines.Count > 0)
			{
				Debug.Log("CAN not set a mine near another one");
				// TODO: better (check with collider)
				return;
			}

			Debug.Log("Mine here from here");
			// TODO: check (and use a Game manager)
			this.Game.SetMineRpc(this.Body.Body.position, this.Body.Body.rotation, true);
		}

		//
		void Update()
		{
			if (!this.IsLocalPlayer)
			{
				return;
			}

			// TODO: disable if other action (encyclopédia, ...)
			this.Body.HandleMovement();

			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.MineRpc();
			}

			if (Input.GetKeyDown(KeyCode.V))
			{
				this.hasVision = !this.hasVision;
			}

			// TODO: only temp
			List<MineController> toRemove = new();
			foreach (var mine in this.editableMines)
			{
				if (!mine.gameObject.activeSelf)
				{
					toRemove.Add(mine);
				}
			}
			foreach (var t in toRemove)
			{
				this.editableMines.Remove(t);
			}

			if (
				Physics.Raycast(
					Body.Body.position,
					Body.Body.rotation * Vector3.forward,
					out var hit,
					Mathf.Infinity
				)
			)
			{
				if (hit.collider.gameObject.TryGetComponent<MineController>(out var seeingMine))
				{
					foreach (var mineX in this.editableMines)
					{
						if (seeingMine == mineX)
						{
							if (this.currentlyEditable != null)
							{
								if (this.currentlyEditable == seeingMine)
								{
									// Same mine
									return;
								}

								this.currentlyEditable.UnshowAsUsabe();
							}

							this.lookingAtAMineInd.SetActive(true);
							this.currentlyEditable = seeingMine;
							seeingMine.ShowAsUsabe();
							return;
						}
					}
				}
			}

			if (this.currentlyEditable != null)
			{
				this.currentlyEditable.UnshowAsUsabe();
				this.currentlyEditable = null;
			}

			this.lookingAtAMineInd.SetActive(false);
		}

		private MineController currentlyEditable = null;

		// Bonus
		private bool hasVision = false;

		// Mine that could be demined // TODO: this is probably not optimum
		private List<MineController> editableMines = new();

		public void SeeMine(MineController mine, PlayerMineCollider.TYPE cType)
		{
			if (!this.IsLocalPlayer)
			{
				// Ignore other player
				return;
			}

			if (cType == PlayerMineCollider.TYPE.NORMAL)
			{
				this.editableMines.Add(mine);
			}

			if (cType == PlayerMineCollider.TYPE.EXTENDED && !this.hasVision)
			{
				return;
			}

			mine.MarkAsShown();
		}

		public void SeeNoMine(MineController mine, PlayerMineCollider.TYPE cType)
		{
			if (!this.IsLocalPlayer)
			{
				// Ignore other player
				return;
			}

			if (cType == PlayerMineCollider.TYPE.NORMAL)
			{
				this.editableMines.Remove(mine);
			}

			if (cType == PlayerMineCollider.TYPE.NORMAL && this.hasVision)
			{
				return;
			}

			mine.MarkAsUnshown();
		}
	}
}
