using Core;
using UnityEngine;

namespace Net.UI
{
	/// <summary>
	///  A "controller" to centralize UI interactions.
	///  Works as a UI proxy
	/// </summary>
	public abstract class PanelInteractive : MonoBehaviour
	{
		[Header("The sound manager")]
		public SoundManager SoundManager;

		/// <summary>
		/// Is the panel currently open
		/// </summary>
		public abstract bool IsOpen();

		/// <summary>
		/// Simply close a panel (not affect on the palyer)
		/// </summary>
		public abstract void Close();
	}
}
