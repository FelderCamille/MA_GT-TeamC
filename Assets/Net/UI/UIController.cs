using UnityEngine;

namespace Net.UI
{
	/// <summary>
	///  A "controller" to centralize UI interactions.
	///  Works as a UI proxy
	/// </summary>
	public class UIController : MonoBehaviour
	{
		public EncyclopediaPanel panelEncyclopedia;
		public StorePanel panelStore;
		public QuestionPanel panelQuestion;

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		void Start() { }

		// Update is called once per frame
		void Update() { }
	}
}
