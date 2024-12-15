using UnityEngine;

namespace Net.UI
{
	/// <summary>
	///  A "controller" to centralize UI interactions.
	///  Works as a UI proxy
	/// </summary>
	public class UIController : MonoBehaviour
	{
		public EncyclopediaPanel PanelEncyclopedia;
		public StorePanel PanelStore;
		public QuestionPanel PanelQuestion;
		public HUDPanel PanelHUD;
	}
}
