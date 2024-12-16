using Core;
using UnityEngine;

namespace Net.UI
{
	/// <summary>
	///  A "controller" to manage the 'store' panel
	/// </summary>
	public class StorePanel : PanelInteractive
	{
		public override bool IsOpen()
		{
			return this.gameObject.activeSelf;
		}

		public override void Close()
		{
			this.gameObject.SetActive(false);
		}

		public void Open(PlayerController player)
		{
			// TODO

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

		void Start() { }

		void Update() { }
	}
}
