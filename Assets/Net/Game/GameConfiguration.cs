using Unity.Netcode;
using UnityEngine;

namespace Net
{
	// Mostly for debug/dev
	public class GameConfiguration : NetworkBehaviour
	{
		// Global "configuration"
		[Header("Show all mines (with its indicator)")]
		public bool SHOW_DEBUG_MINES = Constants.DebugShowMines;

		[Header("Allow a player to demine its own mines (no need of 2nd player for testing)")]
		public bool PLAYER_SELF_DEMINING = false;

		[Header("Allow a player to mine its own field")]
		public bool PLAYER_OWN_FIELD_MINING = false;
	}
}
