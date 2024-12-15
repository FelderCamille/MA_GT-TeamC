using UnityEngine;

namespace Net
{
	/// <summary>
	/// Script that detect mine for a player.
	/// Does not manage the explotion
	/// </summary>
	public class PlayerMineCollider : MonoBehaviour
	{
		public enum TYPE
		{
			[Header("It can interact with a mine")]
			INTERACTION,

			[Header("It can see a mine")]
			NORMAL,

			[Header("It can see a mine (with extension)")]
			EXTENDED,
		}

		[Header("Player to notify")]
		public PlayerController player;

		[Header("Type of notification")]
		public TYPE type;

		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<MineController>(out var mine))
			{
				this.player.SeeMine(mine, this.type);
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent<MineController>(out var mine))
			{
				this.player.SeeNoMine(mine, this.type);
			}
		}
	}
}
