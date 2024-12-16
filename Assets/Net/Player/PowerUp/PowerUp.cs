using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Net
{
	// To manage "active bonus"
	// For simplicity (at least for now), only one bonus active at the same time

	public enum PowerUp
	{
		NONE,
		SPEED,
		RADAR,
		VISION,
	}

	// The resources are a game object so the network variables can be used
	public class PlayerPowerUps : NetworkBehaviour
	{
		// Do not set values from these
		public readonly NetworkVariable<PowerUp> PowerUP = new(PowerUp.NONE);

		/// <summary>
		/// A running bonus countdown
		/// </summary>
		public readonly NetworkVariable<uint> Countdown = new(0);

		// currently running countdown
		private Coroutine coroutine;

		public PowerUp powerUp
		{
			get => PowerUP.Value;
		}
		public bool hasActivePowerUp
		{
			get => powerUp != PowerUp.NONE;
		}

		[Rpc(SendTo.Server)]
		public void SetPowerUpRpc(PowerUp powerUp)
		{
			this.PowerUP.Value = powerUp;

			if (this.coroutine != null)
			{
				StopCoroutine(this.coroutine);
			}

			switch (powerUp)
			{
				case PowerUp.NONE:
					break;
				case PowerUp.SPEED:
					this.coroutine = StartCoroutine(
						this.CountdownTimer(30, () => this.SetPowerUpRpc(PowerUp.NONE))
					);
					break;

				case PowerUp.VISION:
					this.coroutine = StartCoroutine(
						this.CountdownTimer(10, () => this.SetPowerUpRpc(PowerUp.NONE))
					);
					break;

				case PowerUp.RADAR:
					this.coroutine = StartCoroutine(
						this.CountdownTimer(5, () => this.SetPowerUpRpc(PowerUp.NONE))
					);
					break;
			}
		}

		// Always run on the server
		private IEnumerator CountdownTimer(uint seconds, UnityAction callback)
		{
			for (uint s = 0; s <= seconds; ++s)
			{
				Countdown.Value = seconds - s;

				yield return new WaitForSeconds(1);
			}

			this.coroutine = null;
			callback();
		}
	}
}
