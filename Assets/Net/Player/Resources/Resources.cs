using System.Data.SqlTypes;
using Mono.Cecil.Cil;
using Unity.Netcode;

namespace Net
{
	// The resources are a game object so the network variables can be used
	public class PlayerResources : NetworkBehaviour
	{
		public readonly int HEALTH_MAX = (int)Constants.GameSettings.Health;

		// Do not set values from these
		public readonly NetworkVariable<int> Money = new(Constants.GameSettings.Money);
		public readonly NetworkVariable<int> Health = new((int)Constants.GameSettings.Health);

		public void SetHealth(int health)
		{
			if (health < 0)
			{
				this.SetHealth(0);
				return;
			}
			if (health > this.HEALTH_MAX)
			{
				this.SetHealth(this.HEALTH_MAX);
				return;
			}

			this.Health.Value = health;
		}

		/// <summary>
		/// Update health
		/// </summary>
		/// <param name="health">health to add (use negativeto reduce)</param>
		[Rpc(SendTo.Server)]
		public void DeltaHealthRpc(int health)
		{
			this.SetHealth(this.Health.Value + health);
		}
	}
}
