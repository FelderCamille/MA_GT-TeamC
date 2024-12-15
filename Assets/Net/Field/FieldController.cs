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
				Debug.Log($"Enter Field {body.player.name}");
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent<PlayerBody>(out var body))
			{
				Debug.Log($"Leave Field {body.player.name}");
			}
		}
	}
}
