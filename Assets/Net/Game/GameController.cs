using System.Collections.Generic;
using Core;
using Unity.Netcode;
using UnityEngine;

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
		public UI.UIController uM;

		// Gameplay content
		[Header("Camp for player 1")]
		public CampController Camp1;

		[Header("Camp for player 2")]
		public CampController Camp2;

		[Header("Mine prefab")]
		public MineController Mine;

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
		}

		[Rpc(SendTo.Everyone)]
		public void SetMineRpc(Vector3 position, Quaternion rotation, bool firstInit)
		{
			Debug.Log("Mine here");
			// TODO: set player owner

			var mine = Instantiate(this.Mine, position, rotation);
			// TODO: better
			mine.firstInit = firstInit;
		}
	}
}
