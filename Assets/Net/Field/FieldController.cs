using UnityEngine;

namespace Net
{
	/// <summary>
	/// The field is the "playable" space.
	/// It can be friendly or enemy
	/// </summary>
	public class FieldController : MonoBehaviour
	{
		void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<PlayerBody>(out var body))
			{
				this.UpdateFieldStatus(body.player, true);
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent<PlayerBody>(out var body))
			{
				this.UpdateFieldStatus(body.player, false);
			}
		}

		private void UpdateFieldStatus(PlayerController player, bool entering)
		{
			if (player.Camp.field == this)
			{
				player.IsOnOwnField = entering;
			}
			else
			{
				player.IsOnEnemyField = entering;
			}
		}
	}
}
