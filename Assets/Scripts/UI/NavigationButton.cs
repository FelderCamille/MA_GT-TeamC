using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace UI
{
	public class NavigationButton : MonoBehaviour
	{
		public Button button;

		public bool IsEnabled { get; private set; }

		public void Init(bool enable, Action onClickCallback)
		{
			button.onClick.AddListener(() => onClickCallback());
			if (enable)
			{
				IsEnabled = false;
				Enable();
			}
			else
			{
				IsEnabled = true;
				Disable();
			}
		}

		public void SetEnable(bool enable)
		{
			if (enable == IsEnabled)
				return;

			button.interactable = IsEnabled = enable;
		}

		public void Disable() => this.SetEnable(false);

		public void Enable() => this.SetEnable(true);
	}
}
