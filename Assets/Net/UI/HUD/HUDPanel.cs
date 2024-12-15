using UnityEngine;
using BaseUI = UI;

namespace Net.UI
{
	/// <summary>
	///  A "controller" to manage the 'HUD' panel.
	///  The HUD (could) contains score, money, actions ....
	/// </summary>
	public class HUDPanel : MonoBehaviour
	{
		[Header("Resources of Player 1 (local)")]
		public BaseUI.Ressources ressourcesP1;

		[Header("Resources of Player 2 (remote)")]
		public BaseUI.Ressources ressourcesP2;

		// TODO: remove (just temporary)
		public GameObject OnOwnFieldText;
		public GameObject OnEnemyFieldText;

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start() { }

		// Update is called once per frame
		void Update() { }
	}
}
