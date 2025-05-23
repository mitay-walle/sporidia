using GameJam.Plugins.UI.Input;
using UnityEngine;

namespace GameJam.Plugins.UI.Windows
{
	/// <summary>
	/// can cahve hotkey to Toggle-show
	/// </summary>
	public abstract class Panel : MonoBehaviour, IPanel
	{
		public virtual bool IsVisible => gameObject.activeSelf;
		[SerializeField] private bool _needCursor;

		// public virtual void Show() => gameObject.SetActive(true);
		// public virtual void Hide() => gameObject.SetActive(false);
		public void Toggle() => Toggle(!IsVisible);

		public void Toggle(bool value)
		{
			if (value)
			{
				Show();
			}
			else
			{
				Hide();
			}
		}

		public virtual void Show()
		{
			// Debug.LogError($"Show {gameObject}");
			if (_needCursor) CursorControl.Instance.Enablers.Add(this);
			gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			if (_needCursor) CursorControl.Instance.Enablers.Remove(this);
			// Debug.LogError($"Hide {gameObject}");
			gameObject.SetActive(false);
		}
	}
}