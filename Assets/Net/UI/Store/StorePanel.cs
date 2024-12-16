using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Net.UI
{
	/// <summary>
	///  A "controller" to manage the 'store' panel
	/// </summary>
	public class StorePanel : PanelInteractive
	{
		// TODO Better
		public Button ItemRepair50;
		public Button ItemRepair100;

		public Button ItemVision;
		public Button ItemSpeed;
		public Button ItemRadar;

		private readonly List<Button> items = new();

		void Start()
		{
			items.Add(ItemRepair50);
			items.Add(ItemRepair100);
			items.Add(ItemVision);
			items.Add(ItemSpeed);
			items.Add(ItemRadar);
		}

		public override bool IsOpen()
		{
			return this.gameObject.activeSelf;
		}

		public override void Close()
		{
			foreach (var item in items)
			{
				item.onClick.RemoveAllListeners();
			}

			this.gameObject.SetActive(false);
		}

		public void Open(PlayerController player)
		{
			var resources = player.Resources;
			var powerUps = player.PowerUps;
			var money = resources.Money.Value;

			// TODO: better

			// Add all listerners
			this.ItemRepair50.onClick.AddListener(() =>
			{
				resources.ReduceMoneyRpc(200);
				resources.DeltaHealthRpc(50);
				this.Close();
			});
			this.ItemRepair100.onClick.AddListener(() =>
			{
				resources.ReduceMoneyRpc(350);
				resources.DeltaHealthRpc(100);
				this.Close();
			});

			// Buying another bonus kills the previous
			this.ItemVision.onClick.AddListener(() =>
			{
				resources.ReduceMoneyRpc(500);
				powerUps.SetPowerUpRpc(PowerUp.VISION);
				this.Close();
			});
			this.ItemSpeed.onClick.AddListener(() =>
			{
				resources.ReduceMoneyRpc(500);
				powerUps.SetPowerUpRpc(PowerUp.SPEED);
				this.Close();
			});
			this.ItemRadar.onClick.AddListener(() =>
			{
				resources.ReduceMoneyRpc(1000);
				powerUps.SetPowerUpRpc(PowerUp.RADAR);
				this.Close();
			});

			// disable if not enough money // TODO: with real values
			this.ItemRepair50.interactable = money >= 200;
			this.ItemRepair100.interactable = money >= 350;

			var hasActiveBonus = powerUps.hasActivePowerUp;
			this.ItemVision.interactable = money >= 500;
			this.ItemSpeed.interactable = money >= 500;
			this.ItemRadar.interactable = money >= 1000;

			// Remove for the un-usable ones
			foreach (var item in items)
			{
				if (!item.interactable)
				{
					item.onClick.RemoveAllListeners();
				}
			}
			this.gameObject.SetActive(true);
		}

		/// <summary>
		/// Simply toggles the state of the panel
		/// </summary>
		/// <param name="player">The player that opens the store (to update value/disable bonuses/...)</param>
		public void Toogle(PlayerController player)
		{
			if (this.IsOpen())
			{
				this.Close();
			}
			else
			{
				this.Open(player);
			}
		}

		void Update() { }
	}
}
