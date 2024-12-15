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
		public BaseUI.Ressources ressources;

		// TODO: remove (just temporary)
		public GameObject OnOwnFieldText;
		public GameObject OnEnemyFieldText;

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start() { }

		// Update is called once per frame
		void Update() { }
	}
}
