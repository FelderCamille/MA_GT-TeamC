using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Net.UI
{
	/// <summary>
	///  A "controller" to centralize UI interactions.
	///  Works as a UI proxy
	/// </summary>
	public class UIController : MonoBehaviour
	{
		[Header("The sound manager")]
		public SoundManager SoundManager;

		public EncyclopediaPanel PanelEncyclopedia;
		public StorePanel PanelStore;
		public QuestionPanel PanelQuestion;
		public HUDPanel PanelHUD;

		private readonly List<PanelInteractive> panelInteractives = new();

		void Awake()
		{
			panelInteractives.Add(this.PanelEncyclopedia);
			panelInteractives.Add(this.PanelStore);
			panelInteractives.Add(this.PanelQuestion);

			foreach (var panel in panelInteractives)
			{
				panel.SoundManager = this.SoundManager;
			}
		}

		/// <returns>If any interactive panel is open</returns>
		public bool HasPanelInteractiveOpen()
		{
			foreach (var panel in panelInteractives)
			{
				if (panel.IsOpen())
				{
					return true;
				}
			}

			return false;
		}
	}
}
