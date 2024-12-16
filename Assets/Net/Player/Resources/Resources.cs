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

		private void SetHealth(int health)
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

		private void SetMoney(int money)
		{
			if (money < 0)
			{
				this.SetMoney(0);
				return;
			}

			this.Money.Value = money;
		}

		/// <summary>
		/// Update health
		/// </summary>
		/// <param name="delta">health to add (use negativeto reduce)</param>
		[Rpc(SendTo.Server)]
		public void DeltaHealthRpc(int delta)
		{
			this.SetHealth(this.Health.Value + delta);
		}

		/// <summary>
		/// Update money
		/// </summary>
		/// <param name="delta">money to add (use negative to increase)</param>
		[Rpc(SendTo.Server)]
		public void ReduceMoneyRpc(int delta)
		{
			this.SetMoney(this.Money.Value - delta);
		}
	}
}
